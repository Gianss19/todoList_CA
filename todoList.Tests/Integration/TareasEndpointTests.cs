using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace todoList.Tests.Integration;

public class TareasEndpointTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public TareasEndpointTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    private async Task<(HttpClient authClient, Guid userId)> SetupAuthenticatedUserAsync(string nombre = "TareaUser")
    {
        var setupClient = _factory.CreateClient();
        var (userId, token) = await _factory.CreateAndLoginUserAsync(setupClient, nombre);
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return (client, userId);
    }

    [Fact]
    public async Task CrearTarea_DatosValidos_Retorna201()
    {
        var (authClient, userId) = await SetupAuthenticatedUserAsync();

        var response = await authClient.PostAsJsonAsync("/api/tareas", new
        {
            nombre = "Mi nueva tarea",
            usuario_id = userId
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var tarea = await response.Content.ReadFromJsonAsync<JsonElement>();
        tarea.GetProperty("nombre").GetString().Should().Be("Mi nueva tarea");
        tarea.GetProperty("isCompleted").GetBoolean().Should().BeFalse();
    }

    [Fact]
    public async Task CrearTarea_NombreDuplicado_Retorna409()
    {
        var (authClient, userId) = await SetupAuthenticatedUserAsync("DupUser");

        await authClient.PostAsJsonAsync("/api/tareas", new
        {
            nombre = "Tarea Dup",
            usuario_id = userId
        });

        var response = await authClient.PostAsJsonAsync("/api/tareas", new
        {
            nombre = "Tarea Dup",
            usuario_id = userId
        });

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task CrearTarea_UsuarioNoExiste_Retorna409()
    {
        var (authClient, _) = await SetupAuthenticatedUserAsync("NoExisteUser");

        var response = await authClient.PostAsJsonAsync("/api/tareas", new
        {
            nombre = "Tarea sin usuario valido",
            usuario_id = Guid.NewGuid()
        });

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task ObtenerTodas_HayTareas_Retorna200ConLista()
    {
        var (authClient, userId) = await SetupAuthenticatedUserAsync("GetAllUser");

        await authClient.PostAsJsonAsync("/api/tareas", new
        {
            nombre = "Tarea 1",
            usuario_id = userId
        });

        var response = await authClient.GetAsync("/api/tareas");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tareas = await response.Content.ReadFromJsonAsync<List<JsonElement>>();
        tareas.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ObtenerTodas_SinAuth_Retorna401()
    {
        var response = await _client.GetAsync("/api/tareas");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task ObtenerPorId_Existe_Retorna200()
    {
        var (authClient, userId) = await SetupAuthenticatedUserAsync("GetByIdUser");

        var createResp = await authClient.PostAsJsonAsync("/api/tareas", new
        {
            nombre = "Tarea para obtener",
            usuario_id = userId
        });
        var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        var id = created.GetProperty("id").GetGuid();

        var response = await authClient.GetAsync($"/api/tareas/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tarea = await response.Content.ReadFromJsonAsync<JsonElement>();
        tarea.GetProperty("id").GetGuid().Should().Be(id);
    }

    [Fact]
    public async Task ObtenerPorId_NoExiste_Retorna404()
    {
        var (authClient, _) = await SetupAuthenticatedUserAsync("GetNotFoundUser");

        var response = await authClient.GetAsync($"/api/tareas/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CompletarTarea_Existe_Retorna200()
    {
        var (authClient, userId) = await SetupAuthenticatedUserAsync("CompletarUser");

        var createResp = await authClient.PostAsJsonAsync("/api/tareas", new
        {
            nombre = "Tarea a completar",
            usuario_id = userId
        });
        var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        var id = created.GetProperty("id").GetGuid();

        var response = await authClient.PostAsync($"/api/tareas/{id}/completar", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tarea = await response.Content.ReadFromJsonAsync<JsonElement>();
        tarea.GetProperty("isCompleted").GetBoolean().Should().BeTrue();
    }

    [Fact]
    public async Task CompletarTarea_NoExiste_Retorna404()
    {
        var (authClient, _) = await SetupAuthenticatedUserAsync("CompletarNotFoundUser");

        var response = await authClient.PostAsync($"/api/tareas/{Guid.NewGuid()}/completar", null);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CambiarNombreTarea_Existe_Retorna200()
    {
        var (authClient, userId) = await SetupAuthenticatedUserAsync("RenameUser");

        var createResp = await authClient.PostAsJsonAsync("/api/tareas", new
        {
            nombre = "Nombre viejo",
            usuario_id = userId
        });
        var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        var id = created.GetProperty("id").GetGuid();

        var response = await authClient.PatchAsJsonAsync($"/api/tareas/{id}/nombre", new
        {
            id,
            nuevoNombre = "Nombre nuevo"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tarea = await response.Content.ReadFromJsonAsync<JsonElement>();
        tarea.GetProperty("nuevoNombre").GetString().Should().Be("Nombre nuevo");
    }

    [Fact]
    public async Task CambiarNombreTarea_NoExiste_Retorna404()
    {
        var (authClient, _) = await SetupAuthenticatedUserAsync("RenameNotFoundUser");

        var response = await authClient.PatchAsJsonAsync($"/api/tareas/{Guid.NewGuid()}/nombre", new
        {
            id = Guid.NewGuid(),
            nuevoNombre = "Nuevo"
        });

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task BorrarTarea_Existe_Retorna204()
    {
        var (authClient, userId) = await SetupAuthenticatedUserAsync("DeleteUser");

        var createResp = await authClient.PostAsJsonAsync("/api/tareas", new
        {
            nombre = "Tarea a borrar",
            usuario_id = userId
        });
        var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        var id = created.GetProperty("id").GetGuid();

        var response = await authClient.DeleteAsync($"/api/tareas/{id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task BorrarTarea_NoExiste_Retorna404()
    {
        var (authClient, _) = await SetupAuthenticatedUserAsync("DeleteNotFoundUser");

        var response = await authClient.DeleteAsync($"/api/tareas/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task FlujoCompleto_CrearObtenerCompletarBorrar()
    {
        var (authClient, userId) = await SetupAuthenticatedUserAsync("FlujoUser");

        var createResp = await authClient.PostAsJsonAsync("/api/tareas", new
        {
            nombre = "Tarea del flujo",
            usuario_id = userId
        });
        createResp.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createResp.Content.ReadFromJsonAsync<JsonElement>();
        var id = created.GetProperty("id").GetGuid();

        var getResp = await authClient.GetAsync($"/api/tareas/{id}");
        getResp.StatusCode.Should().Be(HttpStatusCode.OK);

        var completeResp = await authClient.PostAsync($"/api/tareas/{id}/completar", null);
        completeResp.StatusCode.Should().Be(HttpStatusCode.OK);
        var completed = await completeResp.Content.ReadFromJsonAsync<JsonElement>();
        completed.GetProperty("isCompleted").GetBoolean().Should().BeTrue();

        var deleteResp = await authClient.DeleteAsync($"/api/tareas/{id}");
        deleteResp.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getAfterDelete = await authClient.GetAsync($"/api/tareas/{id}");
        getAfterDelete.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

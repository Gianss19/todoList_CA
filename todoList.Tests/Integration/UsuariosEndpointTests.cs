using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace todoList.Tests.Integration;

public class UsuariosEndpointTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public UsuariosEndpointTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CrearUsuario_DatosValidos_Retorna201()
    {
        var response = await _client.PostAsJsonAsync("/api/usuarios/crear", new
        {
            nombre = "Nuevo Usuario",
            correo = $"nuevo_{Guid.NewGuid():N}@test.com",
            password = "Password123!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CrearUsuario_CorreoDuplicado_Retorna400()
    {
        var correo = $"dup_{Guid.NewGuid():N}@test.com";
        await _client.PostAsJsonAsync("/api/usuarios/crear", new
        {
            nombre = "Usuario Dup",
            correo,
            password = "Password123!"
        });

        var response = await _client.PostAsJsonAsync("/api/usuarios/crear", new
        {
            nombre = "Usuario Dup 2",
            correo,
            password = "Password123!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task ObtenerTodos_Admin_Retorna200ConLista()
    {
        var adminClient = await _factory.CreateAdminClientAsync();

        var response = await adminClient.GetAsync("/api/usuarios/all");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var usuarios = await response.Content.ReadFromJsonAsync<List<object>>();
        usuarios.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task ObtenerTodos_NoAdmin_Retorna403()
    {
        var userClient = await _factory.CreateAuthenticatedClientAsync("RegularUser");

        var response = await userClient.GetAsync("/api/usuarios/all");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task ObtenerTodos_SinAuth_Retorna401()
    {
        var response = await _client.GetAsync("/api/usuarios/all");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DesactivarUsuario_Admin_Retorna200()
    {
        var adminClient = await _factory.CreateAdminClientAsync();
        var (userId, _, _) = await _factory.RegisterUserAsync(_client, "DesactivarUser");

        var response = await adminClient.PatchAsync($"/api/usuarios/{userId}/desactivar", null);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DesactivarUsuario_NoAdmin_Retorna403()
    {
        var userClient = await _factory.CreateAuthenticatedClientAsync("RegularUser");

        var response = await userClient.PatchAsync("/api/usuarios/00000000-0000-0000-0000-000000000001/desactivar", null);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task ActivarUsuario_Admin_Retorna204()
    {
        var adminClient = await _factory.CreateAdminClientAsync();
        var (userId, _, _) = await _factory.RegisterUserAsync(_client, "ActivarUser");

        await adminClient.PatchAsync($"/api/usuarios/{userId}/desactivar", null);
        var response = await adminClient.PatchAsync($"/api/usuarios/{userId}/activar", null);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task BorrarUsuario_Admin_Retorna204()
    {
        var adminClient = await _factory.CreateAdminClientAsync();
        var (userId, _, _) = await _factory.RegisterUserAsync(_client, "BorrarUser");

        var response = await adminClient.DeleteAsync($"/api/usuarios/{userId}/borrar");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task BorrarUsuario_NoAdmin_Retorna403()
    {
        var userClient = await _factory.CreateAuthenticatedClientAsync("RegularUser");

        var response = await userClient.DeleteAsync("/api/usuarios/00000000-0000-0000-0000-000000000001/borrar");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task CambiarNombre_UsuarioOwn_Retorna200()
    {
        var setupClient = _factory.CreateClient();
        var (userId, token) = await _factory.CreateAndLoginUserAsync(setupClient, "CambiarNombreUser");

        var authClient = _factory.CreateClient();
        authClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await authClient.PatchAsJsonAsync($"/api/usuarios/{userId}/cambiar-nombre", new
        {
            id = userId,
            nuevoNombre = "NombreNuevo"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CambiarContraseña_UsuarioOwn_Retorna204()
    {
        var setupClient = _factory.CreateClient();
        var (userId, token) = await _factory.CreateAndLoginUserAsync(setupClient, "CambiarPwUser", password: "OldPass123!");

        var authClient = _factory.CreateClient();
        authClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await authClient.PatchAsJsonAsync($"/api/usuarios/{userId}/cambiar-contraseña", new
        {
            id = userId,
            nuevaPassword = "NewPass123!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task CambiarContraseña_OtroUsuario_Retorna403()
    {
        var setupClient = _factory.CreateClient();
        var (userId, token) = await _factory.CreateAndLoginUserAsync(setupClient, "OwnerUser");

        var authClient = _factory.CreateClient();
        authClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var otherUserId = Guid.NewGuid();
        var response = await authClient.PatchAsJsonAsync($"/api/usuarios/{otherUserId}/cambiar-contraseña", new
        {
            id = otherUserId,
            nuevaPassword = "NewPass123!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace todoList.Tests.Integration;

public class LoginEndpointTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public LoginEndpointTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_CredencialesValidas_RetornaToken()
    {
        var correo = $"loginuser_{Guid.NewGuid():N}@test.com";
        await _client.PostAsJsonAsync("/api/usuarios/crear", new
        {
            nombre = "LoginUser",
            correo,
            password = "Test123!"
        });

        var response = await _client.PostAsJsonAsync("/api/login", new
        {
            email = correo,
            password = "Test123!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var token = (await response.Content.ReadAsStringAsync()).Trim('"');
        token.Should().NotBeNullOrEmpty();
        token.Should().Contain(".");
    }

    [Fact]
    public async Task Login_CredencialesValidas_RetornaToken200()
    {
        var (_, token) = await _factory.CreateAndLoginUserAsync(_client, "LoginTest");

        token.Should().NotBeNullOrEmpty();
        token.Should().Contain(".");
    }

    [Fact]
    public async Task Login_CredencialesInvalidas_Retorna404()
    {
        var response = await _client.PostAsJsonAsync("/api/login", new
        {
            email = "noexiste@test.com",
            password = "wrongpassword"
        });

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Login_PasswordIncorrecto_Retorna401()
    {
        var correo = $"loginuser_{Guid.NewGuid():N}@test.com";
        await _client.PostAsJsonAsync("/api/usuarios/crear", new
        {
            nombre = "LoginUser",
            correo,
            password = "CorrectPass123!"
        });

        var response = await _client.PostAsJsonAsync("/api/login", new
        {
            email = correo,
            password = "WrongPass!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_BodyNulo_Retorna400()
    {
        var response = await _client.PostAsJsonAsync("/api/login", (object?)null);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

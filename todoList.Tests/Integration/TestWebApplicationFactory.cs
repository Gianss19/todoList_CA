using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using todoList.Domain;
using todoList.Infrastructure.Persistence;

namespace todoList.Tests.Integration;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = $"TodoListTest_{Guid.NewGuid():N}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var dbOptionsDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<TodoListDbContext>));
            if (dbOptionsDescriptor != null)
                services.Remove(dbOptionsDescriptor);

            var dbDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(TodoListDbContext));
            if (dbDescriptor != null)
                services.Remove(dbDescriptor);

            services.AddDbContext<TodoListDbContext>(options =>
                options.UseInMemoryDatabase(_dbName));
        });

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.Sources.Clear();
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:SecretKey"] = "TestSecretKey_12345678901234567890!",
                ["Jwt:Issuer"] = "TestIssuer",
                ["Jwt:Audience"] = "TestAudience",
                ["ConnectionStrings:DefaultConnection"] = ""
            });
        });
    }

    public HttpClient CreateClientWithToken(Guid userId, string nombre, string correo, string rol)
    {
        var client = CreateClient();
        var token = GenerateJwtToken(userId, nombre, correo, rol);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    private string GenerateJwtToken(Guid id, string nombre, string correo, string rol)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TestSecretKey_12345678901234567890!"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Name, nombre),
            new Claim(ClaimTypes.Email, correo),
            new Claim(ClaimTypes.Role, rol)
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            SigningCredentials = creds
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }

    public async Task<(Guid userId, string token)> CreateAndLoginUserAsync(
        HttpClient client, string nombre = "TestUser", string? correo = null, string password = "Test123!")
    {
        correo ??= $"{nombre.ToLower()}_{Guid.NewGuid():N}@test.com";

        var createResponse = await client.PostAsJsonAsync("/api/usuarios/crear", new
        {
            nombre,
            correo,
            password
        });
        createResponse.EnsureSuccessStatusCode();

        var loginResponse = await client.PostAsJsonAsync("/api/login", new
        {
            email = correo,
            password
        });
        loginResponse.EnsureSuccessStatusCode();

        var token = await loginResponse.Content.ReadAsStringAsync();
        var trimmedToken = token.Trim('"');

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(trimmedToken);
        var userId = jwtToken.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub" || c.Type == "nameid")?.Value
            ?? throw new InvalidOperationException("No se encontró el Claim de userId en el token JWT.");

        return (Guid.Parse(userId), trimmedToken);
    }

    public async Task<(Guid userId, string token)> CreateAdminAsync(HttpClient client)
    {
        var correo = $"admin_{Guid.NewGuid():N}@test.com";
        return await CreateAndLoginUserAsync(client, "AdminUser", correo, "Admin123!");
    }

    public async Task<HttpClient> CreateAuthenticatedClientAsync(
        string nombre = "TestUser", string rol = "Usuario")
    {
        var client = CreateClient();
        var correo = $"{nombre.ToLower()}_{Guid.NewGuid():N}@test.com";

        var createResponse = await client.PostAsJsonAsync("/api/usuarios/crear", new
        {
            nombre,
            correo,
            password = "Test123!"
        });
        createResponse.EnsureSuccessStatusCode();

        var loginResponse = await client.PostAsJsonAsync("/api/login", new
        {
            email = correo,
            password = "Test123!"
        });
        loginResponse.EnsureSuccessStatusCode();

        var token = await loginResponse.Content.ReadAsStringAsync();
        var trimmedToken = token.Trim('"');

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", trimmedToken);
        return client;
    }

    public async Task<HttpClient> CreateAdminClientAsync()
    {
        var setupClient = CreateClient();
        var (userId, token) = await CreateAndLoginUserAsync(setupClient, "AdminUser");
        return CreateClientWithToken(userId, "AdminUser", $"{userId}@admin.test", "Administrador");
    }

    public async Task<(Guid userId, string correo, string password)> RegisterUserAsync(
        HttpClient client, string nombre = "TestUser")
    {
        var correo = $"{nombre.ToLower()}_{Guid.NewGuid():N}@test.com";
        var password = "Test123!";

        var response = await client.PostAsJsonAsync("/api/usuarios/crear", new
        {
            nombre,
            correo,
            password
        });
        response.EnsureSuccessStatusCode();

        var loginResponse = await client.PostAsJsonAsync("/api/login", new
        {
            email = correo,
            password
        });
        loginResponse.EnsureSuccessStatusCode();

        var token = (await loginResponse.Content.ReadAsStringAsync()).Trim('"');
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var userId = jwtToken.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub" || c.Type == "nameid")?.Value
            ?? throw new InvalidOperationException("No se encontró el Claim de userId en el token JWT.");

        return (Guid.Parse(userId), correo, password);
    }
}

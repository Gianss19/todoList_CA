using System.Text;
using todoList.Domain;
using todoList.Application.Services;
using todoList.Infrastructure;
using todoList.Application.UseCases.Tarea;
using todoList.Infrastructure.Persistence;
using todoList.Infrastructure.Repository;
using todoList.Application.UseCases.Usuario;
using todoList.Infrastructure.Services;
using todoList.Application.UseCases.Login;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using todoList.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.Configure<FileSettings>(builder.Configuration.GetSection("FileSettings"));
builder.Services.AddScoped<ITareasRepository, EfTareaRepository>();
builder.Services.AddScoped<IUsuarioRepository, EfUsuarioRepository>();

builder.Services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<ITokenService, JwtService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration["Jwt:SecretKey"]
            ?? throw new InvalidOperationException("Jwt:SecretKey no configurada."))),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = false,
        ValidateLifetime = true,
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy => policy.RequireRole(nameof(Rol.Administrador)));
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<TodoListDbContext>(options =>
        options.UseInMemoryDatabase("TodoListDev"));
}
else
{
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("No se encontró la cadena de conexión.");

    builder.Services.AddDbContext<TodoListDbContext>(options =>
        options.UseNpgsql(connectionString));
}

builder.Services.AddHttpClient<IHttpCatService, HttpCatService>();

builder.Services.AddHealthChecks();

builder.Services.AddScoped<ObtenerTodasTareasUseCase>();
builder.Services.AddScoped<ObtenerTareaUseCase>();
builder.Services.AddScoped<CrearTareaUseCase>();
builder.Services.AddScoped<CompletarTareaUseCase>();
builder.Services.AddScoped<CambiarNombreTareaUseCase>();
builder.Services.AddScoped<BorrarTareasUseCase>();
builder.Services.AddScoped<ObtenerTodasTareasPorUsuarioUseCase>();

builder.Services.AddScoped<ObtenerTodosUsuariosUseCase>();
builder.Services.AddScoped<CrearUsuarioUseCase>();
builder.Services.AddScoped<CambiarNombreUsuarioUseCase>();
builder.Services.AddScoped<CambiarContraseñaUsuarioUseCase>();
builder.Services.AddScoped<DesactivarUsuarioUseCase>();
builder.Services.AddScoped<ActivarUsuarioUseCase>();
builder.Services.AddScoped<BorrarUsuarioUseCase>();

builder.Services.AddScoped<LoginUseCase>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
            policy.WithOrigins(origins)
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();
    context.Database.EnsureCreated();
    DevSeedData.Seed(context);
}

app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseMiddleware<ExceptionsMiddlewareHandler>();

app.UseHttpsRedirection();



app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
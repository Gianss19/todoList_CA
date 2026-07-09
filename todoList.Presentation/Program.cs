using Microsoft.AspNetCore.Identity;
using todoList.Domain;

using todoList.Application.UseCases;
using todoList.Application.Services;
using todoList.Infrastructure;
using todoList.Application.UseCases.Tarea;
using todoList.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using todoList.Infrastructure.Repository;
using todoList.Application.UseCases.Usuario;
using todoList.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using todoList.Application.UseCases.Login;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();

builder.Services.AddScoped<ITareasRepository, FileTareasRepository>();
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]
                                                    ?? throw new InvalidOperationException("Jwt:Key no configurada."))),
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



var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "No se encontró la cadena de conexión.");

builder.Services.AddDbContext<TodoListDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});
// UseCases para Tareas

builder.Services.AddHttpClient<IHttpCatService, HttpCatService>();

builder.Services.AddScoped<ObtenerTodasTareasUseCase>();

builder.Services.AddScoped<ObtenerTareaUseCase>();

builder.Services.AddScoped<CrearTareaUseCase>();

builder.Services.AddScoped<CompletarTareaUseCase>();

builder.Services.AddScoped<CambiarNombreTareaUseCase>();

builder.Services.AddScoped<BorrarTareasUseCase>();
// UseCases para Tareas adicionales
builder.Services.AddScoped<ObtenerTodasTareasPorUsuarioUseCase>();



// UseCases para Usuarios
builder.Services.AddScoped<ObtenerTodosUsuariosUseCase>();

builder.Services.AddScoped<CrearUsuarioUseCase>();

builder.Services.AddScoped<CambiarNombreUsuarioUseCase>();

builder.Services.AddScoped<CambiarContraseñaUsuarioUseCase>();

builder.Services.AddScoped<DesactivarUsuarioUseCase>();

builder.Services.AddScoped<ActivarUsuarioUseCase>();

builder.Services.AddScoped<BorrarUsuarioUseCase>();

//UseCases para login
builder.Services.AddScoped<LoginUseCase>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


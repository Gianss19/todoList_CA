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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();

builder.Services.AddScoped<ITareasRepository, FileTareasRepository>();
builder.Services.Configure<FileSettings>(builder.Configuration.GetSection("FileSettings"));   

builder.Services.AddScoped<ITareasRepository, EfTareaRepository>();
builder.Services.AddScoped<IUsuarioRepository, EfUsuarioRepository>();


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

builder.Services.AddScoped<ObtenerUsuarioUseCase>();

builder.Services.AddScoped<CrearUsuarioUseCase>();

builder.Services.AddScoped<CambiarNombreUsuarioUseCase>();

builder.Services.AddScoped<CambiarContraseñaUsuarioUseCase>();

builder.Services.AddScoped<DesactivarUsuarioUseCase>();

builder.Services.AddScoped<ActivarUsuarioUseCase>();

builder.Services.AddScoped<BorrarUsuarioUseCase>();

builder.Services.AddScoped<LoginUsuarioUseCase>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


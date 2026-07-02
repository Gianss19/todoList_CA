using Microsoft.AspNetCore.Identity;
using todoList.Domain;

using todoList.Application.UseCases;
using todoList.Application.Services;
using todoList.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();

builder.Services.AddScoped<ITareasRepository, FileTareasRepository>();

builder.Services.AddHttpClient<IHttpCatService, HttpCatService>();

builder.Services.AddScoped<ObtenerTodasTareasUseCase>();

builder.Services.AddScoped<ObtenerTareaUseCase>();

builder.Services.AddScoped<CrearTareaUseCase>();

builder.Services.AddScoped<CompletarTareaUseCase>();

builder.Services.AddScoped<CambiarNombreUseCase>();

builder.Services.AddScoped<BorrarTareasUseCase>();

builder.Services.Configure<FileSettings>(builder.Configuration.GetSection("FileSettings"));   



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

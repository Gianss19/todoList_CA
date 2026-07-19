using System.Net;
using System.Text.Json;

namespace todoList.Api.Middlewares;

public class ExceptionsMiddlewareHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionsMiddlewareHandler> _logger;

    public ExceptionsMiddlewareHandler(RequestDelegate next, ILogger<ExceptionsMiddlewareHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción no controlada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            KeyNotFoundException e => (HttpStatusCode.NotFound, e.Message),
            UnauthorizedAccessException e => (HttpStatusCode.Unauthorized, e.Message),
            ArgumentException e => (HttpStatusCode.BadRequest, e.Message),
            InvalidOperationException e => (HttpStatusCode.Conflict, e.Message),
            _ => (HttpStatusCode.InternalServerError, "Ocurrió un error interno del servidor.")
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new { error = message };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

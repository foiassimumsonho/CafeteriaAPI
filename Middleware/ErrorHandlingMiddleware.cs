// =============================================================
// Middleware/ErrorHandlingMiddleware.cs
// Captura exceções não tratadas e retorna JSON padronizado
// =============================================================
using System.Net;
using System.Text.Json;
using CafeteriaAPI.DTOs;

namespace CafeteriaAPI.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        // Mapeia tipos de exceção para códigos HTTP
        context.Response.StatusCode = ex switch
        {
            InvalidOperationException => (int)HttpStatusCode.BadRequest,          // 400
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,      // 401
            KeyNotFoundException => (int)HttpStatusCode.NotFound,                 // 404
            _ => (int)HttpStatusCode.InternalServerError                          // 500
        };

        var response = ApiResponse<object>.Erro(ex.Message);
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}

using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace TruckApi.Shared.Filters;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken ct
    )
    {
        var (status, message) = exception switch
        {
            BadHttpRequestException { InnerException: JsonException { Message: var msg } jsonEx } ex
                when msg.Contains("could not be mapped") => (
                ex.StatusCode,
                $"Campo desconhecido: '{jsonEx.Path?.TrimStart('$', '.')}'."
            ),

            BadHttpRequestException { InnerException: JsonException jsonEx } ex => (
                ex.StatusCode,
                $"Valor inválido para o campo '{jsonEx.Path?.TrimStart('$', '.')}'."
            ),

            BadHttpRequestException ex => (ex.StatusCode, "Requisição inválida."),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno no servidor."),
        };

        if (status == StatusCodes.Status500InternalServerError)
            logger.LogError(
                exception,
                "Unhandled exception on {Method} {Path}",
                context.Request.Method,
                context.Request.Path
            );

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new { error = message }, ct);
        return true;
    }
}

using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace TruckApi.Shared;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken ct
    )
    {
        var (status, message) = exception switch
        {
            BadHttpRequestException { InnerException: JsonException jsonEx } ex =>
                (ex.StatusCode, $"Campo inválido na requisição: '{jsonEx.Path?.TrimStart('$', '.')}'."),
            BadHttpRequestException ex => (ex.StatusCode, "Requisição inválida."),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno no servidor."),
        };

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(new { error = message }, ct);
        return true;
    }
}

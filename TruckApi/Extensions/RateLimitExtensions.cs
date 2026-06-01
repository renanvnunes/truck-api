using System.Threading.RateLimiting;

namespace TruckApi.Extensions;

public static class RateLimitExtensions
{
    // Nomes das policies — use estas constantes nos endpoints com .RequireRateLimiting(...)
    // Para adicionar uma nova policy: crie a constante aqui e registre-a em AddRateLimitPolicy
    public static class Policy
    {
        public const string Login = "login";
        // public const string PasswordReset = "password-reset";
    }

    public static IServiceCollection AddRateLimitPolicy(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.AddRateLimiter(options =>
        {
            options.OnRejected = async (context, ct) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsJsonAsync(
                    new { error = "Muitas tentativas. Tente novamente em instantes." },
                    ct
                );
            };

            // Aplicado automaticamente em TODOS os endpoints — não precisa declarar nas rotas
            // Controle: appsettings.json > RateLimit > Global
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: GetClientIp(ctx),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = config.GetValue("RateLimit:Global:PermitLimit", 100),
                        Window = TimeSpan.FromSeconds(
                            config.GetValue("RateLimit:Global:WindowSeconds", 60)
                        ),
                        QueueLimit = 0,
                    }
                )
            );

            // Policy.Login — aplicado manualmente via .RequireRateLimiting(Policy.Login)
            // Controle: appsettings.json > RateLimit > Login
            options.AddPolicy(
                Policy.Login,
                ctx =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: GetClientIp(ctx),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = config.GetValue("RateLimit:Login:PermitLimit", 5),
                            Window = TimeSpan.FromSeconds(
                                config.GetValue("RateLimit:Login:WindowSeconds", 60)
                            ),
                            QueueLimit = 0,
                        }
                    )
            );
        });

        return services;
    }

    private static string GetClientIp(HttpContext ctx) =>
        ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TruckApi.Infrastructure.Cache;
using TruckApi.Infrastructure.Database.Entities;
using TruckApi.Shared.Result;

namespace TruckApi.Shared.Auth;

public static class RequireAuthExtension
{
    /// <summary>
    /// Protege a rota com autenticação JWT e validação de sessão no Redis.
    /// Passe os roles permitidos ou deixe vazio para aceitar qualquer usuário autenticado.
    /// </summary>
    public static RouteHandlerBuilder RequireAuth(
        this RouteHandlerBuilder builder,
        params UserRole[] roles
    )
    {
        return builder
            .RequireAuthorization()
            .AddEndpointFilter(
                async (context, next) =>
                {
                    var http = context.HttpContext;

                    var userId = http.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
                    if (userId is null)
                    {
                        return TypedResults.Unauthorized();
                    }

                    var cache = http.RequestServices.GetRequiredService<ICacheService>();
                    var session = await cache.GetAsync<UserSession>($"session:{userId}");

                    if (session is null)
                    {
                        return TypedResults.Unauthorized();
                    }

                    if (roles.Length > 0 && !roles.Any(r => r.ToString() == session.Role))
                    {
                        return TypedResults.Json(
                            new ErrorResponse(
                                "Auth.Forbidden",
                                "Você não tem permissão para acessar este recurso."
                            ),
                            statusCode: StatusCodes.Status403Forbidden
                        );
                    }

                    http.Items["CurrentUser"] = session;

                    return await next(context);
                }
            );
    }
}

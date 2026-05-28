namespace TruckApi.Shared.Auth;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public UserSession Session =>
        httpContextAccessor.HttpContext?.Items["CurrentUser"] as UserSession
        ?? throw new InvalidOperationException("Nenhum usuário autenticado no contexto.");
}

namespace TruckApi.Shared.Auth;

public interface ICurrentUser
{
    UserSession Session { get; }
}

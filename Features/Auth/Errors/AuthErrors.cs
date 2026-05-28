using TruckApi.Shared;

namespace TruckApi.Features.Auth.Errors;

public static class AuthErrors
{
    public static readonly Error InvalidCredentials = new(
        "Auth.InvalidCredentials",
        "Whatsapp ou senha incorretos."
    );

    public static readonly Error UserInactive = new("Auth.UserInactive", "Usuário inativo.");
}

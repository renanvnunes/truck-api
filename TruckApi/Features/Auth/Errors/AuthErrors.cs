namespace TruckApi.Features.Auth.Errors;

public static class AuthErrors
{
    public static readonly Error InvalidCredentials = new(
        "Auth.InvalidCredentials",
        "Whatsapp ou senha incorretos.",
        StatusCodes.Status401Unauthorized
    );

    public static readonly Error UserNotFound = new(
        "Auth.UserNotFound",
        "Usuário não encontrado.",
        StatusCodes.Status404NotFound
    );

    public static readonly Error UserInactive = new(
        "Auth.UserInactive",
        "Usuário inativo.",
        StatusCodes.Status403Forbidden
    );

    public static readonly Error ForgotPasswordCooldown = new(
        "Auth.ForgotPasswordCooldown",
        "Aguarde 30 segundos antes de solicitar um novo código.",
        StatusCodes.Status429TooManyRequests
    );

    public static readonly Error InvalidVerificationCode = new(
        "Auth.InvalidVerificationCode",
        "Código de verificação inválido.",
        StatusCodes.Status400BadRequest
    );

    public static readonly Error OtpCooldown = new(
        "Auth.OtpCooldown",
        "Aguarde 30 segundos antes de solicitar um novo código.",
        StatusCodes.Status429TooManyRequests
    );

    public static readonly Error InvalidRefreshToken = new(
        "Auth.InvalidRefreshToken",
        "Refresh token inválido ou expirado.",
        StatusCodes.Status401Unauthorized
    );
}

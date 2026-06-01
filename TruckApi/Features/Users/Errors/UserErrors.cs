
namespace TruckApi.Features.Users.Errors;

public static class UserErrors
{
    public static readonly Error NotFound = new(
        "User.NotFound",
        "Usuário não encontrado.",
        StatusCodes.Status404NotFound
    );

    public static readonly Error WhatsappAlreadyExists = new(
        "User.WhatsappAlreadyExists",
        "Whatsapp já cadastrado.",
        StatusCodes.Status409Conflict
    );
}

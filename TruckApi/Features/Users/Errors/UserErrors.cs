
namespace TruckApi.Features.Users.Errors;

public static class UserErrors
{
    public static readonly Error NotFound = new("User.NotFound", "Usuário não encontrado.");

    public static readonly Error WhatsappAlreadyExists = new(
        "User.WhatsappAlreadyExists",
        "Whatsapp já cadastrado."
    );
}

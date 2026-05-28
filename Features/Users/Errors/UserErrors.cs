using TruckApi.Shared;

namespace TruckApi.Features.Users.Errors;

public static class UserErrors
{
    public static readonly Error WhatsappAlreadyExists = new(
        "User.WhatsappAlreadyExists",
        "Whatsapp já cadastrado."
    );
}

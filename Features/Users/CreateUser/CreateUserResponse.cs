namespace TruckApi.Features.Users.CreateUser;

public record CreateUserResponse(
    string Id,
    string FullName,
    string Whatsapp,
    string Role,
    bool IsActive,
    DateTimeOffset CreatedAt
);

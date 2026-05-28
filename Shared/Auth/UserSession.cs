namespace TruckApi.Shared.Auth;

public record UserSession(
    string Id,
    string FullName,
    string Whatsapp,
    string Role,
    string? CompanyId,
    bool IsActive
);

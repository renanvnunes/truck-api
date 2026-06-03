using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Shared.Auth;

public static class UserSessionExtensions
{
    public static bool IsAdmin(this UserSession session)
    {
        return session.Role == UserRole.Admin.ToString();
    }

    public static bool CanAccessCompany(this UserSession session, string? companyId)
    {
        return session.IsAdmin() || session.CompanyId == companyId;
    }
}

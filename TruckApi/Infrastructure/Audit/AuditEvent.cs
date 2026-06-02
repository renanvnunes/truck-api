namespace TruckApi.Infrastructure.Audit;

public enum AuditEvent
{
    UserPasswordChanged,
    UserCreated,
    UserUpdated,
    UserDeleted,
    UserLogin,
    UserLoginOtp,
}

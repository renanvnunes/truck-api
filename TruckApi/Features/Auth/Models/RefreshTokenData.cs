namespace TruckApi.Features.Auth.Models;

public record RefreshTokenData(
    string UserId,
    string Ip,
    string UserAgent,
    string Origin,
    string DeviceName,
    DateTimeOffset CreatedAt
);

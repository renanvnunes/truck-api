namespace TruckApi.Features.Company.Dtos.GetCompanyById;

public record GetCompanyByIdResponse(
    string Id,
    string Name,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);

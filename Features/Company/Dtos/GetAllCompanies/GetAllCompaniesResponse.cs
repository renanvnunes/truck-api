namespace TruckApi.Features.Company.Dtos.GetAllCompanies;

public record GetAllCompaniesResponse(GetAllCompaniesItem[] Data, string? NextCursor);

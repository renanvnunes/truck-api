using Carter;
using TruckApi.Features.Company.Dtos.GetCompanyById;
using TruckApi.Features.Company.UseCases;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Company.Endpoints;

public class CompanyGetById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiVersions.V1}/companies")
            .WithTags("Companies")
            .MapGet(
                "/{id}",
                (GetCompanyByIdUseCase useCase, string id) =>
                    useCase.ExecuteAsync(id).ToHttpResultAsync(company => new GetCompanyByIdResponse(
                        company.Id,
                        company.Name,
                        company.CreatedAt,
                        company.UpdatedAt
                    ))
            )
            .WithSummary("Obter empresa por ID")
            .WithDescription("Retorna os dados de uma empresa específica com base em seu ID.")
            .RequireAuth(UserRole.Admin);
    }
}

using Carter;
using TruckApi.Features.Company.Dtos.UpdateCompany;
using TruckApi.Features.Company.UseCases;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Company.Endpoints;

public class CompanyUpdate : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiVersions.V1}/companies")
            .WithTags("Companies")
            .MapPatch(
                "/{id}",
                (string id, UpdateCompanyRequest request, UpdateCompanyUseCase useCase) =>
                    useCase
                        .ExecuteAsync(id, request)
                        .ToHttpResultAsync(company => new UpdateCompanyResponse(
                            company.Id,
                            company.Name,
                            company.UpdatedAt
                        ))
            )
            .WithSummary("Atualizar empresa")
            .WithDescription(
                "Atualiza parcialmente os dados de uma empresa. Apenas os campos enviados serão alterados."
            )
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<UpdateCompanyRequest>>()
            .RequireAuth(UserRole.Admin);
    }
}

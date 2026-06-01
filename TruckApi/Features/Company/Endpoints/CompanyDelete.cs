using Carter;
using TruckApi.Features.Company.UseCases;
using TruckApi.Infrastructure.Database.Entities;
using CompanyEntity = TruckApi.Infrastructure.Database.Entities.Company;

namespace TruckApi.Features.Company.Endpoints;

public class CompanyDelete : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/companies")
            .WithTags("Companies")
            .MapDelete(
                "/{id}",
                (string id, DeleteCompanyUseCase useCase) =>
                    useCase.ExecuteAsync(id).ToNoContentAsync()
            )
            .WithSummary("Remover empresa")
            .WithDescription("Remove permanentemente uma empresa do sistema.")
            .RequireAuth(UserRole.Admin);
    }
}

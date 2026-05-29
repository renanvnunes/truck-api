using System.Diagnostics;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using TruckApi.Features.Company.UseCases;
using TruckApi.Infrastructure.Database.Entities;
using CompanyEntity = TruckApi.Infrastructure.Database.Entities.Company;

namespace TruckApi.Features.Company.Controllers;

public class CompanyDelete : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/companies")
            .WithTags("Companies")
            .MapDelete(
                "/{id}",
                async Task<Results<NoContent, NotFound<ErrorResponse>>> (
                    string id,
                    DeleteCompanyUseCase useCase
                ) =>
                {
                    return await useCase.ExecuteAsync(id) switch
                    {
                        Result<CompanyEntity>.Ok => TypedResults.NoContent(),
                        Result<CompanyEntity>.Fail { Error: var error } => TypedResults.NotFound(
                            new ErrorResponse(error.Code, error.Message)
                        ),
                        _ => throw new UnreachableException(),
                    };
                }
            )
            .WithSummary("Remover empresa")
            .WithDescription("Remove permanentemente uma empresa do sistema.")
            .RequireAuth(UserRole.Admin);
    }
}

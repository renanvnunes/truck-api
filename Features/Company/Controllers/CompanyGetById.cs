using System.Diagnostics;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using TruckApi.Features.Company.Dtos.GetCompanyById;
using TruckApi.Features.Company.UseCases;
using TruckApi.Infrastructure.Database.Entities;
using CompanyEntity = TruckApi.Infrastructure.Database.Entities.Company;

namespace TruckApi.Features.Company.Controllers;

public class CompanyGetById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/companies")
            .WithTags("Companies")
            .MapGet(
                "/{id}",
                async Task<Results<Ok<GetCompanyByIdResponse>, NotFound<ErrorResponse>>> (
                    GetCompanyByIdUseCase useCase,
                    string id
                ) =>
                {
                    return await useCase.ExecuteAsync(id) switch
                    {
                        Result<CompanyEntity>.Ok { Value: var company } => TypedResults.Ok(
                            new GetCompanyByIdResponse(
                                company.Id,
                                company.Name,
                                company.CreatedAt,
                                company.UpdatedAt
                            )
                        ),
                        Result<CompanyEntity>.Fail { Error: var error } => TypedResults.NotFound(
                            new ErrorResponse(error.Code, error.Message)
                        ),
                        _ => throw new UnreachableException(),
                    };
                }
            )
            .WithSummary("Obter empresa por ID")
            .WithDescription("Retorna os dados de uma empresa específica com base em seu ID.")
            .RequireAuth(UserRole.Admin);
    }
}

using System.Diagnostics;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using TruckApi.Features.Company.Dtos.UpdateCompany;
using TruckApi.Features.Company.Errors;
using TruckApi.Features.Company.UseCases;
using TruckApi.Infrastructure.Database.Entities;
using CompanyEntity = TruckApi.Infrastructure.Database.Entities.Company;

namespace TruckApi.Features.Company.Endpoints;

public class CompanyUpdate : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/companies")
            .WithTags("Companies")
            .MapPatch(
                "/{id}",
                async Task<
                    Results<
                        Ok<UpdateCompanyResponse>,
                        NotFound<ErrorResponse>,
                        Conflict<ErrorResponse>
                    >
                > (string id, UpdateCompanyRequest request, UpdateCompanyUseCase useCase) =>
                {
                    return await useCase.ExecuteAsync(id, request) switch
                    {
                        Result<CompanyEntity>.Ok { Value: var company } => TypedResults.Ok(
                            new UpdateCompanyResponse(company.Id, company.Name, company.UpdatedAt)
                        ),
                        Result<CompanyEntity>.Fail { Error: var error }
                            when error == CompanyErrors.NotFound =>
                            TypedResults.NotFound(new ErrorResponse(error.Code, error.Message)),
                        Result<CompanyEntity>.Fail { Error: var error } => TypedResults.Conflict(
                            new ErrorResponse(error.Code, error.Message)
                        ),
                        _ => throw new UnreachableException(),
                    };
                }
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

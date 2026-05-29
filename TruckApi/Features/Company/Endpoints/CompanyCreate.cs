using System.Diagnostics;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using TruckApi.Features.Company.Dtos.CreateCompany;
using TruckApi.Features.Company.Errors;
using TruckApi.Features.Company.UseCases;
using TruckApi.Infrastructure.Database.Entities;
using CompanyEntity = TruckApi.Infrastructure.Database.Entities.Company;

namespace TruckApi.Features.Company.Endpoints;

public class CompanyCreate : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/companies")
            .WithTags("Companies")
            .MapPost(
                "/",
                async Task<Results<Created<CreateCompanyResponse>, Conflict<ErrorResponse>>> (
                    CreateCompanyRequest request,
                    CreateCompanyUseCase useCase
                ) =>
                {
                    return await useCase.ExecuteAsync(request) switch
                    {
                        Result<CompanyEntity>.Ok { Value: var company } => TypedResults.Created(
                            $"/companies/{company.Id}",
                            new CreateCompanyResponse(company.Id, company.Name, company.CreatedAt)
                        ),
                        Result<CompanyEntity>.Fail { Error: var error } => TypedResults.Conflict(
                            new ErrorResponse(error.Code, error.Message)
                        ),
                        _ => throw new UnreachableException(),
                    };
                }
            )
            .WithSummary("Criar empresa")
            .WithDescription("Cria uma nova empresa no sistema.")
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<CreateCompanyRequest>>()
            .RequireAuth(UserRole.Admin);
    }
}

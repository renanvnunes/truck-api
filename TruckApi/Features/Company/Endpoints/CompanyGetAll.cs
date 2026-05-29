using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using TruckApi.Features.Company.Dtos.GetAllCompanies;
using TruckApi.Features.Company.UseCases;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Company.Endpoints;

public class CompanyGetAll : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/companies")
            .WithTags("Companies")
            .MapGet(
                "/",
                async Task<Ok<GetAllCompaniesResponse>> (
                    GetAllCompaniesUseCase useCase,
                    string? cursor = null,
                    int limit = 20
                ) =>
                {
                    var (companies, nextCursor) = await useCase.ExecuteAsync(cursor, limit);

                    var response = new GetAllCompaniesResponse(
                        companies
                            .Select(c => new GetAllCompaniesItem(c.Id, c.Name, c.CreatedAt))
                            .ToArray(),
                        nextCursor
                    );

                    return TypedResults.Ok(response);
                }
            )
            .WithSummary("Listar empresas")
            .WithDescription(
                "Retorna empresas paginadas com cursor. Use o campo `nextCursor` da resposta como parâmetro `cursor` na próxima requisição."
            )
            .RequireAuth(UserRole.Admin);
    }
}

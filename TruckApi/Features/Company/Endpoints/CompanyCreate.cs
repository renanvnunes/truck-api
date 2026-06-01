using Carter;
using TruckApi.Features.Company.Dtos.CreateCompany;
using TruckApi.Features.Company.UseCases;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Company.Endpoints;

public class CompanyCreate : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/companies")
            .WithTags("Companies")
            .MapPost(
                "/",
                (CreateCompanyRequest request, CreateCompanyUseCase useCase) =>
                    useCase.ExecuteAsync(request).ToCreatedAsync(company => (
                        $"/companies/{company.Id}",
                        new CreateCompanyResponse(company.Id, company.Name, company.CreatedAt)
                    ))
            )
            .WithSummary("Criar empresa")
            .WithDescription("Cria uma nova empresa no sistema.")
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<CreateCompanyRequest>>()
            .RequireAuth(UserRole.Admin);
    }
}

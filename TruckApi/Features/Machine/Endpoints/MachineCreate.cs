using Carter;
using TruckApi.Features.Machine.Dtos.CreateMachine;
using TruckApi.Features.Machine.UseCases;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Machine.Endpoints;

public class MachineCreate : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiVersions.V1}/machines")
            .WithTags("Machines")
            .MapPost(
                "/",
                (CreateMachineRequest request, CreateMachineUseCase useCase) =>
                    useCase.ExecuteAsync(request).ToCreatedAsync(m => (
                        $"{ApiVersions.V1}/machines/{m.Id}",
                        new CreateMachineResponse(m.Id, m.Code, m.Type, m.Brand, m.Model, m.Year, m.SerialNumber, m.Plate, m.CurrentHourmeter, m.Status, m.CompanyId, m.CreatedAt)
                    ))
            )
            .WithSummary("Cadastrar máquina")
            .WithDescription("Cadastra uma nova máquina no sistema.")
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<CreateMachineRequest>>()
            .RequireAuth(UserRole.Admin, UserRole.CompanyManager);
    }
}

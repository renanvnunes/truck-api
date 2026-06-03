using Carter;
using TruckApi.Features.Machine.Dtos.UpdateMachine;
using TruckApi.Features.Machine.UseCases;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Machine.Endpoints;

public class MachineUpdate : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/machines")
            .WithTags("Machines")
            .MapPatch(
                "/{id}",
                (string id, UpdateMachineRequest request, UpdateMachineUseCase useCase) =>
                    useCase.ExecuteAsync(id, request).ToHttpResultAsync(m =>
                        new UpdateMachineResponse(m.Id, m.Code, m.Type, m.Brand, m.Model, m.Year,
                            m.SerialNumber, m.Plate, m.CurrentHourmeter, m.Status,
                            m.CompanyId, m.UpdatedAt)
                    )
            )
            .WithSummary("Atualizar máquina")
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<UpdateMachineRequest>>()
            .RequireAuth(UserRole.Admin, UserRole.CompanyManager);
    }
}

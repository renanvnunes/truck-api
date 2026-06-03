using Carter;
using TruckApi.Features.Machine.Dtos.UpdateMachine;
using TruckApi.Features.Machine.Dtos.UpdateMachineStatus;
using TruckApi.Features.Machine.UseCases;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Machine.Endpoints;

public class MachineUpdateStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/machines")
            .WithTags("Machines")
            .MapPatch(
                "/{id}/status",
                (string id, UpdateMachineStatusRequest request, UpdateMachineStatusUseCase useCase) =>
                    useCase.ExecuteAsync(id, request).ToHttpResultAsync(m =>
                        new UpdateMachineResponse(m.Id, m.Code, m.Type, m.Brand, m.Model, m.Year,
                            m.SerialNumber, m.Plate, m.CurrentHourmeter, m.Status,
                            m.CompanyId, m.UpdatedAt)
                    )
            )
            .WithSummary("Atualizar status da máquina")
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<UpdateMachineStatusRequest>>()
            .RequireAuth(UserRole.Admin, UserRole.CompanyManager, UserRole.CompanySupervisor);
    }
}

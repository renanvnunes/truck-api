using Carter;
using TruckApi.Features.Machine.Dtos.UpdateMachine;
using TruckApi.Features.Machine.Dtos.UpdateMachineHourmeter;
using TruckApi.Features.Machine.UseCases;

namespace TruckApi.Features.Machine.Endpoints;

public class MachineUpdateHourmeter : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/machines")
            .WithTags("Machines")
            .MapPatch(
                "/{id}/hourmeter",
                (string id, UpdateMachineHourmeterRequest request, UpdateMachineHourmeterUseCase useCase) =>
                    useCase.ExecuteAsync(id, request).ToHttpResultAsync(m =>
                        new UpdateMachineResponse(m.Id, m.Code, m.Type, m.Brand, m.Model, m.Year,
                            m.SerialNumber, m.Plate, m.CurrentHourmeter, m.Status,
                            m.CompanyId, m.UpdatedAt)
                    )
            )
            .WithSummary("Atualizar horímetro da máquina")
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<UpdateMachineHourmeterRequest>>()
            .RequireAuth();
    }
}

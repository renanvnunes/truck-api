using Carter;
using TruckApi.Features.Machine.Dtos.GetMachineById;
using TruckApi.Features.Machine.UseCases;

namespace TruckApi.Features.Machine.Endpoints;

public class MachineGetById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/machines")
            .WithTags("Machines")
            .MapGet(
                "/{id}",
                (string id, GetMachineByIdUseCase useCase) =>
                    useCase.ExecuteAsync(id).ToHttpResultAsync(m =>
                        new GetMachineByIdResponse(m.Id, m.Code, m.Type, m.Brand, m.Model, m.Year,
                            m.SerialNumber, m.Plate, m.CurrentHourmeter, m.Status,
                            m.CompanyId, m.CreatedAt, m.UpdatedAt)
                    )
            )
            .WithSummary("Obter máquina por ID")
            .RequireAuth();
    }
}

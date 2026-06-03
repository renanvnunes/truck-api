using Carter;
using TruckApi.Features.Machine.UseCases;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Machine.Endpoints;

public class MachineDelete : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiVersions.V1}/machines")
            .WithTags("Machines")
            .MapDelete(
                "/{id}",
                (string id, DeleteMachineUseCase useCase) =>
                    useCase.ExecuteAsync(id).ToHttpResultAsync()
            )
            .WithSummary("Remover máquina")
            .RequireAuth(UserRole.Admin);
    }
}

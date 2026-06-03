using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using TruckApi.Features.Machine.Dtos.GetAllMachines;
using TruckApi.Features.Machine.UseCases;

namespace TruckApi.Features.Machine.Endpoints;

public class MachineGetAll : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiVersions.V1}/machines")
            .WithTags("Machines")
            .MapGet(
                "/",
                async Task<Ok<GetAllMachinesResponse>> (
                    GetAllMachinesUseCase useCase,
                    string? cursor = null,
                    int limit = 20
                ) =>
                {
                    var (machines, nextCursor) = await useCase.ExecuteAsync(cursor, limit);

                    var response = new GetAllMachinesResponse(
                        machines.Select(m => new GetAllMachinesItem(
                            m.Id, m.Code, m.Type, m.Brand, m.Model, m.Year,
                            m.SerialNumber, m.Plate, m.CurrentHourmeter, m.Status,
                            m.CompanyId, m.CreatedAt
                        )).ToArray(),
                        nextCursor
                    );

                    return TypedResults.Ok(response);
                }
            )
            .WithSummary("Listar máquinas")
            .WithDescription("Retorna máquinas paginadas com cursor. Admin retorna todas, demais roles retornam apenas as da própria empresa.")
            .RequireAuth();
    }
}

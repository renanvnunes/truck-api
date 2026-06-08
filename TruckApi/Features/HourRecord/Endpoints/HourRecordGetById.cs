using Carter;
using TruckApi.Features.HourRecord.Dtos.GetHourRecordById;
using TruckApi.Features.HourRecord.UseCases;

namespace TruckApi.Features.HourRecord.Endpoints;

public class HourRecordGetById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiVersions.V1}/hour-records")
            .WithTags("HourRecords")
            .MapGet(
                "/{id}",
                (string id, GetHourRecordByIdUseCase useCase) =>
                    useCase.ExecuteAsync(id).ToHttpResultAsync(h =>
                        new GetHourRecordByIdResponse(
                            h.Id,
                            h.MachineId,
                            h.Machine.Code,
                            h.OperatorId,
                            h.Operator.FullName,
                            h.Date,
                            h.StartedAt,
                            h.EndedAt,
                            h.TotalHours,
                            h.HourmeterStart,
                            h.HourmeterEnd,
                            h.Notes,
                            h.CreatedAt,
                            h.UpdatedAt
                        )
                    )
            )
            .WithSummary("Buscar registro de horas por ID")
            .RequireAuth();
    }
}

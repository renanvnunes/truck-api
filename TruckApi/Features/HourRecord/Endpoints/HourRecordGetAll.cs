using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using TruckApi.Features.HourRecord.Dtos.GetAllHourRecords;
using TruckApi.Features.HourRecord.UseCases;

namespace TruckApi.Features.HourRecord.Endpoints;

public class HourRecordGetAll : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiVersions.V1}/hour-records")
            .WithTags("HourRecords")
            .MapGet(
                "/",
                async Task<Ok<GetAllHourRecordsResponse>> (
                    GetAllHourRecordsUseCase useCase,
                    string? cursor = null,
                    int limit = 20,
                    string? machineId = null,
                    string? operatorId = null,
                    DateOnly? date = null
                ) =>
                {
                    var (records, nextCursor) = await useCase.ExecuteAsync(cursor, limit, machineId, operatorId, date);

                    var response = new GetAllHourRecordsResponse(
                        records
                            .Select(h => new GetAllHourRecordsItem(
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
                                h.CreatedAt
                            ))
                            .ToArray(),
                        nextCursor
                    );

                    return TypedResults.Ok(response);
                }
            )
            .WithSummary("Listar registros de horas")
            .WithDescription(
                "Retorna registros de horas paginados com cursor. Suporta filtros por máquina, operador e data."
            )
            .RequireAuth();
    }
}

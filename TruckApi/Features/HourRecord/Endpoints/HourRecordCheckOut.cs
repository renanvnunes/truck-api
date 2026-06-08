using Carter;
using TruckApi.Features.HourRecord.Dtos.CheckOut;
using TruckApi.Features.HourRecord.UseCases;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.HourRecord.Endpoints;

public class HourRecordCheckOut : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiVersions.V1}/hour-records")
            .WithTags("HourRecords")
            .MapPatch(
                "/{id}/checkout",
                (string id, CheckOutRequest request, CheckOutUseCase useCase) =>
                    useCase
                        .ExecuteAsync(id, request)
                        .ToHttpResultAsync(h => new CheckOutResponse(
                            h.Id,
                            h.MachineId,
                            h.OperatorId,
                            h.Date,
                            h.StartedAt,
                            h.EndedAt!.Value,
                            h.TotalHours!.Value,
                            h.HourmeterStart,
                            h.HourmeterEnd,
                            h.Notes,
                            h.UpdatedAt
                        ))
            )
            .WithSummary("Registrar saída do operador")
            .WithDescription(
                "Fecha um registro de horas em aberto, calculando o total de horas trabalhadas."
            )
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<CheckOutRequest>>()
            .RequireAuth(
                UserRole.CompanyOperator,
                UserRole.CompanySupervisor,
                UserRole.CompanyManager,
                UserRole.Admin
            );
    }
}

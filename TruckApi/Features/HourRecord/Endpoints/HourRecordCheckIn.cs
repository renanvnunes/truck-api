using Carter;
using TruckApi.Features.HourRecord.Dtos.CheckIn;
using TruckApi.Features.HourRecord.UseCases;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.HourRecord.Endpoints;

public class HourRecordCheckIn : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup($"{ApiVersions.V1}/hour-records")
            .WithTags("HourRecords")
            .MapPost(
                "/",
                (CheckInRequest request, CheckInUseCase useCase) =>
                    useCase
                        .ExecuteAsync(request)
                        .ToCreatedAsync(h =>
                            (
                                $"{ApiVersions.V1}/hour-records/{h.Id}",
                                new CheckInResponse(
                                    h.Id,
                                    h.MachineId,
                                    h.Machine.Code,
                                    h.OperatorId,
                                    h.Operator.FullName,
                                    h.Date,
                                    h.StartedAt,
                                    h.HourmeterStart,
                                    h.Notes,
                                    h.CreatedAt
                                )
                            )
                        )
            )
            .WithSummary("Registrar entrada do operador")
            .WithDescription(
                "Inicia um registro de horas para o operador autenticado em uma máquina."
            )
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<CheckInRequest>>()
            .RequireAuth(UserRole.CompanyOperator);
    }
}

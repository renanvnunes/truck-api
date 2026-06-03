using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Machine.Dtos.UpdateMachineStatus;

public record UpdateMachineStatusRequest(MachineStatus Status);

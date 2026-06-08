namespace TruckApi.Features.HourRecord.Errors;

public static class HourRecordErrors
{
    public static readonly Error NotFound = new(
        "HourRecord.NotFound",
        "Registro de hora não encontrado.",
        StatusCodes.Status404NotFound
    );

    public static readonly Error AlreadyCheckedIn = new(
        "HourRecord.AlreadyCheckedIn",
        "Operador já possui uma entrada em aberto para esta máquina.",
        StatusCodes.Status409Conflict
    );

    public static readonly Error AlreadyCheckedOut = new(
        "HourRecord.AlreadyCheckedOut",
        "Este registro já possui saída registrada.",
        StatusCodes.Status409Conflict
    );

    public static readonly Error ForbiddenCompany = new(
        "HourRecord.ForbiddenCompany",
        "Acesso negado a esta empresa, verifique se você tem permissão para acessar.",
        StatusCodes.Status403Forbidden
    );

    public static readonly Error Forbidden = new(
        "HourRecord.Forbidden",
        "Acesso negado a este recurso.",
        StatusCodes.Status403Forbidden
    );

    public static readonly Error MachineNotFound = new(
        "HourRecord.MachineNotFound",
        "Máquina não encontrada.",
        StatusCodes.Status404NotFound
    );
}

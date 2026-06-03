namespace TruckApi.Features.Machine.Errors;

public static class MachineErrors
{
    public static readonly Error NotFound = new(
        "Machine.NotFound",
        "Máquina não encontrada.",
        StatusCodes.Status404NotFound
    );

    public static readonly Error Forbidden = new(
        "Machine.Forbidden",
        "Você não tem permissão para acessar esta máquina.",
        StatusCodes.Status403Forbidden
    );

    public static readonly Error SerialNumberAlreadyExists = new(
        "Machine.SerialNumberAlreadyExists",
        "Já existe uma máquina com este número de série.",
        StatusCodes.Status409Conflict
    );

    public static readonly Error CodeAlreadyExistsInCompany = new(
        "Machine.CodeAlreadyExistsInCompany",
        "Já existe uma máquina com este código nesta empresa.",
        StatusCodes.Status409Conflict
    );

    public static readonly Error CompanyIdRequired = new(
        "Machine.CompanyIdRequired",
        "O ID da empresa é obrigatório.",
        StatusCodes.Status400BadRequest
    );
}

namespace TruckApi.Features.Company.Errors;

public static class CompanyErrors
{
    public static readonly Error NotFound = new("Company.NotFound", "Empresa não encontrada.");

    public static readonly Error NameAlreadyExists = new(
        "Company.NameAlreadyExists",
        "Já existe uma empresa com esse nome."
    );
}

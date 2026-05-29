using FluentValidation.TestHelper;
using TruckApi.Features.Company.Dtos.UpdateCompany;
using Xunit;

namespace TruckApi.Tests.Features.Company.Dtos.UpdateCompany;

public class UpdateCompanyRequestValidatorTests
{
    private readonly UpdateCompanyRequestValidator _validator = new();

    [Theory]
    [InlineData(null)] // Nulo é válido (por causa do .When)
    [InlineData("Empresa Truck")] // Nome normal é válido
    [InlineData("A")] // Limite mínimo (1 caractere) é válido
    public void Should_Pass_Validation_For_Valid_Name_Inputs(string? validName)
    {
        var model = new UpdateCompanyRequest { Name = validName };
        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [InlineData("")] // Vazio é inválido
    [InlineData("   ")] // Espaços em branco são inválidos
    public void Should_Fail_Validation_For_Invalid_Name_Inputs(string? invalidName)
    {
        var model = new UpdateCompanyRequest { Name = invalidName };
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Fail_When_Name_Exceeds_100_Characters()
    {
        // Esse fica separado só porque string longa não cabe bem no [InlineData]
        var model = new UpdateCompanyRequest { Name = new string('A', 101) };
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}

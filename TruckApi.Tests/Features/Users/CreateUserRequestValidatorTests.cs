using FluentAssertions;
using FluentValidation.TestHelper;
using TruckApi.Features.Users.Dtos.CreateUser;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Tests.Features.Users;

public class CreateUserRequestValidatorTests
{
    private readonly CreateUserRequestValidator _validator = new();

    private static CreateUserRequest ValidAdmin() =>
        new(FullName: "João Silva", Whatsapp: "5511999999999", Role: UserRole.Admin);

    private static CreateUserRequest ValidOperator() =>
        new(
            FullName: "Carlos Souza",
            Whatsapp: "5511888888888",
            Role: UserRole.CompanyOperator,
            CompanyId: "company_123"
        );

    // --- FullName ---

    [Fact]
    public void FullName_WhenEmpty_ShouldFail()
    {
        var result = _validator.TestValidate(ValidAdmin() with { FullName = "" });
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void FullName_WhenExceeds100Chars_ShouldFail()
    {
        var result = _validator.TestValidate(ValidAdmin() with { FullName = new string('a', 101) });
        result.ShouldHaveValidationErrorFor(x => x.FullName);
    }

    [Fact]
    public void FullName_WhenValid_ShouldPass()
    {
        var result = _validator.TestValidate(ValidAdmin());
        result.ShouldNotHaveValidationErrorFor(x => x.FullName);
    }

    // --- Whatsapp ---

    [Fact]
    public void Whatsapp_WhenEmpty_ShouldFail()
    {
        var result = _validator.TestValidate(ValidAdmin() with { Whatsapp = "" });
        result.ShouldHaveValidationErrorFor(x => x.Whatsapp);
    }

    [Theory]
    [InlineData("551199999999")] // 12 dígitos
    [InlineData("55119999999999")] // 14 dígitos
    public void Whatsapp_WhenNot13Digits_ShouldFail(string whatsapp)
    {
        var result = _validator.TestValidate(ValidAdmin() with { Whatsapp = whatsapp });
        result.ShouldHaveValidationErrorFor(x => x.Whatsapp);
    }

    [Fact]
    public void Whatsapp_When13Digits_ShouldPass()
    {
        var result = _validator.TestValidate(ValidAdmin());
        result.ShouldNotHaveValidationErrorFor(x => x.Whatsapp);
    }

    // --- Password ---

    [Fact]
    public void Password_WhenNull_ShouldPass()
    {
        var result = _validator.TestValidate(ValidAdmin() with { Password = null });
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("abc")] // muito curto
    [InlineData("aaaaabbbbbcccccdddddeeeeeffffff1")] // 31 chars
    public void Password_WhenOutOfRange_ShouldFail(string password)
    {
        var result = _validator.TestValidate(ValidAdmin() with { Password = password });
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Password_WhenValid_ShouldPass()
    {
        var result = _validator.TestValidate(ValidAdmin() with { Password = "senha123" });
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    // --- Age ---

    [Theory]
    [InlineData(17)]
    [InlineData(91)]
    public void Age_WhenOutOfRange_ShouldFail(int age)
    {
        var result = _validator.TestValidate(ValidAdmin() with { Age = age });
        result.ShouldHaveValidationErrorFor(x => x.Age);
    }

    [Fact]
    public void Age_WhenNull_ShouldPass()
    {
        var result = _validator.TestValidate(ValidAdmin() with { Age = null });
        result.ShouldNotHaveValidationErrorFor(x => x.Age);
    }

    [Fact]
    public void Age_WhenValid_ShouldPass()
    {
        var result = _validator.TestValidate(ValidAdmin() with { Age = 30 });
        result.ShouldNotHaveValidationErrorFor(x => x.Age);
    }

    // --- CompanyId (regra de negócio por Role) ---

    [Fact]
    public void CompanyId_WhenAdminWithCompanyId_ShouldFail()
    {
        var request = ValidAdmin() with { CompanyId = "company_123" };
        var result = _validator.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(x => x.CompanyId)
            .WithErrorMessage("Admin não pode pertencer a uma empresa.");
    }

    [Fact]
    public void CompanyId_WhenAdminWithoutCompanyId_ShouldPass()
    {
        var result = _validator.TestValidate(ValidAdmin());
        result.ShouldNotHaveValidationErrorFor(x => x.CompanyId);
    }

    [Theory]
    [InlineData(UserRole.CompanyManager)]
    [InlineData(UserRole.CompanySupervisor)]
    [InlineData(UserRole.CompanyOperator)]
    public void CompanyId_WhenNonAdminWithoutCompanyId_ShouldFail(UserRole role)
    {
        var request = ValidOperator() with { Role = role, CompanyId = null };
        var result = _validator.TestValidate(request);

        result
            .ShouldHaveValidationErrorFor(x => x.CompanyId)
            .WithErrorMessage("CompanyId é obrigatório para este perfil.");
    }

    [Theory]
    [InlineData(UserRole.CompanyManager)]
    [InlineData(UserRole.CompanySupervisor)]
    [InlineData(UserRole.CompanyOperator)]
    public void CompanyId_WhenNonAdminWithCompanyId_ShouldPass(UserRole role)
    {
        var request = ValidOperator() with { Role = role };
        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.CompanyId);
    }
}

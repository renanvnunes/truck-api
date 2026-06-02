using FluentAssertions;
using FluentValidation.TestHelper;
using TruckApi.Features.Auth.Dtos.ForgotPassword;

namespace TruckApi.Tests.Features.Auth;

public class ForgotPasswordRequestValidatorTests
{
    private readonly ForgotPasswordRequestValidator _validator = new();

    private static ForgotPasswordRequest Valid() => new("5511999999999");

    // --- Whatsapp ---

    [Fact]
    public void Whatsapp_WhenEmpty_ShouldFail()
    {
        var result = _validator.TestValidate(Valid() with { Whatsapp = "" });
        result.ShouldHaveValidationErrorFor(x => x.Whatsapp);
    }

    [Theory]
    [InlineData("551199999999")]   // 12 dígitos
    [InlineData("55119999999999")] // 14 dígitos
    public void Whatsapp_WhenNot13Digits_ShouldFail(string whatsapp)
    {
        var result = _validator.TestValidate(Valid() with { Whatsapp = whatsapp });
        result
            .ShouldHaveValidationErrorFor(x => x.Whatsapp)
            .WithErrorMessage("Whatsapp deve conter exatamente 13 dígitos. Exemplo: 5511999999999");
    }

    [Fact]
    public void Whatsapp_When13Digits_ShouldPass()
    {
        var result = _validator.TestValidate(Valid());
        result.ShouldNotHaveValidationErrorFor(x => x.Whatsapp);
    }

    // --- Request completo ---

    [Fact]
    public void ValidRequest_ShouldPassWithNoErrors()
    {
        var result = _validator.TestValidate(Valid());
        result.IsValid.Should().BeTrue();
    }
}

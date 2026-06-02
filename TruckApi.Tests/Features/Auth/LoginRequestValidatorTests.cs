using FluentAssertions;
using FluentValidation.TestHelper;
using TruckApi.Features.Auth.Dtos.Login;

namespace TruckApi.Tests.Features.Auth;

public class LoginRequestValidatorTests
{
    private readonly LoginRequestValidator _validator = new();

    private static LoginRequest Valid() => new("5511999999999", "senha123");

    // --- Whatsapp ---

    [Fact]
    public void Whatsapp_WhenEmpty_ShouldFail()
    {
        var result = _validator.TestValidate(Valid() with { Whatsapp = "" });
        result.ShouldHaveValidationErrorFor(x => x.Whatsapp);
    }

    [Theory]
    [InlineData("551199999999")] // 12 dígitos
    [InlineData("55119999999999")] // 14 dígitos
    public void Whatsapp_WhenNot13Digits_ShouldFail(string whatsapp)
    {
        var result = _validator.TestValidate(Valid() with { Whatsapp = whatsapp });
        result.ShouldHaveValidationErrorFor(x => x.Whatsapp);
    }

    [Fact]
    public void Whatsapp_When13Digits_ShouldPass()
    {
        var result = _validator.TestValidate(Valid());
        result.ShouldNotHaveValidationErrorFor(x => x.Whatsapp);
    }

    // --- Password ---

    [Fact]
    public void Password_WhenEmpty_ShouldFail()
    {
        var result = _validator.TestValidate(Valid() with { Password = "" });
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("abc")] // 3 chars — abaixo do mínimo
    [InlineData("aaaaabbbbbcccccdddddeeeeeffffff1")] // 31 chars — acima do máximo
    public void Password_WhenOutOfRange_ShouldFail(string password)
    {
        var result = _validator.TestValidate(Valid() with { Password = password });
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("abc123")] // 6 chars — mínimo
    [InlineData("aaaaabbbbbcccccdddddeeeeefffff")] // 30 chars — máximo
    public void Password_WhenAtBoundary_ShouldPass(string password)
    {
        var result = _validator.TestValidate(Valid() with { Password = password });
        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    // --- Request completo ---

    [Fact]
    public void ValidRequest_ShouldPassWithNoErrors()
    {
        var result = _validator.TestValidate(Valid());
        result.IsValid.Should().BeTrue();
    }
}

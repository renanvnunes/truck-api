using TruckApi.Features.Auth.Dtos.Login;
using TruckApi.Features.Auth.Errors;
using TruckApi.Features.Auth.Services;
using TruckApi.Features.Auth.UseCases;
using TruckApi.Features.Users.Interfaces;
using TruckApi.Infrastructure.Cache;
using TruckApi.Shared.Utils;

namespace TruckApi.Tests.Features.Auth.UseCases;

public class LoginUseCaseTests
{
    private readonly IUserRepository _userRepo = Substitute.For<IUserRepository>();
    private readonly ITokenService _tokenService = Substitute.For<ITokenService>();
    private readonly IRefreshTokenService _refreshTokenService =
        Substitute.For<IRefreshTokenService>();
    private readonly ICacheService _cache = Substitute.For<ICacheService>();
    private readonly LoginUseCase _sut;

    public LoginUseCaseTests()
    {
        _sut = new LoginUseCase(_userRepo, _tokenService, _refreshTokenService, _cache);
    }

    private static LoginRequest ValidRequest() => new("5511999999999", "senha123");

    private static User ActiveUser(UserRole role, string? companyId = "company_123") =>
        new()
        {
            Id = "user-1",
            FullName = "Carlos Souza",
            Whatsapp = "5511999999999",
            Password = PasswordHash.Hash("senha123"),
            Role = role,
            CompanyId = companyId,
            IsActive = true,
        };

    // --- Credenciais inválidas ---

    [Fact]
    public async Task WhenUserNotFound_ShouldReturnInvalidCredentials()
    {
        _userRepo.GetByWhatsappAsync(Arg.Any<string>()).Returns((User?)null);

        var result = await _sut.ExecuteAsync(ValidRequest());

        result
            .Should()
            .BeOfType<Result<LoginResponse>.Fail>()
            .Which.Error.Should()
            .Be(AuthErrors.InvalidCredentials);
    }

    [Fact]
    public async Task WhenWrongPassword_ShouldReturnInvalidCredentials()
    {
        var user = new User
        {
            Id = "user-1",
            FullName = "Carlos Souza",
            Whatsapp = "5511999999999",
            Password = PasswordHash.Hash("outrasenha"),
            Role = UserRole.Admin,
            IsActive = true,
        };
        _userRepo.GetByWhatsappAsync(Arg.Any<string>()).Returns(user);

        var result = await _sut.ExecuteAsync(ValidRequest());

        result
            .Should()
            .BeOfType<Result<LoginResponse>.Fail>()
            .Which.Error.Should()
            .Be(AuthErrors.InvalidCredentials);
    }

    // --- Usuário inativo ---

    [Fact]
    public async Task WhenUserInactive_ShouldReturnUserInactive()
    {
        var user = new User
        {
            Id = "user-1",
            FullName = "Carlos Souza",
            Whatsapp = "5511999999999",
            Password = PasswordHash.Hash("senha123"),
            Role = UserRole.Admin,
            IsActive = false,
        };
        _userRepo.GetByWhatsappAsync(Arg.Any<string>()).Returns(user);

        var result = await _sut.ExecuteAsync(ValidRequest());

        result
            .Should()
            .BeOfType<Result<LoginResponse>.Fail>()
            .Which.Error.Should()
            .Be(AuthErrors.UserInactive);
    }

    // --- Conta incompleta (não-Admin sem companyId) ---

    [Theory]
    [InlineData(UserRole.CompanyManager)]
    [InlineData(UserRole.CompanySupervisor)]
    [InlineData(UserRole.CompanyOperator)]
    public async Task WhenNonAdminWithoutCompanyId_ShouldReturnAccountIncomplete(UserRole role)
    {
        _userRepo.GetByWhatsappAsync(Arg.Any<string>()).Returns(ActiveUser(role, companyId: null));

        var result = await _sut.ExecuteAsync(ValidRequest());

        result
            .Should()
            .BeOfType<Result<LoginResponse>.Fail>()
            .Which.Error.Should()
            .Be(AuthErrors.AccountIncomplete);
    }

    // --- Login bem-sucedido ---

    [Theory]
    [InlineData(UserRole.CompanyManager)]
    [InlineData(UserRole.CompanySupervisor)]
    [InlineData(UserRole.CompanyOperator)]
    public async Task WhenNonAdminWithCompanyId_ShouldReturnToken(UserRole role)
    {
        _userRepo.GetByWhatsappAsync(Arg.Any<string>()).Returns(ActiveUser(role));
        _tokenService.Generate(Arg.Any<User>(), out Arg.Any<TimeSpan>()).Returns("jwt-token");
        _refreshTokenService.GenerateAsync(Arg.Any<string>()).Returns("refresh-token");

        var result = await _sut.ExecuteAsync(ValidRequest());

        result
            .Should()
            .BeOfType<Result<LoginResponse>.Ok>()
            .Which.Value.Token.Should()
            .Be("jwt-token");
    }

    [Fact]
    public async Task WhenAdmin_ShouldLoginWithoutCompanyId()
    {
        _userRepo
            .GetByWhatsappAsync(Arg.Any<string>())
            .Returns(ActiveUser(UserRole.Admin, companyId: null));
        _tokenService.Generate(Arg.Any<User>(), out Arg.Any<TimeSpan>()).Returns("jwt-token");
        _refreshTokenService.GenerateAsync(Arg.Any<string>()).Returns("refresh-token");

        var result = await _sut.ExecuteAsync(ValidRequest());

        result
            .Should()
            .BeOfType<Result<LoginResponse>.Ok>()
            .Which.Value.Token.Should()
            .Be("jwt-token");
    }
}

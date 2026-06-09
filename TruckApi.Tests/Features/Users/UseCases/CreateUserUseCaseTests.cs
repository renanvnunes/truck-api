using TruckApi.Features.Company.Errors;
using TruckApi.Features.Company.Interfaces;
using TruckApi.Features.Users.Dtos.CreateUser;
using TruckApi.Features.Users.Errors;
using TruckApi.Features.Users.Interfaces;
using TruckApi.Features.Users.UseCases;

namespace TruckApi.Tests.Features.Users.UseCases;

public class CreateUserUseCaseTests
{
    private readonly IUserRepository _userRepo = Substitute.For<IUserRepository>();
    private readonly ICompanyRepository _companyRepo = Substitute.For<ICompanyRepository>();
    private readonly CreateUserUseCase _sut;

    public CreateUserUseCaseTests()
    {
        _sut = new CreateUserUseCase(_userRepo, _companyRepo);
    }

    private static CreateUserRequest ValidAdmin() =>
        new(FullName: "João Silva", Whatsapp: "5511999999999", Role: UserRole.Admin);

    private static CreateUserRequest ValidNonAdmin(UserRole role, string? companyId = "company_123") =>
        new(FullName: "Carlos Souza", Whatsapp: "5511888888888", Role: role, CompanyId: companyId);

    // --- CompanyId inexistente ---

    [Theory]
    [InlineData(UserRole.CompanyManager)]
    [InlineData(UserRole.CompanySupervisor)]
    [InlineData(UserRole.CompanyOperator)]
    public async Task WhenNonAdminWithInvalidCompanyId_ShouldReturnCompanyNotFound(UserRole role)
    {
        _userRepo.WhatsappExistsAsync(Arg.Any<string>()).Returns(false);
        _companyRepo.ExistsAsync("company_123").Returns(false);

        var result = await _sut.ExecuteAsync(ValidNonAdmin(role));

        result
            .Should()
            .BeOfType<Result<User>.Fail>()
            .Which.Error.Should()
            .Be(CompanyErrors.NotFound);
    }

    // --- CompanyId válido ---

    [Theory]
    [InlineData(UserRole.CompanyManager)]
    [InlineData(UserRole.CompanySupervisor)]
    [InlineData(UserRole.CompanyOperator)]
    public async Task WhenNonAdminWithValidCompanyId_ShouldCreateUser(UserRole role)
    {
        _userRepo.WhatsappExistsAsync(Arg.Any<string>()).Returns(false);
        _companyRepo.ExistsAsync("company_123").Returns(true);
        _userRepo.CreateAsync(Arg.Any<User>()).Returns(x => x.Arg<User>());

        var result = await _sut.ExecuteAsync(ValidNonAdmin(role));

        result
            .Should()
            .BeOfType<Result<User>.Ok>()
            .Which.Value.CompanyId.Should()
            .Be("company_123");
    }

    // --- Admin não verifica companyId ---

    [Fact]
    public async Task WhenAdmin_ShouldNotCheckCompanyExistence()
    {
        _userRepo.WhatsappExistsAsync(Arg.Any<string>()).Returns(false);
        _userRepo.CreateAsync(Arg.Any<User>()).Returns(x => x.Arg<User>());

        await _sut.ExecuteAsync(ValidAdmin());

        await _companyRepo.DidNotReceive().ExistsAsync(Arg.Any<string>());
    }

    // --- Whatsapp duplicado ---

    [Fact]
    public async Task WhenWhatsappAlreadyExists_ShouldReturnConflict()
    {
        _userRepo.WhatsappExistsAsync("5511888888888").Returns(true);

        var result = await _sut.ExecuteAsync(ValidNonAdmin(UserRole.CompanyOperator));

        result
            .Should()
            .BeOfType<Result<User>.Fail>()
            .Which.Error.Should()
            .Be(UserErrors.WhatsappAlreadyExists);
    }
}

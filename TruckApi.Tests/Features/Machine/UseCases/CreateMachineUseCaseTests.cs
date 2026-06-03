using TruckApi.Features.Company.Errors;
using TruckApi.Features.Company.Interfaces;
using TruckApi.Features.Machine.Dtos.CreateMachine;
using TruckApi.Features.Machine.Errors;
using TruckApi.Features.Machine.UseCases;

namespace TruckApi.Tests.Features.Machine.UseCases;

public class CreateMachineUseCaseTests
{
    private readonly IMachineRepository _machineRepo = Substitute.For<IMachineRepository>();
    private readonly ICompanyRepository _companyRepo = Substitute.For<ICompanyRepository>();
    private readonly ICurrentUser _currentUser = Substitute.For<ICurrentUser>();
    private readonly CreateMachineUseCase _sut;

    public CreateMachineUseCaseTests()
    {
        _sut = new CreateMachineUseCase(_machineRepo, _companyRepo, _currentUser);
    }

    private static CreateMachineRequest ValidRequest(string? companyId = null)
    {
        return new("M001", "Excavator", "CAT", "320", 2020, "SN123", null, companyId);
    }

    private static UserSession AdminSession()
    {
        return new("u1", "Admin", "5511900000001", UserRole.Admin.ToString(), null, true);
    }

    private static UserSession ManagerSession(string companyId)
    {
        return new("u2", "Manager", "5511900000002", UserRole.CompanyManager.ToString(), companyId, true);
    }

    private static MachineEntity CreatedMachine(string companyId)
    {
        return new()
        {
            Id = "machine-1",
            Code = "M001",
            Type = MachineType.Excavator,
            Brand = "CAT",
            Model = "320",
            Year = 2020,
            SerialNumber = "SN123",
            CompanyId = companyId,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
        };
    }

    [Fact]
    public async Task WhenAdminWithoutCompanyId_ShouldReturnCompanyIdRequired()
    {
        _currentUser.Session.Returns(AdminSession());

        var result = await _sut.ExecuteAsync(ValidRequest(companyId: null));

        result
            .Should()
            .BeOfType<Result<MachineEntity>.Fail>()
            .Which.Error.Should()
            .Be(MachineErrors.CompanyIdRequired);
    }

    [Fact]
    public async Task WhenNotAdmin_ShouldUseSessionCompanyId()
    {
        _currentUser.Session.Returns(ManagerSession("company-1"));
        _companyRepo.ExistsAsync("company-1").Returns(true);
        _machineRepo.SerialNumberExistsAsync("SN123").Returns(false);
        _machineRepo.CodeExistsInCompanyAsync("M001", "company-1").Returns(false);
        _machineRepo.CreateAsync(Arg.Any<MachineEntity>()).Returns(x => x.Arg<MachineEntity>());

        var result = await _sut.ExecuteAsync(ValidRequest());

        result
            .Should()
            .BeOfType<Result<MachineEntity>.Ok>()
            .Which.Value.CompanyId.Should()
            .Be("company-1");
    }

    [Fact]
    public async Task WhenCompanyNotFound_ShouldReturnCompanyNotFound()
    {
        _currentUser.Session.Returns(ManagerSession("company-1"));
        _companyRepo.ExistsAsync("company-1").Returns(false);

        var result = await _sut.ExecuteAsync(ValidRequest());

        result
            .Should()
            .BeOfType<Result<MachineEntity>.Fail>()
            .Which.Error.Should()
            .Be(CompanyErrors.NotFound);
    }

    [Fact]
    public async Task WhenSerialNumberAlreadyExists_ShouldReturnConflict()
    {
        _currentUser.Session.Returns(ManagerSession("company-1"));
        _companyRepo.ExistsAsync("company-1").Returns(true);
        _machineRepo.SerialNumberExistsAsync("SN123").Returns(true);

        var result = await _sut.ExecuteAsync(ValidRequest());

        result
            .Should()
            .BeOfType<Result<MachineEntity>.Fail>()
            .Which.Error.Should()
            .Be(MachineErrors.SerialNumberAlreadyExists);
    }

    [Fact]
    public async Task WhenCodeAlreadyExistsInCompany_ShouldReturnConflict()
    {
        _currentUser.Session.Returns(ManagerSession("company-1"));
        _companyRepo.ExistsAsync("company-1").Returns(true);
        _machineRepo.SerialNumberExistsAsync("SN123").Returns(false);
        _machineRepo.CodeExistsInCompanyAsync("M001", "company-1").Returns(true);

        var result = await _sut.ExecuteAsync(ValidRequest());

        result
            .Should()
            .BeOfType<Result<MachineEntity>.Fail>()
            .Which.Error.Should()
            .Be(MachineErrors.CodeAlreadyExistsInCompany);
    }

    [Fact]
    public async Task WhenAdminProvidesCompanyId_ShouldUseRequestCompanyId()
    {
        _currentUser.Session.Returns(AdminSession());
        _companyRepo.ExistsAsync("company-99").Returns(true);
        _machineRepo.SerialNumberExistsAsync("SN123").Returns(false);
        _machineRepo.CodeExistsInCompanyAsync("M001", "company-99").Returns(false);
        _machineRepo.CreateAsync(Arg.Any<MachineEntity>()).Returns(x => x.Arg<MachineEntity>());

        var result = await _sut.ExecuteAsync(ValidRequest(companyId: "company-99"));

        result
            .Should()
            .BeOfType<Result<MachineEntity>.Ok>()
            .Which.Value.CompanyId.Should()
            .Be("company-99");
    }
}

using TruckApi.Features.Machine.Dtos.UpdateMachine;
using TruckApi.Features.Machine.Errors;
using TruckApi.Features.Machine.UseCases;

namespace TruckApi.Tests.Features.Machine.UseCases;

public class UpdateMachineUseCaseTests
{
    private readonly IMachineRepository _repo = Substitute.For<IMachineRepository>();
    private readonly ICurrentUser _currentUser = Substitute.For<ICurrentUser>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly UpdateMachineUseCase _sut;

    public UpdateMachineUseCaseTests()
    {
        _sut = new UpdateMachineUseCase(_repo, _currentUser, _unitOfWork);
    }

    private static MachineEntity MakeMachine(string id = "machine-1", string companyId = "company-1")
    {
        return new()
        {
            Id = id,
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

    private static UserSession AdminSession()
    {
        return new("u1", "Admin", "5511900000001", UserRole.Admin.ToString(), null, true);
    }

    private static UserSession ManagerSession(string companyId)
    {
        return new("u2", "Manager", "5511900000002", UserRole.CompanyManager.ToString(), companyId, true);
    }

    private static UpdateMachineRequest EmptyRequest()
    {
        return new(null, null, null, null, null, null, null);
    }

    [Fact]
    public async Task WhenMachineNotFound_ShouldReturnNotFound()
    {
        _repo.GetByIdAsync("machine-1").Returns((MachineEntity?)null);
        _currentUser.Session.Returns(AdminSession());

        var result = await _sut.ExecuteAsync("machine-1", EmptyRequest());

        result
            .Should()
            .BeOfType<Result<MachineEntity>.Fail>()
            .Which.Error.Should()
            .Be(MachineErrors.NotFound);
    }

    [Fact]
    public async Task WhenUserIsNotAdminAndDifferentCompany_ShouldReturnForbidden()
    {
        _repo.GetByIdAsync("machine-1").Returns(MakeMachine(companyId: "company-1"));
        _currentUser.Session.Returns(ManagerSession("company-2"));

        var result = await _sut.ExecuteAsync("machine-1", EmptyRequest());

        result
            .Should()
            .BeOfType<Result<MachineEntity>.Fail>()
            .Which.Error.Should()
            .Be(MachineErrors.Forbidden);
    }

    [Fact]
    public async Task WhenNewSerialNumberAlreadyExists_ShouldReturnConflict()
    {
        _repo.GetByIdAsync("machine-1").Returns(MakeMachine());
        _repo.SerialNumberExistsForOtherMachineAsync("SN999", "machine-1").Returns(true);
        _currentUser.Session.Returns(AdminSession());

        var result = await _sut.ExecuteAsync(
            "machine-1",
            EmptyRequest() with
            {
                SerialNumber = "SN999",
            }
        );

        result
            .Should()
            .BeOfType<Result<MachineEntity>.Fail>()
            .Which.Error.Should()
            .Be(MachineErrors.SerialNumberAlreadyExists);
    }

    [Fact]
    public async Task WhenNewCodeAlreadyExistsInCompany_ShouldReturnConflict()
    {
        _repo.GetByIdAsync("machine-1").Returns(MakeMachine());
        _repo
            .CodeExistsForOtherMachineInCompanyAsync("M999", "company-1", "machine-1")
            .Returns(true);
        _currentUser.Session.Returns(AdminSession());

        var result = await _sut.ExecuteAsync("machine-1", EmptyRequest() with { Code = "M999" });

        result
            .Should()
            .BeOfType<Result<MachineEntity>.Fail>()
            .Which.Error.Should()
            .Be(MachineErrors.CodeAlreadyExistsInCompany);
    }

    [Fact]
    public async Task WhenAllValid_ShouldApplyChangesAndReturnSuccess()
    {
        var machine = MakeMachine();
        _repo.GetByIdAsync("machine-1").Returns(machine);
        _currentUser.Session.Returns(AdminSession());

        var result = await _sut.ExecuteAsync(
            "machine-1",
            EmptyRequest() with
            {
                Brand = "Volvo",
                Year = 2023,
            }
        );

        var ok = result.Should().BeOfType<Result<MachineEntity>.Ok>().Which.Value;
        ok.Brand.Should().Be("Volvo");
        ok.Year.Should().Be(2023);
        await _repo.Received(1).UpdateAsync(machine);
    }
}

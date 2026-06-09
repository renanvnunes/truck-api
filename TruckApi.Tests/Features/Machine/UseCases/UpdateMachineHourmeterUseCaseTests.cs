using TruckApi.Features.Machine.Dtos.UpdateMachineHourmeter;
using TruckApi.Features.Machine.Errors;
using TruckApi.Features.Machine.UseCases;

namespace TruckApi.Tests.Features.Machine.UseCases;

public class UpdateMachineHourmeterUseCaseTests
{
    private readonly IMachineRepository _repo = Substitute.For<IMachineRepository>();
    private readonly ICurrentUser _currentUser = Substitute.For<ICurrentUser>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly UpdateMachineHourmeterUseCase _sut;

    public UpdateMachineHourmeterUseCaseTests()
    {
        _sut = new UpdateMachineHourmeterUseCase(_repo, _currentUser, _unitOfWork);
    }

    private static MachineEntity MakeMachine(decimal currentHourmeter = 100m, string companyId = "company-1")
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
            CurrentHourmeter = currentHourmeter,
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

    [Fact]
    public async Task WhenMachineNotFound_ShouldReturnNotFound()
    {
        _repo.GetByIdAsync("machine-1").Returns((MachineEntity?)null);
        _currentUser.Session.Returns(AdminSession());

        var result = await _sut.ExecuteAsync("machine-1", new(200m));

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

        var result = await _sut.ExecuteAsync("machine-1", new(200m));

        result
            .Should()
            .BeOfType<Result<MachineEntity>.Fail>()
            .Which.Error.Should()
            .Be(MachineErrors.Forbidden);
    }

    [Fact]
    public async Task WhenHourmeterIsLessThanCurrent_ShouldReturnError()
    {
        _repo.GetByIdAsync("machine-1").Returns(MakeMachine(currentHourmeter: 100m));
        _currentUser.Session.Returns(AdminSession());

        var result = await _sut.ExecuteAsync("machine-1", new(50m));

        result
            .Should()
            .BeOfType<Result<MachineEntity>.Fail>()
            .Which.Error.Code.Should()
            .Be("Machine.HourmeterCannotDecrease");
    }

    [Fact]
    public async Task WhenHourmeterIsValid_ShouldUpdateAndReturnSuccess()
    {
        var machine = MakeMachine(currentHourmeter: 100m);
        _repo.GetByIdAsync("machine-1").Returns(machine);
        _currentUser.Session.Returns(AdminSession());

        var result = await _sut.ExecuteAsync("machine-1", new(150m));

        var ok = result.Should().BeOfType<Result<MachineEntity>.Ok>().Which.Value;
        ok.CurrentHourmeter.Should().Be(150m);
        await _repo.Received(1).UpdateHourmeterAsync("machine-1", 150m);
    }

    [Fact]
    public async Task WhenHourmeterEqualsCurrentValue_ShouldReturnSuccess()
    {
        var machine = MakeMachine(currentHourmeter: 100m);
        _repo.GetByIdAsync("machine-1").Returns(machine);
        _currentUser.Session.Returns(AdminSession());

        var result = await _sut.ExecuteAsync("machine-1", new(100m));

        result.Should().BeOfType<Result<MachineEntity>.Ok>();
    }
}

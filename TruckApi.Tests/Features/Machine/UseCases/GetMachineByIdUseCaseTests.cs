using TruckApi.Features.Machine.Errors;
using TruckApi.Features.Machine.UseCases;

namespace TruckApi.Tests.Features.Machine.UseCases;

public class GetMachineByIdUseCaseTests
{
    private readonly IMachineRepository _repo = Substitute.For<IMachineRepository>();
    private readonly ICurrentUser _currentUser = Substitute.For<ICurrentUser>();
    private readonly GetMachineByIdUseCase _sut;

    public GetMachineByIdUseCaseTests()
    {
        _sut = new GetMachineByIdUseCase(_repo, _currentUser);
    }

    private static MachineEntity MakeMachine(string companyId = "company-1")
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

        var result = await _sut.ExecuteAsync("machine-1");

        result
            .Should()
            .BeOfType<Result<MachineEntity>.Fail>()
            .Which.Error.Should()
            .Be(MachineErrors.NotFound);
    }

    [Fact]
    public async Task WhenUserIsNotAdminAndDifferentCompany_ShouldReturnForbidden()
    {
        _repo.GetByIdAsync("machine-1").Returns(MakeMachine("company-1"));
        _currentUser.Session.Returns(ManagerSession("company-2"));

        var result = await _sut.ExecuteAsync("machine-1");

        result
            .Should()
            .BeOfType<Result<MachineEntity>.Fail>()
            .Which.Error.Should()
            .Be(MachineErrors.Forbidden);
    }

    [Fact]
    public async Task WhenUserIsAdmin_ShouldReturnSuccess()
    {
        _repo.GetByIdAsync("machine-1").Returns(MakeMachine("company-1"));
        _currentUser.Session.Returns(AdminSession());

        var result = await _sut.ExecuteAsync("machine-1");

        result.Should().BeOfType<Result<MachineEntity>.Ok>();
    }

    [Fact]
    public async Task WhenUserIsNotAdminAndSameCompany_ShouldReturnSuccess()
    {
        _repo.GetByIdAsync("machine-1").Returns(MakeMachine("company-1"));
        _currentUser.Session.Returns(ManagerSession("company-1"));

        var result = await _sut.ExecuteAsync("machine-1");

        result.Should().BeOfType<Result<MachineEntity>.Ok>();
    }
}

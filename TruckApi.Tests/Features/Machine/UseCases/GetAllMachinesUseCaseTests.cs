using TruckApi.Features.Machine.UseCases;

namespace TruckApi.Tests.Features.Machine.UseCases;

public class GetAllMachinesUseCaseTests
{
    private readonly IMachineRepository _repo = Substitute.For<IMachineRepository>();
    private readonly ICurrentUser _currentUser = Substitute.For<ICurrentUser>();
    private readonly GetAllMachinesUseCase _sut;

    public GetAllMachinesUseCaseTests()
    {
        _sut = new GetAllMachinesUseCase(_repo, _currentUser);
    }

    private static UserSession AdminSession()
    {
        return new("u1", "Admin", "5511900000001", UserRole.Admin.ToString(), null, true);
    }

    private static UserSession ManagerSession(string companyId)
    {
        return new("u2", "Manager", "5511900000002", UserRole.CompanyManager.ToString(), companyId, true);
    }

    private static MachineEntity[] MakeMachines(int count)
    {
        return Enumerable
            .Range(1, count)
            .Select(i => new MachineEntity
            {
                Id = $"machine-{i}",
                Code = $"M{i:000}",
                Type = MachineType.Truck,
                Brand = "CAT",
                Model = "320",
                Year = 2020,
                SerialNumber = $"SN{i}",
                CompanyId = "company-1",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
            })
            .ToArray();
    }

    [Fact]
    public async Task WhenAdmin_ShouldQueryWithNullCompanyId()
    {
        _currentUser.Session.Returns(AdminSession());
        _repo.GetAllAsync(null, 11, null).Returns([]);

        await _sut.ExecuteAsync(null, 10);

        await _repo.Received(1).GetAllAsync(null, 11, null);
    }

    [Fact]
    public async Task WhenNotAdmin_ShouldQueryWithSessionCompanyId()
    {
        _currentUser.Session.Returns(ManagerSession("company-1"));
        _repo.GetAllAsync(null, 11, "company-1").Returns([]);

        await _sut.ExecuteAsync(null, 10);

        await _repo.Received(1).GetAllAsync(null, 11, "company-1");
    }

    [Fact]
    public async Task WhenResultExceedsLimit_ShouldReturnNextCursor()
    {
        _currentUser.Session.Returns(AdminSession());
        var machines = MakeMachines(11);
        _repo.GetAllAsync(null, 11, null).Returns(machines);

        var (result, nextCursor) = await _sut.ExecuteAsync(null, 10);

        result.Should().HaveCount(10);
        nextCursor.Should().Be("machine-10");
    }

    [Fact]
    public async Task WhenResultWithinLimit_ShouldReturnNullCursor()
    {
        _currentUser.Session.Returns(AdminSession());
        _repo.GetAllAsync(null, 11, null).Returns(MakeMachines(5));

        var (result, nextCursor) = await _sut.ExecuteAsync(null, 10);

        result.Should().HaveCount(5);
        nextCursor.Should().BeNull();
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-5, 1)]
    [InlineData(200, 100)]
    public async Task WhenLimitOutOfRange_ShouldClamp(int inputLimit, int expectedClampedLimit)
    {
        _currentUser.Session.Returns(AdminSession());
        _repo.GetAllAsync(null, Arg.Any<int>(), null).Returns([]);

        await _sut.ExecuteAsync(null, inputLimit);

        await _repo.Received(1).GetAllAsync(null, expectedClampedLimit + 1, null);
    }
}

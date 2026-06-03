using TruckApi.Features.Machine.Errors;
using TruckApi.Features.Machine.UseCases;

namespace TruckApi.Tests.Features.Machine.UseCases;

public class DeleteMachineUseCaseTests
{
    private readonly IMachineRepository _repo = Substitute.For<IMachineRepository>();
    private readonly DeleteMachineUseCase _sut;

    public DeleteMachineUseCaseTests()
    {
        _sut = new DeleteMachineUseCase(_repo);
    }

    private static MachineEntity MakeMachine()
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
            CompanyId = "company-1",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
        };
    }

    [Fact]
    public async Task WhenMachineNotFound_ShouldReturnNotFound()
    {
        _repo.GetByIdAsync("machine-1").Returns((MachineEntity?)null);

        var result = await _sut.ExecuteAsync("machine-1");

        result
            .Should()
            .BeOfType<Result<bool>.Fail>()
            .Which.Error.Should()
            .Be(MachineErrors.NotFound);
    }

    [Fact]
    public async Task WhenMachineExists_ShouldRemoveAndReturnSuccess()
    {
        _repo.GetByIdAsync("machine-1").Returns(MakeMachine());

        var result = await _sut.ExecuteAsync("machine-1");

        result.Should().BeOfType<Result<bool>.Ok>().Which.Value.Should().BeTrue();
        await _repo.Received(1).RemoveAsync("machine-1");
    }
}

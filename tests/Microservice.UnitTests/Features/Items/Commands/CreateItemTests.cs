 using Microservice.Features.Items.Commands;
 using Microservice.Features.Items.Domain;
namespace Microservice.UnitTests.Features.Items.Commands;

public class CreateItemTests
{
    private readonly Mock<IRepositoryBase<Item>> _mockRepository = new();
    private readonly Mock<IAppLogger<CreateItem.Handler>> _mockLogger = new();

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnNewItemId()
    {
        // Arrange
        var handler = new CreateItem.Handler( _mockLogger.Object,_mockRepository.Object);
        var command = new CreateItem.Command( "Test Items");

        // Act
        var result = await handler.Handle(command,CancellationToken.None);

        // Assert
         Assert.IsType<Guid>(result);
        _mockRepository.Verify(repo => repo.CreateAsync
            (It.IsAny<Item>()), Times.Once);
        _mockLogger.Verify(logger => logger.LogInformation(
            "Creating a new item with name: {Name}", command.Name), Times.Once);
        _mockLogger.Verify(logger => logger.LogInformation(
            "Items created successfully with ID: {ItemId}", result), Times.Once);
    }

  
}
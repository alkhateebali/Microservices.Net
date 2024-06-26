using Microservice.Core.Exceptions;
using Microservice.Features.Items.Commands;
using Microservice.Features.Items.Domain;
namespace Microservice.UnitTests.Features.Items.Commands;

public class DeleteItemTests
{
    private readonly Mock<IAppLogger<DeleteItem.Handler>> _mockLogger;
    private readonly Mock<IRepositoryBase<Item>> _mockRepository;
    private readonly DeleteItem.Handler _handler;

    public DeleteItemTests()
    {
        _mockLogger = new Mock<IAppLogger<DeleteItem.Handler>>();
        _mockRepository = new Mock<IRepositoryBase<Item>>();
        _handler = new DeleteItem.Handler(_mockLogger.Object, _mockRepository.Object);
    }

    [Fact]
    public async Task Handle_ShouldDeleteItem()
    {
        // Arrange
        var itemId = Guid.NewGuid();
        var item = new Item { Id = itemId, Name = "Test Item" };
        _mockRepository.Setup(repo => repo.GetByIdAsync(itemId, It.IsAny<CancellationToken>())).ReturnsAsync(item);
        _mockRepository.Setup(repo => repo.DeleteAsync(item)).Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(new DeleteItem.Command(itemId), CancellationToken.None);

        // Assert
        _mockRepository.Verify(repo => repo.GetByIdAsync(itemId, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(repo => repo.DeleteAsync(item), Times.Once);
        _mockLogger.Verify(logger => logger.LogInformation("Deleting item with ID: {ItemId}", itemId), Times.Once);
        // _mockLogger.Verify(logger => logger.LogInformation("Item with ID: {ItemId} deleted successfully", itemId), Times.Once);    }
    }


    

}
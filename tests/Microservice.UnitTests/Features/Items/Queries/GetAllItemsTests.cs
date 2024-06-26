using Microservice.Features.Items.Commands;
using Microservice.Features.Items.Domain;
using Microservice.Features.Items.Queries;
namespace Microservice.UnitTests.Features.Items.Queries;

public class GetAllItemsTests
{
    private readonly Mock<IRepositoryBase<Item>> _mockRepository = new();
    private readonly Mock<IAppLogger<CreateItem.Handler>> _mockLogger = new();
    private readonly GetAllItems.Handler _handler;

    public GetAllItemsTests()
    {
        _handler = new GetAllItems.Handler(_mockLogger.Object, _mockRepository.Object);
    }
    
    [Fact]
    public async Task GetAllItems_ValidQuery_ShouldReturnAllItems()
    {
        var items = new List<Item>
        {
            new Item { Id = Guid.NewGuid(), Name = "Item 1" },
            new Item { Id = Guid.NewGuid(), Name = "Item 2" }
        };
        _mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(items);

        // Act
        var result = await _handler.Handle(new GetAllItems(), CancellationToken.None);

        // Assert
        Assert.Equal(items, result);
        _mockLogger.Verify(logger => logger.LogInformation("Fetching all items..."), Times.Once);
        _mockLogger.Verify(logger => logger.LogInformation("Fetched {Count} items", items.Count), Times.Once);

    }
    [Fact]
    public async Task Handle_WhenExceptionThrown_ShouldLogErrorAndRethrow()
    {
        // Arrange
        var exception = new Exception("Test exception");
        _mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ThrowsAsync(exception);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(new GetAllItems(), CancellationToken.None));
        Assert.Equal(exception, ex);
        _mockLogger.Verify(logger => logger.LogInformation("Fetching all items..."), Times.Once);
        _mockLogger.Verify(logger => logger.LogError(exception, "Error occurred while fetching items"), Times.Once);
    }
    
}
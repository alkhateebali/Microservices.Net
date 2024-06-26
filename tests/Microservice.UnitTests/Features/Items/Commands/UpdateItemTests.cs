using Microservice.Features.Items.Commands;
using Microservice.Features.Items.Domain;

namespace Microservice.UnitTests.Features.Items.Commands;

public class UpdateItemTests
{
    private readonly Mock<IAppLogger<UpdateItem.Handler>> _mockLogger;
    private readonly Mock<IRepositoryBase<Item>> _mockRepository;
    private readonly UpdateItem.Handler _handler;

    public UpdateItemTests()
    {
        _mockLogger = new Mock<IAppLogger<UpdateItem.Handler>>();
        _mockRepository = new Mock<IRepositoryBase<Item>>();
        _handler = new UpdateItem.Handler(_mockLogger.Object, _mockRepository.Object);
    }
     [Fact]
        public async Task Handle_ShouldUpdateItem()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            const string itemName = "Updated Item";
            var item = new Item { Id = itemId, Name = "Old Item" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(itemId, It.IsAny<CancellationToken>())).ReturnsAsync(item);
      
            // Act
             await _handler.Handle(new UpdateItem.Command(itemId, itemName), CancellationToken.None);

            // Assert
            _mockRepository.Verify(repo => repo.GetByIdAsync(itemId, It.IsAny<CancellationToken>()), Times.Once);
            _mockRepository.Verify(repo => repo.UpdateAsync(It.Is<Item>(i => i.Id == itemId && i.Name == itemName)), Times.Once);
             _mockLogger.Verify(logger => logger.LogInformation("Updating item with ID: {ItemId}", itemId), Times.Once);
             // _mockLogger.Verify(logger => logger.LogInformation("Item with ID: {ItemId} updated successfully", itemId), Times.Once);
        }

      
    }



 using Microservice.Features.Items.Commands;
 using Microservice.Features.Items.Domain;
 using Microservice.Features.Items.Queries;

 namespace Microservice.UnitTests.Features.Items.Queries;

public class GetItemTests
{
    private readonly Mock<IRepositoryBase<Item>> _mockRepository = new();
    private readonly Mock<IAppLogger<CreateItem.Handler>> _mockLogger = new();

    [Fact]
    public async Task Handle_ValidQuery_ShouldReturnItemNyId()
    {
        {
            // Arrange
            var query =  new GetItem(new Guid() );
            var handler = new GetItem.Handler(_mockLogger.Object,_mockRepository.Object );
            var item = new Item { Id = query.Id, Name = "Test Item"};
            _mockRepository.Setup(repo => repo.GetByIdAsync(query.Id,CancellationToken.None)).ReturnsAsync(item);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(item, result); 
        }
    }
}
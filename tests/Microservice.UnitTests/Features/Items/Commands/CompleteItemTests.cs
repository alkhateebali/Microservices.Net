using Microservice.Features.Items.Commands;
using Microservice.Features.Items.Domain;
using Microservice.Features.Items.Domain.Events;

namespace Microservice.UnitTests.Features.Items.Commands;

public class CompleteItemTests
{
    [Fact]
    public void Item_Complete_ShouldUpdateCompleted()
    {
        // Arrange
        var item = new Item()
        {
            Id = Guid.NewGuid(),
            Name = "New item"
        };
        
        // Act
        item.Complete();
        
        // Assert
        Assert.True(item.IsCompleted);
    }
    
    [Fact]
    public void Item_Complete_ShouldAddEvent()
    {
        // Arrange
        var item = new Item()
        {
            Id = Guid.NewGuid(),
            Name = "New item"
        };
        
        // Act
        item.Complete();
        
        // Assert
        Assert.Contains(item.StagedEvents, x => x is ItemCompletedEvent);   
    }
}
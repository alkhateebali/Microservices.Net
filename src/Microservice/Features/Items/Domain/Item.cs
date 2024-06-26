using System.ComponentModel.DataAnnotations;
using Microservice.Core.Domain;
using Microservice.Features.Items.Domain.Events;

namespace Microservice.Features.Items.Domain;

public class Item:BaseEntity
{
    public Item() 
    {
        StagedEvents.Add(new ItemCreatedEvent(Id));
    }
    [Required]
    [MaxLength(500)]
    public required string Name { get;  set; }

    public bool IsCompleted { get; set; }
    
    /// <exception cref="InvalidOperationException">Throws when trying to complete an already completed item</exception>
    public void Complete()
    {
        if (IsCompleted)
        {
            throw new InvalidOperationException("Items is already completed");
        }
        
        IsCompleted = true;
    
        StagedEvents.Add(new ItemCompletedEvent(Id));
    }
}
using MediatR;

namespace Microservice.Core.Domain;

public abstract class BaseEntity
{
   
    public Guid Id { get; init; } 
    public readonly List<INotification> StagedEvents = [];
}
using MediatR;

namespace Microservice.Features.Items.Domain.Events;


    public record ItemCompletedEvent(Guid ItemId) : INotification;
    public record ItemCreatedEvent(Guid ItemId) : INotification;

using System.Text;
using MassTransit;
using RabbitMQ.Client;

namespace Microservice.Infrastructure.Messaging;
public interface IMessageProducer
{
    Task SendMessageAsync(SampleMessage message);
}
public class Producer(ISendEndpointProvider sendEndpointProvider) 
    : IMessageProducer
{
    public async Task SendMessageAsync(SampleMessage message)
    {
        var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://localhost/example-queue"));
        await sendEndpoint.Send(message);
    }
}

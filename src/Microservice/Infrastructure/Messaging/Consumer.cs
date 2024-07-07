

using MassTransit;
namespace Microservice.Infrastructure.Messaging;
public abstract class Consumer : IConsumer<SampleMessage>
{
        public Task Consume(ConsumeContext<SampleMessage> context)
        {
            // Handle the message
            Console.WriteLine("Received message: " + context.Message.Text);
            return Task.CompletedTask;
            
        }
}

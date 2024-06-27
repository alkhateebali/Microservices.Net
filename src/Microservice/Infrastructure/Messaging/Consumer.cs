#if (messaging)
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Microservice.Infrastructure.Messaging;

public class Consumer(RabbitMqSettings settings )
{
   
    public void StartConsuming()
    {
        var factory = new ConnectionFactory() { HostName = settings.Host, UserName =settings.UserName, Password = settings.Password };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: "sample-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("Received message: " + message);
        };
        channel.BasicConsume(queue: "sample-queue", autoAck: true, consumer: consumer);
    }
}
#endif

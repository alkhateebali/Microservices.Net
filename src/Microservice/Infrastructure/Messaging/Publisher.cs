#if (messaging)
using System.Text;
using RabbitMQ.Client;

namespace Microservice.Infrastructure.Messaging;

public class Publisher(RabbitMqSettings settings)
{
  

    public void Publish(string message)
    {
        var factory = new ConnectionFactory() { HostName = settings.Host, UserName =settings.UserName, Password = settings.Password };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: "sample-queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "", routingKey: "sample-queue", basicProperties: null, body: body);
    }
}
#endif
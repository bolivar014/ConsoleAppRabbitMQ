using RabbitMQ.Client;
using System.Text;

class Program
{
    static async Task Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        // Creación de exchange
        channel.ExchangeDeclare(exchange: "my_exchange", type: "direct");

        // Creación de Queue
        channel.QueueDeclare(queue: "my_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        // Vinculamos Exchenge a queue
        channel.QueueBind(queue: "my_queue", exchange: "my_exchange", routingKey: "my_routing_key");

        // Enviamos mensaje de prueba
        var message = "Hello, RabbitMQ!";
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "my_exchange", routingKey: "my_routing_key", basicProperties: null, body: body);
        Console.WriteLine(" [x] Sent {0}", message);

    }
}
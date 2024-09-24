using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // await CreateConectionRabbit();
            var factory = await CreateConnectionFactory();
            
            // 
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Creación de exchange
            try
            {
                channel.ExchangeDeclare(exchange: "my_exchange", type: "direct", durable: true);
                Console.WriteLine("Exchange declared successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error declaring exchange: {ex.Message}");
            }

            // Creación de Queue
            try
            {
                channel.QueueDeclare(queue: "my_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                Console.WriteLine("Queue declared successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error declaring queue: {ex.Message}");
            }
            
            // Vinculamos Exchenge a queue
            channel.QueueBind(queue: "my_queue", exchange: "my_exchange", routingKey: "my_routing_key");


        }
        catch (Exception ex)
        {
            Console.WriteLine("Error CreateConnectionFactory: " + ex.Message.ToString());
        }
    }
    static async Task<ConnectionFactory> CreateConnectionFactory()
    {
        return new ConnectionFactory() { HostName = "localhost" };
    }

    static async Task CreateConectionRabbit()
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
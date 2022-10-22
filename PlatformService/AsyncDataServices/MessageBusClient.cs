using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

public class MessageBusClient : IMessageBusClient, IDisposable
{
    private readonly IConfiguration _config;
    protected IConnection Connection { get; set; }
    protected IModel Channel { get; set; }

    public MessageBusClient(IConfiguration configuration)
    {
        _config = configuration;
        CreateConnection();
    }

    protected virtual void CreateConnection()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _config["RabbitMQHost"],
            Port = int.Parse(_config["RabbitMQPort"])
        };
        try
        {
            Connection = factory.CreateConnection();
            Channel = Connection.CreateModel();

            // document magic strings - exchange
            Channel.ExchangeDeclare("trigger", ExchangeType.Fanout);

            Connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

            System.Console.WriteLine("--> Connected to message bus");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"--> Could not connect to Message Bus: {ex.Message}");
        }
    }

    private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        System.Console.WriteLine("--> RabbitMQ Connection Shutdown");
    }

    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        var message = JsonSerializer.Serialize(platformPublishedDto);

        if (Connection.IsOpen)
        {
            System.Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
            SendMessage(message);
            return;
        }

        System.Console.WriteLine("--> RabbitMQ connections closed, not sending");
    }

    protected virtual void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        // FanOut Exchange does not need a route key.
        // document magic strings - exchange
        Channel.BasicPublish("trigger", "", null, body);

        System.Console.WriteLine($"--> We have send {message}");
    }

    public void Dispose()
    {
        System.Console.WriteLine("--> Message Bus Disposed");
        if (Channel.IsOpen)
            Channel.Close();
        if (Connection.IsOpen)
            Connection.Close();
    }
}
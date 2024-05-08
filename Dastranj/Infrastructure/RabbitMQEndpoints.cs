namespace Dastranj.Infrastructure;

public class RabbitMQEndpoints
{
    public string QueueName { get; set; }
    public string ExchangeName { get; set; }
    public string ConsumerType { get; set; }

    public static List<RabbitMQEndpoints> GetAllEndpoints(IConfiguration configuration)
    {
        var endpointsConfig = configuration.GetSection("RabbitMQEndpoints").Get<List<RabbitMQEndpoints>>();
        return endpointsConfig ?? new List<RabbitMQEndpoints>();
    }
}
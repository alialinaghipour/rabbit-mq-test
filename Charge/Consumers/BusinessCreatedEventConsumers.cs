using MassTransit;
using SalesSystemPackages.Events;

namespace Charge.Consumers;


public class BusinessCreatedEventConsumers : IConsumer<BusinessCreatedEvent>
{
    private readonly IConfiguration _configuration;

    public BusinessCreatedEventConsumers(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task Consume(ConsumeContext<BusinessCreatedEvent> context)
    {
        var productId = GetProductIdFromAppSettings();

        if (context.Message.ProductId == "charge-error")
        {
            throw new Exception("charge-error");
        }

        if (context.Message.ProductId == productId)
        {
            Console.WriteLine("***********************************\n" +
                              "received: \n" + context.Message);
        }
        else
        {
            Console.WriteLine("*****************************\n" +
                              "event is not for charge service\n" +
                              "productId: " + context.Message.ProductId);
        }

        return Task.CompletedTask;
    }

    private string GetProductIdFromAppSettings()
    {
        return
            _configuration.GetValue<string>("SalesSystem:ProductId")!;
    }
}
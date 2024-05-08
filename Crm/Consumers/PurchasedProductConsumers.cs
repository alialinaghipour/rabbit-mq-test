using MassTransit;
using Taav.SalesSystem.Events;

namespace Crm.Consumers;

public class PurchasedProductConsumers : IConsumer<PackagePurchasedEvent>
{
    private readonly IConfiguration _configuration;

    public PurchasedProductConsumers(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task Consume(ConsumeContext<PackagePurchasedEvent> context)
    {
        var productId = GetProductIdFromAppSettings();

        if (context.Message.ProductId == "crm-error")
        {
            throw new Exception("crm-error");
        }

        if (context.Message.ProductId == productId)
        {
            Console.WriteLine("***********************************\n" +
                              "received: \n" + context.Message);
        }
        else
        {
            Console.WriteLine("*****************************\n" +
                              "event is not for crm service\n" +
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
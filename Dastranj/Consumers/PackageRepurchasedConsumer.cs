using MassTransit;
using SalesSystemPackages.Events;

namespace Dastranj.Consumers;

public class PackageRepurchasedConsumer : IConsumer<PackageRepurchased>
{
    private readonly IConfiguration _configuration;

    public PackageRepurchasedConsumer(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task Consume(ConsumeContext<PackageRepurchased> context)
    {
        var productId = GetProductIdFromAppSettings();

        if (context.Message.ProductId == "dastranj-error")
        {
            throw new Exception("dastranj-error");
        }

        if (context.Message.ProductId == productId)
        {
            Console.WriteLine("***********************************\n" +
                              "received: \n" + context.Message);
        }
        else
        {
            Console.WriteLine("*****************************\n" +
                              "event is not for dastranj service\n" +
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
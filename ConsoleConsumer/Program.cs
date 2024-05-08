using Microsoft.Extensions.Hosting;
using ConsoleConsumer.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Taav.SalesSystem;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext,config)=>{
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        config.AddEnvironmentVariables();
})
.ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        services.AddMassTransit(x =>
        {
            x.AddConsumer<BusinessCreatedEventConsumers>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["EventBusConfigs:HostAddress"]);

                cfg.ReceiveEndpoint("console-consumer", e =>
                {
                    e.Bind(ExchangeNames.PackagePurchased);
                    e.ConfigureConsumer<BusinessCreatedEventConsumers>(context);
                });
            });
        });
                
        services.AddMassTransitHostedService();
    })
    .Build();
    
    
await host.RunAsync();
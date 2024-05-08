using Charge.Consumers;
using Infrastructure.Publisher;
using MassTransit;
using RabbitMQ.Client;
using SalesSystemPackages.Commons;
using Taav.SalesSystem;
using Taav.SalesSystem.Events;

namespace Charge.Infrastructure;

public static class PublisherConfigs
{
    public static IServiceCollection AddTaavMassTransit(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddScoped<TaavPublisher, MassTransitTaavPublisher>();
        services.AddMassTransit(options =>
        {
            options.AddConsumers(typeof(Program).Assembly);
            
            options.UsingRabbitMq((ctx, conf) =>
            {
                conf.Host(configuration.GetValue<string>("EventBusConfigs:HostAddress"));
                
                conf.ReceiveEndpoint("charge-consumer", c =>
                {
                    c.Bind("business-created-fanout");
                    c.ConfigureConsumer<BusinessCreatedEventConsumers>(ctx);
                });
                
                conf.Send<QuantityIncreasedEvent>(s =>
                {
                    s.UseRoutingKeyFormatter(c=> RoutingKey.QuantityIncreased);
                });
                
                conf.Message<QuantityIncreasedEvent>(m => m.SetEntityName(ExchangeNames.QuantityIncreased));
                conf.Publish<QuantityIncreasedEvent>(e => e.ExchangeType = ExchangeType.Direct);
                
                
                conf.ReceiveEndpoint(queueName:"QuantityIncreased1", e =>
                {
                    e.ConfigureConsumer<QuantityIncreasedEventConsumer>(ctx);
                    // Bind the queue to an exchange with a specific routing key
                    e.Bind(ExchangeNames.QuantityIncreased, s =>
                    {
                        s.RoutingKey = "keykey";
                        s.ExchangeType = ExchangeType.Direct;
                    });
                });
                
                conf.ReceiveEndpoint("charge-consumer-2", c =>
                {
                    c.Bind("package-repurchased-fanout");
                    c.ConfigureConsumer<PackageRepurchasedConsumer>(ctx);
                });
            });
        });

        services.AddScoped<BusinessCreatedEventConsumers>();
        services.AddMassTransitHostedService();

        return services;
    }
}
using Crm.Consumers;
using GreenPipes;
using Infrastructure;
using Infrastructure.Publisher;
using MassTransit;
using RabbitMQ.Client;
using SalesSystemPackages.Commons;
using SalesSystemPackages.Events;
using Taav.SalesSystem;
using Taav.SalesSystem.Events;

namespace Crm.Infrastructure;

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
                conf.Host(
                    configuration.GetValue<string>(
                        "EventBusConfigs:HostAddress"));

                conf.ReceiveEndpoint(queueName: "crm-consumer", c =>
                {
                    c.Durable = true;
                    c.PrefetchCount = 16;
                    c.ConcurrentMessageLimit = 10;
                    c.UseMessageRetry(_ =>
                        _.Interval(retryCount:3,interval: TimeSpan.FromSeconds(5)));
                    
                    c.Bind(exchangeName: "business-created-fanout");
                    c.ConfigureConsumer<BusinessCreatedEventConsumers>(ctx);
                });


                conf.ReceiveEndpoint(queueName: "crm-consumer-2", c =>
                {
                    c.Durable = true;
                    c.PrefetchCount = 16;
                    c.ConcurrentMessageLimit = 10;
                    // c.UseMessageRetry(_ =>
                    //     _.Interval(retryCount:3,interval: TimeSpan.FromSeconds(5)));
                    
                    c.Bind(exchangeName: ExchangeNames.PackagePurchased);
                    c.ConfigureConsumer<PurchasedProductConsumers>(ctx);
                });
                
                conf.ReceiveEndpoint("crm-consumer-3", c =>
                {
                    c.Bind("package-repurchased-fanout");
                    c.ConfigureConsumer<PackageRepurchasedConsumer>(ctx);
                });


                conf.Send<QuantityIncreasedEvent>(s =>
                {
                    s.UseRoutingKeyFormatter(c=> RoutingKey.QuantityIncreased);
                });
                
                conf.Message<QuantityIncreasedEvent>(m => m.SetEntityName(ExchangeNames.QuantityIncreased));
                conf.Publish<QuantityIncreasedEvent>(e => e.ExchangeType = ExchangeType.Direct);
                
            });
        });

        services.AddScoped<BusinessCreatedEventConsumers>();
        services.AddMassTransitHostedService();

        return services;
    }
}
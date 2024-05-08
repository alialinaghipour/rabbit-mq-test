using SalesSystem.Consumers;

namespace SalesSystem.Infrastructure;

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
                
                conf.Message<BusinessCreatedEvent>(_=> _.SetEntityName(entityName: "business-created-fanout"));
                conf.Publish<BusinessCreatedEvent>(_=>_.ExchangeType = ExchangeType.Fanout);
                
                
                conf.Message<PackagePurchasedEvent>(_=> _.SetEntityName(entityName: ExchangeNames.PackagePurchased));
                conf.Publish<PackagePurchasedEvent>(_=>_.ExchangeType = ExchangeType.Fanout);
                
                conf.ReceiveEndpoint(queueName:"QuantityIncreased", e =>
                {
                    e.ConfigureConsumer<QuantityIncreasedEventConsumer>(ctx);
                    // Bind the queue to an exchange with a specific routing key
                    e.Bind(ExchangeNames.QuantityIncreased, s =>
                    {
                        s.RoutingKey = RoutingKey.QuantityIncreased;
                        s.ExchangeType = ExchangeType.Direct;
                    });
                });
            });
        });
        services.AddMassTransitHostedService();

        return services;
    }
}
using Dastranj.Consumers;
using Infrastructure.Publisher;
using MassTransit;
using SalesSystemPackages.Events;

namespace Dastranj.Infrastructure;

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
                
                conf.ReceiveEndpoint("dastranj-consumer", c =>
                {
                    c.Bind("business-created-fanout");
                    c.ConfigureConsumer<BusinessCreatedEventConsumers>(ctx);
                });
                
                conf.ReceiveEndpoint("dastranj-consumer-2", c =>
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
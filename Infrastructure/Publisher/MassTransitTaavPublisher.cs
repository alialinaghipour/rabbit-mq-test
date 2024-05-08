using MassTransit;

namespace Infrastructure.Publisher;

public class MassTransitTaavPublisher : TaavPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitTaavPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task Publish<T>(
        T message,
        CancellationToken cancellationToken = default)
        where T : class
    {
        await _publishEndpoint.Publish(message, cancellationToken);
    }
}
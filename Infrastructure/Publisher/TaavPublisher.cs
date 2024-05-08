namespace Infrastructure.Publisher;

public interface TaavPublisher
{
    Task Publish<T>(T message, CancellationToken cancellationToken = default)
        where T : class;
}
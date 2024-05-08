namespace SalesSystem.Consumers;

public class QuantityIncreasedEventConsumer : IConsumer<QuantityIncreasedEvent>
{
    public async Task Consume(ConsumeContext<QuantityIncreasedEvent> context)
    {
        Console.WriteLine("**********************************\n" +
                          "received : "+context.Message);
        await Task.CompletedTask;
    }
}
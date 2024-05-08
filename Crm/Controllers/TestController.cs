using Infrastructure.Publisher;
using Microsoft.AspNetCore.Mvc;
using Taav.SalesSystem.Events;

namespace Crm.Controllers;

[ApiController]
[Route("customers")]
public class TestController : ControllerBase
{
    private readonly TaavPublisher _taavPublisher;

    public TestController(TaavPublisher taavPublisher)
    {
        _taavPublisher = taavPublisher;
    }

    [HttpPost]
    public async Task QuantityIncreased(QuantityIncreasedEvent @event)
    {
        await _taavPublisher.Publish(@event);
        Console.WriteLine("publish - "+nameof(QuantityIncreasedEvent));
    }
}
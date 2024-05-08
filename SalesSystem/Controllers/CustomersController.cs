using Microsoft.AspNetCore.Mvc;

namespace SalesSystem.Controllers;


[ApiController]
[Route("customers")]
public class CustomersController : ControllerBase
{
    private readonly TaavPublisher _taavPublisher;

    public CustomersController(TaavPublisher taavPublisher)
    {
        _taavPublisher = taavPublisher;
    }

    [HttpPost("create-business")]
    public void CreateBusiness(BusinessCreatedEvent @event)
    {
        try
        {
            Console.WriteLine("published: "+@event);
            _taavPublisher.Publish(@event);
        }
        catch (Exception e)
        {
            Console.WriteLine("error: "+e.Message);
        }
    }
    
    
    [HttpPost("package-purchased")]
    public void CreateBusiness(PackagePurchasedEvent @event)
    {
        try
        {
            Console.WriteLine("published: "+@event);
            _taavPublisher.Publish(@event);
        }
        catch (Exception e)
        {
            Console.WriteLine("error: "+e.Message);
        }
    }
    
    [HttpPost("package-repurchased")]
    public void CreateBusiness(PackageRepurchased @event)
    {
        try
        {
            Console.WriteLine("published: "+@event);
            _taavPublisher.Publish(@event);
        }
        catch (Exception e)
        {
            Console.WriteLine("error: "+e.Message);
        }
    }
}
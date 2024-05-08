using Taav.SalesSystem.Events;

namespace Crm;

public static class Runner
{
    static bool isRunning = false;

    public static void Run()
    {
        while (isRunning)
        {
            Console.WriteLine("publish "+nameof(QuantityIncreasedEvent)+" = 1");
            var value = Console.ReadLine();
            if (value == "1")
            {
                
            }
        }
    }
}
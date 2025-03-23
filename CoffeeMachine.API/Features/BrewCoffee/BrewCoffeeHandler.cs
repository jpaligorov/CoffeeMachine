using CoffeeMachine.ActionResults;
using CoffeeMachine.Common;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Features.BrewCoffee;

public class BrewCoffeeHandler
{
    private static int _requestCount;
    private readonly IDateTimeProvider _dateTimeProvider;

    public BrewCoffeeHandler(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public IActionResult HandleRequest()
    {
        var currentCount = ++_requestCount;

        if (_dateTimeProvider.CurrentDateTimeNow is { Month: 4, Day: 1 })
        {
            return new StatusCodeResult(418);
        }

        if (currentCount % 5 == 0)
        {
            return new CustomStatusCodeResult(503);
        }

        var response = new BrewCoffeeResponseDto
        {
            Message = "Your piping hot coffee is ready",
            Prepared = DateTime.UtcNow,
        };
        return new OkObjectResult(response);
    }
}
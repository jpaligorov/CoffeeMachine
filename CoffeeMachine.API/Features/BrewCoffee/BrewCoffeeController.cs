using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Features.BrewCoffee;

[ApiController]
[Route("brew-coffee")]
public class BrewCoffeeController : ControllerBase
{
    private readonly BrewCoffeeHandler _handler;

    public BrewCoffeeController(BrewCoffeeHandler handler)
    {
        _handler = handler;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var result = _handler.HandleRequest();

        return result;
    }
}
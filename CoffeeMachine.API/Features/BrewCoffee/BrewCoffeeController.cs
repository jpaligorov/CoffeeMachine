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
    public async Task<IActionResult> Get()
    {
        var result = await _handler.HandleRequestAsync();

        return result;
    }
}
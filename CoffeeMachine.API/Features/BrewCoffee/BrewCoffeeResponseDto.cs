namespace CoffeeMachine.Features.BrewCoffee;

public class BrewCoffeeResponseDto
{
    public required string Message { get; set; }
    public DateTime Prepared { get; set; }
}
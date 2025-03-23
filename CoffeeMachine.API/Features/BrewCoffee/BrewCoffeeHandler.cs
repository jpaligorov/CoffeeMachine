using CoffeeMachine.ActionResults;
using CoffeeMachine.Common;
using CoffeeMachine.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.Features.BrewCoffee;

public class BrewCoffeeHandler
{
    private static int _requestCount;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IWeatherService _weatherService;

    public BrewCoffeeHandler(IDateTimeProvider dateTimeProvider, IWeatherService weatherService)
    {
        _dateTimeProvider = dateTimeProvider;
        _weatherService = weatherService;
    }

    public async Task<IActionResult> HandleRequestAsync()
    {
        var result = CheckSpecialCases();
        if (result is not null)
        {
            return result;
        }
        
        var responseMessage = await GetResponseMessageAsync();
        var response = new BrewCoffeeResponseDto
        {
            Message = responseMessage,
            Prepared = DateTime.UtcNow,
        };
        return new OkObjectResult(response);
    }

    private IActionResult? CheckSpecialCases()
    {
        if (_dateTimeProvider.CurrentDateTimeNow is { Month: 4, Day: 1 })
        {
            return new StatusCodeResult(418);
        }

        var currentCount = ++_requestCount;
        return currentCount % 5 == 0 ? new CustomStatusCodeResult(503) : null;
    }

    private async Task<string> GetResponseMessageAsync()
    {
        try
        {
            var temperature = await _weatherService.GetCurrentTemperatureAsync("Melbourne, Australia");

            if (!temperature.HasValue)
            {
                return "Unable to determine the current temperature.";
            }
            return temperature > 30 ? "Your refreshing iced coffee is ready" : "Your piping hot coffee is ready";
        }
        catch (Exception ex)
        {
            return "An error occurred while determining the weather. Serving a default hot coffee.";
        }
    }
}
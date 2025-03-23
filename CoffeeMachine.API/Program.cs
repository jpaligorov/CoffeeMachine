using CoffeeMachine.Common;
using CoffeeMachine.Configuration;
using CoffeeMachine.Features.BrewCoffee;
using CoffeeMachine.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<WeatherApiConfig>(
    builder.Configuration.GetSection("WeatherApi"));


builder.Services.AddOpenApi();
builder.Services.AddTransient<BrewCoffeeHandler>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddScoped<IWeatherService, WeatherService>();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();
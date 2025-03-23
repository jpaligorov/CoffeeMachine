using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachine.ActionResults;

public class CustomStatusCodeResult : IActionResult
{
    public int StatusCode { get;  }

    public CustomStatusCodeResult(int statusCode)
    {
        StatusCode = statusCode;
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;

        response.StatusCode = StatusCode;
        response.ContentLength = 0;
        await response.Body.WriteAsync(Array.Empty<byte>());
    }
}
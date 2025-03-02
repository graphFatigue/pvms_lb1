using System.Text.RegularExpressions;


var builder = WebApplication.CreateBuilder(args);

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors();

// Define the API Endpoint
app.MapPost("/findDecimalNumbers", async (HttpContext context) =>
{
    var request = await context.Request.ReadFromJsonAsync<RequestData>();
    if (request == null || string.IsNullOrWhiteSpace(request.Data))
    {
        return Results.Json(new { message = "Немає даних для обробки", decimalNumbers = new List<double>() });
    }

    // Move function inside the request handler to avoid top-level conflicts
    List<double> ExtractDecimalNumbers(string input)
    {
        var matches = Regex.Matches(input, @"\d+(\.\d+)?");
        return matches.Select(m => double.Parse(m.Value)).ToList();
    }

    var decimalNumbers = ExtractDecimalNumbers(request.Data);

    if (decimalNumbers.Count == 0)
    {
        return Results.Json(new { message = "Десяткових чисел не знайдено", decimalNumbers });
    }

    return Results.Json(new { message = "Знайдені десяткові числа", decimalNumbers });
});

// Run the application
app.Run("http://192.168.0.74:3333");

// Keep only the class outside (no function here)
public class RequestData
{
    public string Data { get; set; } = "";
}

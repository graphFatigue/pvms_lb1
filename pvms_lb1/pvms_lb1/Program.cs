using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors();

app.MapPost("/findDecimalNumbers", async (HttpContext context) =>
{
    var request = await context.Request.ReadFromJsonAsync<RequestData>();
    if (request == null || string.IsNullOrWhiteSpace(request.Data))
    {
        return Results.Json(new { message = "No data provided for processing", decimalNumbers = new List<double>() });
    }

    List<double> ExtractDecimalNumbers(string input)
    {
        var matches = Regex.Matches(input, @"\d+(\.\d+)?");
        return matches.Select(m => double.Parse(m.Value)).ToList();
    }

    var decimalNumbers = ExtractDecimalNumbers(request.Data);

    if (decimalNumbers.Count == 0)
    {
        return Results.Json(new { message = "No decimal numbers found", decimalNumbers });
    }

    return Results.Json(new { message = "Decimal numbers found", decimalNumbers });
});

Console.WriteLine("Server is running on port 3333");

app.Run("http://192.168.0.157:3333");

public class RequestData
{
    public string Data { get; set; } = "";
}

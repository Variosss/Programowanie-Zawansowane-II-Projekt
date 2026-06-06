using Backend.Data;
using Backend.Models;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
    "AllowAll",
    policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<CurrencyDbContext>(
options =>
    options.UseNpgsql(
    builder.Configuration.GetConnectionString(
    "DefaultConnection")));

builder.Services.AddHttpClient<NbpService>();

builder.Services.AddScoped<CurrencyService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db =
        scope.ServiceProvider
            .GetRequiredService<CurrencyDbContext>();

    db.Database.Migrate();
}

app.UseCors("AllowAll");

app.MapGet(
"/api/currencies",
async (
    CurrencyService service,
    int? year,
    int? month,
    int? day) =>
{
    Console.WriteLine($"{year}-{month}-{day}");
    var date = DateOnly.FromDateTime(DateTime.Today);
    if (year.HasValue && month.HasValue && day.HasValue)
    {
        date = new DateOnly(year.Value, month.Value, day.Value);
    }
    
    var data =
        await service.GetCurrenciesAsync(date);

    var query = data.AsQueryable();

    return Results.Ok(query.ToList());
});
app.MapPost(
"/api/currencies/fetch",
async (CurrencyService service) =>
{
    var count =
        await service.RefreshCurrenciesAsync();

    return Results.Ok(
    new FetchCurrenciesResponse
        {
            Success = true,
            FetchedCount = count,
            Message = "Data refreshed from NBP"
        });
});

app.Run();
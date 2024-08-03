
using TourOfHeroesApi.Db;
using TourOfHeroesApi.Models;

namespace TourOfHeroesApi;

public class Program
{
    public static DbHandler DbHandler { get; set; } = new();
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IDbHandler, DbHandler>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", (HttpContext httpContext) =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = summaries[Random.Shared.Next(summaries.Length)]
                })
                .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast")
        .WithOpenApi();

        app.MapGet("/heroes", (HttpContext httpContext, IDbHandler dbHandler) =>
        {
            return dbHandler.GetAllHeroes();
        })
        .WithName("GetHeroes")
        .WithOpenApi();

        app.MapGet("/heroes/{id}", (int id, IDbHandler dbHandler) => dbHandler.GetHeroById(id));

        app.MapPost("/heroes", (Hero hero, IDbHandler dbHandler) =>  { return dbHandler.AddHero(hero); });

        app.Run();
    }
}

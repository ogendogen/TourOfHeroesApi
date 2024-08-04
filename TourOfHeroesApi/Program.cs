
using TourOfHeroesApi.Db;
using TourOfHeroesApi.Models;

namespace TourOfHeroesApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IDbHandler, DbHandler>();

        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins("https://localhost:4200", "http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors(MyAllowSpecificOrigins);

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapGet("/heroes", async (HttpContext httpContext, IDbHandler dbHandler) =>
            await dbHandler.GetAllHeroes())
                .WithName("GetHeroes")
                .WithOpenApi();

        app.MapGet("/heroes/{id}", async (int id, IDbHandler dbHandler) =>
            await dbHandler.GetHeroById(id) is Hero hero ? Results.Ok(hero) : Results.BadRequest(Responses.HeroNotFound(id)))
                .WithName("GetHero")
                .WithOpenApi();

        app.MapPost("/heroes", async (HeroNoId hero, IDbHandler dbHandler) =>
            await dbHandler.AddHero(new Hero() { Id = 0, Name = hero.Name}) is bool result ? Results.Ok(result) : Results.BadRequest(Responses.HeroGenericError))
                .WithName("AddHero")
                .WithOpenApi();

        app.MapPut("/heroes", async (Hero hero, IDbHandler dbHandler) =>
            await dbHandler.UpdateHero(hero))
                .WithName("UpdateHero")
                .WithOpenApi();

        app.MapDelete("/heroes/{id}", async  (int id, IDbHandler dbHandler) =>
            await dbHandler.DeleteHero(id))
                .WithName("DeleteHero")
                .WithOpenApi();

        app.Run();
    }
}

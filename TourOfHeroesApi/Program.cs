
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

        app.MapGet("/heroes", (HttpContext httpContext, IDbHandler dbHandler) => dbHandler.GetAllHeroes())
            .WithName("GetHeroes")
            .WithOpenApi();

        app.MapGet("/heroes/{id}", (int id, IDbHandler dbHandler) => dbHandler.GetHeroById(id))
            .WithName("GetHero")
            .WithOpenApi();

        app.MapPost("/heroes", (HeroNoId hero, IDbHandler dbHandler) => dbHandler.AddHero(new Hero() { Id = 0, Name = hero.Name}))
            .WithName("AddHero")
            .WithOpenApi();

        app.MapPut("/heroes", (Hero hero, IDbHandler dbHandler) => dbHandler.UpdateHero(hero))
            .WithName("UpdateHero")
            .WithOpenApi();

        app.MapDelete("/heroes/{id}", (int id, IDbHandler dbHandler) => dbHandler.DeleteHero(id))
            .WithName("DeleteHero")
            .WithOpenApi();

        app.Run();
    }
}

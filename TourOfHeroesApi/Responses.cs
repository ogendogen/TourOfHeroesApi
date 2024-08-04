using TourOfHeroesApi.Models;

namespace TourOfHeroesApi;

public static class Responses
{
    public static HeroError HeroNotFound(int id) => new HeroError() { Error = $"Hero with id {id} not found." };
    public static HeroError HeroGenericError => new HeroError() { Error = $"Operation failed." };
    public static HeroResult HeroOk => new HeroResult() { Message = $"Operation succedeed." };
}

using System.Data.SQLite;
using TourOfHeroesApi.Models;

namespace TourOfHeroesApi.Db;

public interface IDbHandler
{
    SQLiteConnection Connection { get; }
    Task<Hero> GetHeroById(int id);
    Task<IEnumerable<Hero>> GetAllHeroes();
    Task<bool> AddHero(Hero hero);
    Task<bool> UpdateHero(Hero hero);
    Task<bool> DeleteHero(int id);
}

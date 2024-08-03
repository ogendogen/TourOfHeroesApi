using System.Data.SQLite;
using TourOfHeroesApi.Models;

namespace TourOfHeroesApi.Db;

public class DbHandler : IDbHandler
{
    public SQLiteConnection Connection { get; }

    public DbHandler()
    {
        Connection = new SQLiteConnection("Data Source=heroes.db;Version=3;New=True;Compress=True;");

        try
        {
            Connection.Open();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CreateConnection error: {ex.Message}");
        }

        SQLiteCommand command = Connection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS heroes (id INTEGER NOT NULL PRIMARY KEY, name VARCHAR(64))";
        command.ExecuteNonQuery();
    }

    public async Task<Hero> GetHeroById(int id)
    {
        string query = "SELECT * FROM heroes WHERE id = @Id";
        SQLiteCommand command = new SQLiteCommand(query, Connection);
        command.Parameters.AddWithValue("@Id", id);
        var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Hero
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            };
        }

        return null!;
    }

    public async Task<IEnumerable<Hero>> GetAllHeroes()
    {
        string query = "SELECT * FROM heroes";
        SQLiteCommand command = new SQLiteCommand(query, Connection);
        var reader = await command.ExecuteReaderAsync();
        List<Hero> heroes = new List<Hero>();

        while (await reader.ReadAsync())
        {
            heroes.Add(new Hero
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        return heroes;
    }

    public async Task<bool> AddHero(Hero hero)
    {
        string query = "INSERT INTO heroes (name) VALUES (@name)";
        SQLiteCommand command = new SQLiteCommand(query, Connection);
        command.Parameters.AddWithValue("@name", hero.Name);
        int result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> UpdateHero(Hero hero)
    {
        string query = "UPDATE heroes SET name = @name WHERE id = @id";
        SQLiteCommand command = new SQLiteCommand(query, Connection);
        command.Parameters.AddWithValue("@name", hero.Name);
        command.Parameters.AddWithValue("@id", hero.Id);
        int result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }

    public async Task<bool> DeleteHero(int id)
    {
        string query = "DELETE FROM heroes WHERE id = @id";
        SQLiteCommand command = new SQLiteCommand(query, Connection);
        command.Parameters.AddWithValue("@id", id);
        int result = await command.ExecuteNonQueryAsync();
        return result > 0;
    }
}

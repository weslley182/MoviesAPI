using MoviesAPI.Data;

namespace MoviesAPI.Models;

public class DataSeeder
{
    private readonly AppDbContext _context;
    public DataSeeder(AppDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (_context.Movies.Any())
        {
            return;
        }

        var movies = new List<Movie>()
        {
            new Movie()
            {
                Producers = "Teste",
                Studios = "Teste",
                Title = "Funcionou o Seeder",
                Winner = false,
                Year = 2023
            }
        };

        _context.Movies.AddRange(movies);
        _context.SaveChanges();

    }
}

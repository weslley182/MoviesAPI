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

        var movies = new List<Movie>();

        string filePath = @"Data\movielist.csv";
        using (StreamReader reader = new StreamReader(filePath))
        {
            reader.ReadLine();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                var year = values[0];
                var winner = values[4].ToLower() == "yes" ? true : false;                

                var movie = new Movie()
                {
                    Year = int.Parse(year),
                    Title = values[1],
                    Studios = values[2],
                    Producers = values[3],
                    Winner = winner
                };

                movies.Add(movie);
            }
        }
        
        _context.Movies.AddRange(movies);
        _context.SaveChanges();
    }
}

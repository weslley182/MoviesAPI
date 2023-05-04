using MoviesAPI.Data;
using MoviesAPI.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MoviesAPI.Tests;

public class MovieMockData
{
    public static async Task CreateMovies(MoviesAPIApplication application, bool create, int quantity = 0)
    {
        using (var scope = application.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            using (var context = provider.GetRequiredService<AppDbContext>())
            {
                await context.Database.EnsureCreatedAsync();
                var count = 0;
                if (create)
                {
                    string filePath = @"..\..\..\Data\movielist.csv";
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            count++;
                            if(quantity > 0 && quantity == count)
                            {
                                break;
                            }

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

                            await context.Movies.AddAsync(movie);
                            
                        }
                    }                   

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

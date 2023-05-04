using MoviesAPI.Data;
using MoviesAPI.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MoviesAPI.Tests;

public class MovieMockData
{
    public static async Task CreateMovies(MoviesAPIApplication application, bool create)
    {
        using (var scope = application.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            using (var context = provider.GetRequiredService<AppDbContext>())
            {
                await context.Database.EnsureCreatedAsync();

                if (create)
                {
                    await context.Movies.AddAsync(new Movie { 
                        Id = 1,
                        Producers = "Jhon",
                        Studios = "Warner",
                        Title = "Jhon no arms",
                        Year = 1990,
                        Winner = true
                    });

                    await context.Movies.AddAsync(new Movie {
                        Id = 2,
                        Producers = "Mary",
                        Studios = "Warner",
                        Title = "Mary no arms",
                        Year = 1992,
                        Winner = true
                    });

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

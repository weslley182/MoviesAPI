using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Dto;
using MoviesAPI.Models;
using MoviesAPI.Repository.Interface;
using System.Collections.Generic;

namespace MoviesAPI.Repository
{
    public class MovieRepository: IMovieRepository
    {
        private readonly AppDbContext _context;
        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<Movie>> GetAllAsync()
        {
            return await _context.Movies
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await _context.Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PrizeInterval> GetBiggestPrizeRange()
        {
            var movies = await _context.Movies
                .AsNoTracking()
                .ToListAsync();
            
            var winnerMovies = movies.Where(p => p.Winner == true).ToList();

            var newMovies = GetSplitedProducers(winnerMovies);

            var query = newMovies.GroupBy(x => x.Producers).Where(g => g.Count() > 0).ToList();

            var minInterval = 0;
            
            PrizeInterval prizeInt = null;

            query.ForEach(q =>
            {
                var min = q.Select(p => p.Year).Min();
                var max = q.Select(p => p.Year).Max();
                var interval = max - min;
                if(interval > minInterval)
                {
                    minInterval = interval;
                    prizeInt = new PrizeInterval()
                    {
                        Producer = q.Select(a => a.Producers).FirstOrDefault(),
                        Interval = minInterval,
                        PreviousWin = min,
                        FollowingWin = max
                    };
                }
                
            });


            return prizeInt;
        }

        public async Task<PrizeInterval> GetTwoFastestPrizes()
        {
            var movies = await _context.Movies
                .AsNoTracking()
                .ToListAsync();

            var winnerMovies = movies.Where(p => p.Winner == true).ToList();

            var newMovies = GetSplitedProducers(winnerMovies);

            var query = newMovies.GroupBy(x => x.Producers).Where(g => g.Count() > 0).ToList();

            var maxInterval = 999999;

            PrizeInterval prizeInt = null;

            query.ForEach(q =>
            {
                var min = q.Select(p => p.Year).Min();
                var max = q.Select(p => p.Year).Max();
                var interval = max - min;
                if (interval < maxInterval)
                {
                    maxInterval = interval;
                    prizeInt = new PrizeInterval()
                    {
                        Producer = q.Select(a => a.Producers).FirstOrDefault(),
                        Interval = maxInterval,
                        PreviousWin = min,
                        FollowingWin = max
                    };
                }

            });

            return prizeInt;
        }

        private List<Movie> GetSplitedProducers(List<Movie> winnerMovies)
        {
            var movies = new List<Movie>();

            winnerMovies.ForEach(p =>
            {
                var produc = p.Producers.Split(new string[] { " and ", "," }, StringSplitOptions.None);

                for (var i = 0; i < produc.Length; i++)
                {
                    var mov = new Movie()
                    {
                        Id = p.Id,
                        Producers = produc[i].Trim(),
                        Studios = p.Studios,
                        Title = p.Title,
                        Winner = p.Winner,
                        Year = p.Year
                    };

                    movies.Add(mov);
                }

            });

            return movies;
        }

    }
}

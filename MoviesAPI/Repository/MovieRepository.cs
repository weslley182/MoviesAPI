using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Dto;
using MoviesAPI.Models;
using MoviesAPI.Repository.Interface;

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

        public async Task<PrizeIntervalDto> GetBiggestPrizeRange()
        {
            var winnerMovies = await _context.Movies
                .Where(p => p.Winner == true)
                .AsNoTracking()
                .ToListAsync();

            var newMovies = GetSplitedProducers(winnerMovies);

            var groupProducers = newMovies.GroupBy(x => x.Producers).Where(g => g.Count() > 1).ToList();

            var minInterval = 0;
            
            PrizeIntervalDto prizeInt = null;

            groupProducers.ForEach(q =>
            {
                var min = q.Select(p => p.Year).Min();
                var max = q.Select(p => p.Year).Max();
                var interval = max - min;
                if(interval > minInterval)
                {
                    minInterval = interval;
                    prizeInt = new PrizeIntervalDto()
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

        public async Task<PrizeIntervalDto> GetTwoFastestPrizes()
        {
            var winnerMovies = await _context.Movies
                .Where(p => p.Winner == true)
                .AsNoTracking()
                .ToListAsync();            

            var newMovies = GetSplitedProducers(winnerMovies);

            var groupProducers = newMovies.GroupBy(x => x.Producers).Where(g => g.Count() > 1).ToList();

            var maxInterval = 999999;

            PrizeIntervalDto prizeInt = null;

            groupProducers.ForEach(q =>
            {
                var min = q.Select(p => p.Year).Min();
                var max = q.Select(p => p.Year).Max();
                var interval = max - min;
                if (interval < maxInterval)
                {
                    maxInterval = interval;
                    prizeInt = new PrizeIntervalDto()
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

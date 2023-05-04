using Microsoft.AspNetCore.Mvc;
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
        public async Task Add(Movie movie)
        {
            try
            {
                await _context.Movies.AddAsync(movie);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error on create Movie: " + e.Message);
            }
        }

        public async Task Update(Movie movie)
        {
            try
            {
                _context.Movies.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error on update Movie: " + e.Message);
            }
        }

        public async Task Delete(Movie movie)
        {
            try
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error on delete Movie: " + e.Message);
            }
        }

        public async Task<PrizeIntervalDto> GetBiggestPrizeRange()
        {
            var groupProducers = await GetGroupProducers();

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
            var groupProducers = await GetGroupProducers();

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

        private async Task<List<IGrouping<string, Movie>>> GetGroupProducers()
        {
            var winnerMovies = await _context.Movies
                .Where(p => p.Winner == true)
                .AsNoTracking()
                .ToListAsync();

            var newMovies = GetSplitedProducers(winnerMovies);

            return newMovies
                .GroupBy(x => x.Producers)
                .Where(g => g.Count() > 1)
                .ToList();
        }

    }
}

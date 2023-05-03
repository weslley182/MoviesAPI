using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
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

        public async Task<Movie> GetBiggestPrizeRange()
        {
            var movies = await _context.Movies
                .AsNoTracking()
                .ToListAsync();
            
            var prod = movies.Where(p => p.Winner == true).FirstOrDefault();
            return prod;
        }

        public async Task<Movie> GetTwoFastestPrizes()
        {
            var movies =  await _context.Movies
                .AsNoTracking()
                .ToListAsync();
            var prod = movies.Where(p => p.Winner == true).FirstOrDefault();
            return prod;
        }

    }
}

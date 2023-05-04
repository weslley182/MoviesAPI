using Microsoft.EntityFrameworkCore;
using MoviesAPI.Dto;
using MoviesAPI.Models;

namespace MoviesAPI.Repository.Interface;

public interface IMovieRepository
{
    Task<List<Movie>> GetAllAsync();
    Task<Movie?> GetByIdAsync(int Id);
    Task Add(Movie movie);
    Task Update(Movie movie);
    Task Delete(Movie movie);
    Task<PrizeIntervalDto> GetBiggestPrizeRange();
    Task<PrizeIntervalDto> GetTwoFastestPrizes();
}

﻿using Microsoft.EntityFrameworkCore;
using MoviesAPI.Dto;
using MoviesAPI.Models;

namespace MoviesAPI.Repository.Interface;

public interface IMovieRepository
{
    Task<List<Movie>> GetAllAsync();
    Task<Movie?> GetByIdAsync(int Id);
    Task<PrizeInterval> GetBiggestPrizeRange();
    Task<PrizeInterval> GetTwoFastestPrizes();
}

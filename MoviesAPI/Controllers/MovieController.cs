using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Dto;
using MoviesAPI.Models;
using MoviesAPI.Repository.Interface;

namespace MoviesAPI.Controllers;

[ApiController]
[Route(template:"v1")]
public class MovieController: ControllerBase
{
    private readonly IMovieRepository _repo;
    public MovieController(IMovieRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [Route("Movies")]
    public async Task<ActionResult> GetAll()
    {
        var movies = await _repo.GetAllAsync();
        return Ok(movies);
    }

    [HttpGet]
    [Route("Movies/{id}")]
    public async Task<ActionResult> GetById([FromRoute] int id)
    {
        var movie = await _repo.GetByIdAsync(id);
        return movie == null ? NotFound() : Ok(movie);
    }

    [HttpGet]
    [Route("Movies/BiggestPrizeRangeAndTwoFastestPrizes")]
    public async Task<ActionResult> GetBiggestPrizeRange()
    {
        var max = new List<PrizeIntervalDto>();
        max.Add(await _repo.GetBiggestPrizeRange());

        var min = new List<PrizeIntervalDto>();
        min.Add(await _repo.GetTwoFastestPrizes());

        var prizesList = new PrizesDto
        {
            Max = max,
            Min = min            
        };

        return Ok(prizesList);
    }

}

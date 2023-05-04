using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        return !movies.Any() ? NotFound() : Ok(movies);
    }

    [HttpGet]
    [Route("Movies/{id}")]
    public async Task<ActionResult> GetById([FromRoute] int id)
    {
        var movie = await _repo.GetByIdAsync(id);
        return movie == null ? NotFound() : Ok(movie);
    }


    [HttpPost("Movies")]
    public async Task<ActionResult> PostAsync([FromBody] MovieDto movieDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var movie = new Movie()
        {
            Producers = movieDto.Producers,
            Studios = movieDto.Studios,
            Title = movieDto.Title,
            Winner = movieDto.Winner,
            Year = movieDto.Year
        };


        await _repo.Add(movie);
        return Ok();
    }

    [HttpPut("Movies/{id}")]
    public async Task<ActionResult> PutAsync([FromBody] MovieDto movieDto, [FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        var movie = await _repo.GetByIdAsync(id);
        
        if(movie == null)
        {
            return NotFound(); 
        }
        
        movie.Producers = movieDto.Producers;
        movie.Studios = movieDto.Studios;
        movie.Title = movieDto.Title;
        movie.Winner = movieDto.Winner;
        movie.Year = movieDto.Year;

        await _repo.Update(movie);
        
        return Ok();
    }

    [HttpDelete("Movies/{id}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        var movie = await _repo.GetByIdAsync(id);

        if (movie == null)
        {
            return NotFound();
        }

        await _repo.Delete(movie);
        return Ok();
    }

    [HttpGet]
    [Route("Movies/BiggestPrizeRangeAndTwoFastestPrizes")]
    public async Task<ActionResult> GetBiggestPrizeRange()
    {
        var max = new List<PrizeIntervalDto>
        {
            await _repo.GetBiggestPrizeRange()
        };

        var min = new List<PrizeIntervalDto>
        {
            await _repo.GetTwoFastestPrizes()
        };

        var prizesList = new PrizesDto
        {
            Max = max,
            Min = min            
        };

        return Ok(prizesList);
    }

}

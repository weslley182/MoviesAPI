using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MoviesAPI.Dto;
using MoviesAPI.Models;
using MoviesAPI.Repository.Interface;

namespace MoviesAPI.Controllers;

[ApiController]
[Route(template:"v1/Movies")]
public class MovieController: ControllerBase
{
    private readonly IMovieRepository _repo;
    public MovieController(IMovieRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]    
    public async Task<ActionResult> GetAll()
    {
        var movies = await _repo.GetAllAsync();
        return !movies.Any() ? NotFound() : Ok(movies);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult> GetById([FromRoute] int id)
    {
        var movie = await _repo.GetByIdAsync(id);
        return movie == null ? NotFound() : Ok(movie);
    }


    [HttpPost]
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

    [HttpPut("{id}")]
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

    [HttpDelete("{id}")]
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

}

using Microsoft.AspNetCore.Mvc;
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
        var movies = _repo.GetAllAsync();
        return Ok(movies);
    }

    [HttpGet]
    [Route("Movies/{id}")]
    public async Task<ActionResult> GetById([FromRoute] int id)
    {
        var movie = _repo.GetByIdAsync(id);
        return movie == null ? NotFound() : Ok(movie);
    }

    [HttpGet]
    [Route("Movies/all")]
    public async Task<ActionResult> GetAsk()
    {
        var movies = _repo.GetAllAsync();
        return Ok(movies);
    }

}

using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Dto;
using MoviesAPI.Repository.Interface;

namespace MoviesAPI.Controllers;

[ApiController]
[Route(template: "v1/Prizes")]
public class PrizesController : ControllerBase
{
    private readonly IMovieRepository _repo;
    public PrizesController(IMovieRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [Route("BiggestPrizeRangeAndTwoFastestPrizes")]
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

using System.Net.Http.Json;
using System.Net;
using MoviesAPI.Models;
using NUnit.Framework;
using MoviesAPI.Dto;

namespace MoviesAPI.Tests;

public class GetMoviesTest
{
    [Test]
    public async Task GET_Return_All_Movies()
    {
        var expectedQuantity = 50;
        var jumpLine = 1;
        await using var application = new MoviesAPIApplication();

        await MovieMockData.CreateMovies(application, true, (expectedQuantity + jumpLine));
        var url = "/v1/Movies";

        var client = application.CreateClient();

        var result = await client.GetAsync(url);
        var movies = await client.GetFromJsonAsync<List<Movie>>("/v1/Movies");

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(movies);
        Assert.AreEqual(expectedQuantity, movies.Count);
    }

    [Test]
    public async Task GET_Return_Movies_Null()
    {
        await using var application = new MoviesAPIApplication();

        await MovieMockData.CreateMovies(application, false);

        var client = application.CreateClient();
        var movies = await client.GetFromJsonAsync<List<Movie>>("/v1/Movies");        
        
        Assert.IsNotNull(movies);
        Assert.IsTrue(movies.Count == 0);
    }

    [Test]
    public async Task GET_Return_GetBiggestPrizeRange_and_GetTwoFastestPrizes()
    {
        await using var application = new MoviesAPIApplication();

        await MovieMockData.CreateMovies(application, true);
        var url = "/v1/Movies/BiggestPrizeRangeAndTwoFastestPrizes";

        var client = application.CreateClient();

        var result = await client.GetAsync(url);
        var prizes = await client.GetFromJsonAsync<PrizesDto>("/v1/Movies/BiggestPrizeRangeAndTwoFastestPrizes");

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(prizes);
        Assert.AreEqual(prizes.Min[0].Producer, "Joel Silver");
        Assert.AreEqual(prizes.Min[0].PreviousWin, 1990);
        Assert.AreEqual(prizes.Min[0].FollowingWin, 1991);
        Assert.AreEqual(prizes.Min[0].Interval, 1);

        Assert.AreEqual(prizes.Max[0].Producer, "Matthew Vaughn");
        Assert.AreEqual(prizes.Max[0].PreviousWin, 2002);
        Assert.AreEqual(prizes.Max[0].FollowingWin, 2015);
        Assert.AreEqual(prizes.Max[0].Interval, 13);

    }
}

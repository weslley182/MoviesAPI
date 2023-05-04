using MoviesAPI.Dto;
using MoviesAPI.Models;
using System.Net.Http.Json;
using System.Net;
using NUnit.Framework;


namespace MoviesAPI.Tests;

public class GetMoviesTest
{
    private MoviesAPIApplication _application;

    [SetUp]
    public void SetUp()
    {
        _application = new MoviesAPIApplication();
    }

    [Test]
    public async Task GET_Return_All_Movies()
    {
        var expectedQuantity = 50;
        var columnNamesQtd = 1;        

        await MovieMockData.CreateMovies(_application, true, (expectedQuantity + columnNamesQtd));
        var url = "/v1/Movies";

        var client = _application.CreateClient();

        var result = await client.GetAsync(url);
        var movies = await client.GetFromJsonAsync<List<Movie>>(url);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(movies);
        Assert.AreEqual(expectedQuantity, movies.Count);
    }

    [Test]
    public async Task GET_Return_Movies_Null()
    {
        await MovieMockData.CreateMovies(_application, false);
        var url = "/v1/Movies";
        
        var client = _application.CreateClient();

        var result = await client.GetAsync(url);
        
        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);        
    }

    [Test]
    public async Task GET_Return_GetBiggestPrizeRange_and_GetTwoFastestPrizes()
    {     
        await MovieMockData.CreateMovies(_application, true);
        var url = "/v1/Movies/BiggestPrizeRangeAndTwoFastestPrizes";

        var client = _application.CreateClient();

        var result = await client.GetAsync(url);
        var prizes = await client.GetFromJsonAsync<PrizesDto>(url);

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

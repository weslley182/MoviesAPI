using System.Net.Http.Json;
using System.Net;
using MoviesAPI.Models;
using NUnit.Framework;

namespace MoviesAPI.Tests;

public class GetMoviesTest
{
    [Test]
    public async Task GET_Return_All_Movies()
    {
        await using var application = new MoviesAPIApplication();

        await MovieMockData.CreateMovies(application, true);
        var url = "/v1/Movies";

        var client = application.CreateClient();

        var result = await client.GetAsync(url);
        var movies = await client.GetFromJsonAsync<List<Movie>>("/v1/Movies");

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        Assert.IsNotNull(movies);
        Assert.IsTrue(movies.Count == 2);
    }
}

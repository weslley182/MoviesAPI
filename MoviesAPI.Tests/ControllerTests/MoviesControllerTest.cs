using Microsoft.Extensions.Configuration;
using MoviesAPI.Dto;
using MoviesAPI.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace MoviesAPI.Tests.ControllerTests;

public class MoviesControllerTest
{
    private MoviesAPIApplication _application;
    private MovieMockData _mock;
    private IConfiguration _configuration;

    [SetUp]
    public void SetUp()
    {

        _application = new MoviesAPIApplication();

        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, false)
            .Build();

        _mock = new MovieMockData(_configuration);
    }

    [Test]
    public async Task GET_Return_All_Movies()
    {
        var expectedQuantity = 50;
        var columnNamesQtd = 1;

        await _mock.CreateMovies(_application, true, expectedQuantity + columnNamesQtd);
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
        await _mock.CreateMovies(_application, false);
        var url = "/v1/Movies";

        var client = _application.CreateClient();

        var result = await client.GetAsync(url);

        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Test]
    public async Task POST_Must_Create_Movie()
    {
        await _mock.CreateMovies(_application, false);
        var url = "/v1/Movies";

        var client = _application.CreateClient();
        var newMovie = new MovieDto()
        {
            Producers = "Jhon",
            Title = "Jhon no arms",
            Year = 1984,
            Winner = true
        };

        var result = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(newMovie), Encoding.UTF8, "application/json"));

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
    }

    [Test]
    public async Task POST_Must_Send_Error_On_Create_Movie()
    {
        await _mock.CreateMovies(_application, false);
        var url = "/v1/Movies";

        var client = _application.CreateClient();
        var newMovie = new MovieDto()
        {
            Title = "Jhon no arms",
            Year = 1984,
            Winner = true
        };

        var result = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(newMovie), Encoding.UTF8, "application/json"));

        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Test]
    public async Task PUT_Must_Change_Movie()
    {
        await _mock.CreateMovies(_application, true, 10);
        var url = "/v1/Movies/5";

        var client = _application.CreateClient();
        var changeMovie = new MovieDto()
        {
            Producers = "Jhon",
            Title = "Jhon no arms",
            Studios = "Warner",
            Year = 1984,
            Winner = true
        };

        var result = await client.PutAsync(url, new StringContent(JsonConvert.SerializeObject(changeMovie), Encoding.UTF8, "application/json"));
        var movie = await client.GetFromJsonAsync<Movie>(url);

        Assert.AreEqual(movie.Id, 5);
        Assert.AreEqual(movie.Studios, changeMovie.Studios);
        Assert.AreEqual(movie.Producers, changeMovie.Producers);
        Assert.AreEqual(movie.Title, changeMovie.Title);
        Assert.AreEqual(movie.Year, changeMovie.Year);
        Assert.AreEqual(movie.Winner, changeMovie.Winner);
    }

    [Test]
    public async Task PUT_Must_Send_Error()
    {
        await _mock.CreateMovies(_application, true, 10);
        var url = "/v1/Movies/5";

        var client = _application.CreateClient();
        var changeMovie = new MovieDto()
        {
            Studios = "Warner",
            Year = 1984,
            Winner = true
        };

        var result = await client.PutAsync(url, new StringContent(JsonConvert.SerializeObject(changeMovie), Encoding.UTF8, "application/json"));

        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);

    }

    [Test]
    public async Task Delete_Must_Remove_Movie()
    {
        await _mock.CreateMovies(_application, true, 10);
        var url = "/v1/Movies/5";

        var client = _application.CreateClient();

        var result = await client.DeleteAsync(url);

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

    }

    [Test]
    public async Task Delete_Must_Send_NotFound()
    {
        await _mock.CreateMovies(_application, true, 10);
        var url = "/v1/Movies/15";

        var client = _application.CreateClient();

        var result = await client.DeleteAsync(url);

        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

    }
}

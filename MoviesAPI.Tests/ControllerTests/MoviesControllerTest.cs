using MoviesAPI.Models;
using System.Net.Http.Json;
using System.Net;
using NUnit.Framework;
using MoviesAPI.Dto;
using System.Text;
using Newtonsoft.Json;

namespace MoviesAPI.Tests.ControllerTests;

public class MoviesControllerTest
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

        await MovieMockData.CreateMovies(_application, true, expectedQuantity + columnNamesQtd);
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
    public async Task POST_Must_Create_Movie()
    {
        await MovieMockData.CreateMovies(_application, false);
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
        await MovieMockData.CreateMovies(_application, false);
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
        await MovieMockData.CreateMovies(_application, true, 10);
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
        await MovieMockData.CreateMovies(_application, true, 10);
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
        await MovieMockData.CreateMovies(_application, true, 10);
        var url = "/v1/Movies/5";

        var client = _application.CreateClient();        

        var result = await client.DeleteAsync(url);        

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);        

    }

    [Test]
    public async Task Delete_Must_Send_NotFound()
    {
        await MovieMockData.CreateMovies(_application, true, 10);
        var url = "/v1/Movies/15";

        var client = _application.CreateClient();

        var result = await client.DeleteAsync(url);

        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

    }
}

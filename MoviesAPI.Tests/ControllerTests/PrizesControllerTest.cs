using Microsoft.Extensions.Configuration;
using MoviesAPI.Dto;
using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;

namespace MoviesAPI.Tests.ControllerTests
{
    public class PrizesControllerTest
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
        public async Task GET_Return_GetBiggestPrizeRange_and_GetTwoFastestPrizes()
        {
            await _mock.CreateMovies(_application, true);
            var url = "/v1/Prizes/BiggestPrizeRangeAndTwoFastestPrizes";

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
}

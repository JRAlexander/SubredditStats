using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace SubredditStats.Common.Services.Tests
{
    [TestClass()]
    public class SubredditStatsApiTests
    {
        [TestMethod()]
        public async Task GetRankedPostsTest()
        {
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            using var response = await client.GetAsync("/getrankedposts");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod()]
        public async Task GetRankedSubredditUsersTest()
        {
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            using var response = await client.GetAsync("/getrankedsubredditusers");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod()]
        public async Task GetSwaggerTest()
        {
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            using var response = await client.GetAsync("/swagger");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod()]
        public async Task Get_Root_Status_Resp()
        {
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            using var response = await client.GetAsync("/");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Subreddit r/all Monitoring Status: All systems go!", await response.Content.ReadAsStringAsync());
        }
    }
}
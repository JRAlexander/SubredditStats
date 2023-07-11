using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SubredditStats.Common.Configuration;
using SubredditStats.Common.Models;
using SubredditStats.Common.Services;
using SubredditStats.Common.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

// create DBContext to report subreddit stats
builder.Services.AddDbContext<RankedPostDb>(options => options.UseInMemoryDatabase("items"));
builder.Services.AddDbContext<RankedSubredditUserDb>(options => options.UseInMemoryDatabase("items"));

//setup swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RedditStats API", Description = "The SubRedditStats API monitors your selected SubReddit in near real-time.</p> <div>The home page lists the Reddit API Status</div><div>/getrankedposts will return the created posts ranked by votes (score) during the processing window.</div><div>/getrankedsubredditusers will return subreddit users ranked by the number of posts they have submitted during the processing window.</div> ", Version = "v1" });
});

//configure reddit api wrapper client

var section = builder.Configuration.GetSection("RedditAPIConfig");
var redditAPIConfig = section.Get<RedditAPIConfig>();

RedditWrapper.ConfigureRedditProcessing(redditAPIConfig.AppId, redditAPIConfig.RefreshToken, redditAPIConfig.AccessToken, redditAPIConfig.Subreddit);
RedditWrapper.ConfigureRankedPostDb();
RedditWrapper.ConfigureRankedSubredditUserDb();
RedditWrapper.StartMonitoring();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RedditStats API V1");
});
app.MapGet("/getrankedposts", RankedPostsApi.GetRankedPosts).Produces<RankedPost>();

app.MapGet("/getrankedsubredditusers", RankedSubredditUsersApi.GetRankedSubredditUsers).Produces<RankedSubredditUser>();

app.MapGet("/", () => $"Subreddit r/{redditAPIConfig.Subreddit} Monitoring Status: " + RedditStatus.StatusMessage);

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }
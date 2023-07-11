using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SubredditStats.Common.Models;

namespace SubredditStats.Common.Services
{
    public static class RankedPostsApi
    {
        public static async Task<IResult> GetRankedPosts(RankedPostDb rankedPostDb)
        {
            return TypedResults.Ok(await rankedPostDb.RankedPosts.Select(p => new { p.Score, p.Title, p.Author, Permalink = "https://reddit.com" + p.Permalink }).OrderByDescending(p => p.Score).ToListAsync());
        }
    }
}

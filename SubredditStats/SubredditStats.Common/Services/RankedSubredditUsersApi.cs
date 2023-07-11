using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SubredditStats.Common.Models;

namespace SubredditStats.Common.Services
{
    public static class RankedSubredditUsersApi
    {
        public static async Task<IResult> GetRankedSubredditUsers(RankedSubredditUserDb rankedSubredditUserDb)
        {
            return TypedResults.Ok(await rankedSubredditUserDb.RankedSubredditUsers.Select(u => new { u.Name, u.totalPosts }).OrderByDescending(u => u.totalPosts).ToListAsync());
        }
    }
}


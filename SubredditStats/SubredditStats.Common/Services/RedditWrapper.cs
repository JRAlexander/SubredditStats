using Microsoft.EntityFrameworkCore;
using Reddit;
using Reddit.Controllers;
using Reddit.Controllers.EventArgs;
using SubredditStats.Common.Models;
using SubredditStats.Common.Utils;

namespace SubredditStats.Common.Services
{

    public static class RedditWrapper
    {
        private static RankedPostDb _rankedPostDb;
        private static RankedSubredditUserDb _rankedSubredditUserDb;
        private static RedditClient _reddit;
        private static Subreddit _sub;

        public static void ConfigureRankedSubredditUserDb()
        {
            var rankedSubredditUserDbOptions = new DbContextOptionsBuilder<RankedSubredditUserDb>()
                .UseInMemoryDatabase("items")
                .Options;

            _rankedSubredditUserDb = new RankedSubredditUserDb(rankedSubredditUserDbOptions);
        }

        public static void ConfigureRankedPostDb()
        {
            var rankedPostDbOptions = new DbContextOptionsBuilder<RankedPostDb>()
                .UseInMemoryDatabase("items")
                .Options;

            _rankedPostDb = new RankedPostDb(rankedPostDbOptions);
        }

        // Configures and creates redditclient, sets subreddit
        public static void ConfigureRedditProcessing(string appId, string refreshToken, string accessToken, string subReddit)
        {
            //validate required parameters
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentNullException(nameof(appId));
            }

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }

            if (string.IsNullOrWhiteSpace(subReddit))
            {
                throw new ArgumentNullException(nameof(subReddit));
            }

            //create reddit client 
            try
            {
                _reddit = new RedditClient(appId: appId, refreshToken: refreshToken, accessToken: accessToken);
            }
            catch (Exception ex)
            {
                RedditStatus.TranslateRedditException(ex);
            }
            _sub = _reddit.Subreddit(subReddit);
        }

        public static void StartMonitoring()
        {
            try
            {
                _sub.Posts.GetNew();  // Before monitoring, let's grab the posts once so we have a point of comparison when identifying new posts that come in.
                _sub.Posts.MonitorNew(500); // half second between monitoring
                _sub.Posts.NewUpdated += C_NewPostsUpdated; //add event handler
            }
            catch (Exception ex)
            {
                RedditStatus.TranslateRedditException(ex);
            }
        }

        public static void StopMonitoring()
        {
            try
            {
                _sub.Posts.MonitorNew();  // Toggle off.
                _sub.Posts.NewUpdated -= C_NewPostsUpdated;
            }
            catch (Exception ex)
            {
                RedditStatus.TranslateRedditException(ex);
            }
        }

        //event handler for new posts in subreddit
        public static void C_NewPostsUpdated(object sender, PostsUpdateEventArgs e)
        {
            foreach (Post post in e.Added)
            {
                // add each post to db - with only relevant fields
                _rankedPostDb.Add(new RankedPost { PostId = post.Id, Created = post.Created, Author = post.Author, Title = post.Title, Score = post.Score, Permalink = post.Permalink });
                _rankedPostDb.SaveChanges();

                // pull the user from the post and check if they've already posted during processing window
                // if so, increment total posts and update db
                // otherwise add to db
                var updateUser = _rankedSubredditUserDb.RankedSubredditUsers.FirstOrDefault(u => u.Name == post.Author);
                if (updateUser != null)
                {
                    updateUser.totalPosts += 1;
                    _rankedSubredditUserDb.SaveChanges();
                }
                else
                {
                    _rankedSubredditUserDb.Add(new RankedSubredditUser { Name = post.Author, totalPosts = 1 });
                    _rankedSubredditUserDb.SaveChanges();
                }
            }
        }
    }
}

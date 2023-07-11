namespace SubredditStats.Common.Configuration
{
    public class RedditAPIConfig
    {

        public required string AppId { get; set; }
        public required string RefreshToken { get; set; }
        public required string AccessToken { get; set; }
        public required string Subreddit { get; set; }

    }
}

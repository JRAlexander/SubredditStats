namespace SubredditStats.Common.Utils;

public static class RedditStatus
{
    private static string _message = "All systems go!";

    public static string StatusMessage
    {
        get { return _message; }
        set { _message = value; }
    }
    // Translates Reddit.NET exceptions thrown from Reddit API into friendly messages
    public static void TranslateRedditException(Exception ex)
    {
        switch (ex.GetType().Name)
        {
            case "RedditUnauthorizedException":
                _message = "There is an authorization issue. Please check your credentials and restart.";
                break;
            case "RedditServiceUnavailableException":
                _message = "The Reddit Service is currently unavailable: Please try again later.";
                break;
            case "RedditGatewayTimeoutException":
                _message = "The Reddit Service gateway is currently timing out: Please try again later.";
                break;
            case "RedditInternalServerErrorException":
                _message = "The Reddit Service has an Internal Server issue: Please try again later.";
                break;
            case "RedditException":
                _message = "The Reddit Service has an issue: Please try again later.";
                break;
            default:
                _message = "All systems go!";
                break;
        }
    }
}
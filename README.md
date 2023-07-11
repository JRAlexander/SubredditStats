# SubredditStats
A C# Minimal API Reddit Stat Processor. This project shows how to authenicate to the Reddit API, monitor a given subreddit for new posts, process that information before making it available via API endpoints.

## Prerequisites 
1. Reddit Account and app - [developer app page](https://www.reddit.com/prefs/apps)
 * Sign up or Login
 * Click on **Create App**
 * Enter a name
 * Select **Installed App**
 * **about url** is not needed - leave blank
 * In **redirect url** use http://127.0.0.1:8080/Reddit.NET/oauthRedirect - this is needed for the Reddit.NET library.
 * Click **Create App**
 * Next, your app page will show an **AppID** underneath your app name and type (installed app) at the top.
 * Copy the AppId and paste in your favorite text editor.
 * leave your app page open
 
2. Install [.NET SDK 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
1. Clone this repository or download and unzip the files
## Registering the app and Authenticating
1. Open **SubRedditStats.sln** in Visual Studio
2. Once Solution has loaded, right click on the **AuthTokenRetriever** Project in the Solution Explorer and select **Set as Startup Project**
   * This is a Console Application that will easily allow you to obtain your Auth tokens.
3. Run the **AuthTokenRetriever** Project.
4. Paste your AppId from your text editor into the app and press Enter.
5. Press Enter again to leave the App Secret blank. Its not used for the app type you created.
6. You will see a warning. ** IMPORTANT:  Before you proceed any further, make sure you are logged into Reddit as the user you wish to authenticate! **
   In the next step, a browser window will open and you'll be taken to Reddit's app authentication page.  Press any key to continue....
7. After you hit the any key, :) a page should open with something like "MyStatApp would like to connect with your reddit account.". Scroll down to the orange  **Allow** button and click to register your Reddit app and obtain your developer credentials.
8. The page should display "Token retrieval completed successfully!" with your Access token and your Refresh tokens
9. save both tokens to your text editor. You can close the page and exit the console app.
### Troubleshooting
* if you get an exception on line 14, the BROWSER_PATH is set to "C:\Program Files\Google\Chrome\Application\chrome.exe" currently, you may need to change if you arent using Chrome.
* If you press the key and you see something like "You broke Reddit", check the following and restart the app 
 * make sure your AppID is correct
 * make sure you're logged into reddit
 * reverify your reddit app **redirect url** ise http://127.0.0.1:8080/Reddit.NET/oauthRedirect
- these will take care of the majority of issues.
10. Next, right click on **SubredditStats.API** project and select **Set as Startup Project**.
11. In the **SubredditStats.API** project, double-click the **appsettings.json** file
12. Fill in the AppId, RefreshToken, and AccessToken entries (between the quotes) with the corresponding information you just put in your text editor.
13. Theres also an entry for Subreddit with a default of "all" -- that's where you can change the name from the default of the all subreddit to something else. I'd recommend starting with the default for the best experience.
14. Save the **appsettings.json** file. You're now ready to run the redditstats api!
## Running the App
Click on run to start the **SubredditStats.API**. A browser window will open.

### API GUIDE
| Name  | Route  | Description  |  
|---|---|---|
| Root  | /  | Provides status for Reddit API Communication. | 
| Swagger  | /swagger | Provides API testing and documentation. |
| GetRankedPosts  | /getrankedposts  | returns a ranked list of subreddit posts submitted during the processing window. Ranked by score (number of upvotes).  |   
| GetRankedSubredditUsers  | /getrankedsubredditusers  | returns a ranked list of subreddit users who submitted posts during the processing window. Ranked by total number of posts.  | 
 
## Solution Outline
### Overview
The SubRedditStats Solution is an ASP.NET Core Minimal Api using Entity Framework Core In Memory datastores for processing and reporting. Exception handling has been implemented to handle both generic .Net exceptions and specific Reddit based exceptions as well. Unit tests have been created to test each api route. The [Reddit.Net](https://github.com/sirkris/Reddit.NET) library is used for the communication with the Reddit API. It is comprehensive and is probably the widest used Reddit API library for .Net. 
### Structure
SubRedditStats is comprised of the following projects:
* AuthTokenRetriever - One-time Console app used for Reddit token retrieval needed to communicate with the Reddit API.
* SubRedditStats.API - Asp.Net minimal Api
* SubredditStats.Common - Common infrastructure (Configuration, Models, Services, Utils)
* SubredditStats.CommonTests - Test Project   


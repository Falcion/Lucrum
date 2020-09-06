using System;
using System.Collections;
using System.Collections.Generic;

using Octokit;

public class API {

    public async void Information(List<string> arguments) {

        string ApiToken = Storage.ApiToken;

        GitHubClient Client = new GitHubClient(new ProductHeaderValue("Lucrum"));

        Credentials TokenAuth = new Credentials(ApiToken);
        Client.Credentials = TokenAuth;

        MiscellaneousRateLimit miscellaneousRate = await Client.Miscellaneous.GetRateLimits();

        RateLimit coreRequests = miscellaneousRate.Resources.Core;
        RateLimit searchRequests = miscellaneousRate.Resources.Search;

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("GitHub API's requests information.");
        Console.WriteLine("Core requests information:");

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Core requests per Hour: " + coreRequests.Limit);
        Console.WriteLine("Core requests remaining: " + coreRequests.Remaining);
        Console.WriteLine("Core requests Reset: " + $"{coreRequests.Reset.ToLocalTime()}");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Search requests information:");

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Search requests per Minute: " + searchRequests.Limit);
        Console.WriteLine("Search requests remaining: " + searchRequests.Remaining);
        Console.WriteLine("Search requests Reset: " + $"{searchRequests.Reset.ToLocalTime()}");
    }
}
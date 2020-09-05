using System;
using System.Collections;
using System.Collections.Generic;

using Octokit;

public class Git {

    public async void Information(string[] arguments) {

        string ApiToken = Storage.ApiToken;

        GitHubClient Client = new GitHubClient(new ProductHeaderValue("Lucrum"));

        Credentials TokenAuth = new Credentials(ApiToken);

        Client.Credentials = TokenAuth;

        MiscellaneousRateLimit miscellaneousRate
                = await Client.Miscellaneous.GetRateLimits();

        RateLimit coreRate = miscellaneousRate.Resources.Core;
        RateLimit searchRate = miscellaneousRate.Resources.Search;

        Console.Write("\n" + "Core requests per Hour: " + $"{coreRate.Limit}");
        Console.Write("\n" + "Core requests remaining: " + $"{coreRate.Remaining}");
        Console.Write("\n" + "Core's limit Reset: " + $"{coreRate.Reset.DateTime}");
        Console.WriteLine();
        Console.Write("\n" + "Search requests per Minute: " + $"{searchRate.Limit}");
        Console.Write("\n" + "Search requests remaining: " + $"{searchRate.Remaining}");
        Console.Write("\n" + "Search's limit Reset: " + $"{searchRate.Reset.DateTime}");
        Console.WriteLine("\n");
    }
}
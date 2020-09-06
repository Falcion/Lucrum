using System;
using System.Collections;
using System.Collections.Generic;

using Octokit;

public class ReleaseContext {

    public async void RepositoryReleases(List<string> arguments) {

        string ApiToken = Storage.ApiToken;

        GitHubClient Client = new GitHubClient(new ProductHeaderValue("Lucrum"));

        Credentials TokenAuth = new Credentials(ApiToken);
        Client.Credentials = TokenAuth;

        string reposOwner = "", reposName = "";

        int viewCount = 10;

        if(arguments[0] == "repos-releases") arguments.Remove("repos-releases");

        if(arguments.Count >= 3) {
            
            reposOwner = arguments[0];
            reposName = arguments[1];
            
            try {
                viewCount = int.Parse(arguments[2]);
            }
            catch(Exception) {
                new Context().Error("Critical command syntax error! Please, check your command and it's arguments!");
            }

            arguments.Clear();
        }

        if(reposOwner == "" || reposName == "") {

            new Context().Error("Command syntax error! Please, check your command and it's arguments!");
        }

        IReadOnlyList<Release> releases = await Client.Repository.Release.GetAll(reposOwner, reposName);

        Repository repos = await Client.Repository.Get(reposOwner, reposName);

        if(viewCount >= releases.Count) viewCount = releases.Count;

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(repos.Name + "'s releases");

        for(int i = 0; i < viewCount; i++) {

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(releases[i].Name);
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Tag's Name: " + releases[i].TagName);
            Console.WriteLine("Creation Date: " + releases[i].CreatedAt);
            Console.WriteLine("Release's URL: " + releases[i].Url);
        }
    }

    public async void ReleaseInformation(List<string> arguments) {

        string ApiToken = Storage.ApiToken;

        GitHubClient Client = new GitHubClient(new ProductHeaderValue("Lucrum"));

        Credentials TokenAuth = new Credentials(ApiToken);
        Client.Credentials = TokenAuth;

        if (arguments[0] == "release-info") arguments.Remove("release-info");

        string reposOwner = "", reposName = "", tagName = "";

        if(arguments.Count >= 3) {
            
            reposOwner = arguments[0];
            reposName = arguments[1];
            tagName = arguments[2];
        }

        if(reposOwner == "" || reposName == "" || tagName == "") {

            new Context().Error("Command syntax error! Please, check your command and it's arguments!");
        }

        Release release = await Client.Repository.Release.Get(reposOwner, reposName, tagName);

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Release's information");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Release's Name: " + release.Name);
        Console.WriteLine("Upload URL: " + release.UploadUrl);
        Console.WriteLine("Target Commitish: " + release.TargetCommitish);
        Console.WriteLine("Draft: " + $"{release.Draft}");
    }
}
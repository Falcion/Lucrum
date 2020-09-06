using System;
using System.Collections;
using System.Collections.Generic;

using Octokit;

public class BranchContext {

    public async void RepositoryBranches(List<string> arguments) {

        string ApiToken = Storage.ApiToken;

        GitHubClient Client = new GitHubClient(new ProductHeaderValue("Lucrum"));

        Credentials TokenAuth = new Credentials(ApiToken);
        Client.Credentials = TokenAuth;

        string reposOwner = "", reposName = "";

        if(arguments[0] == "repos-branches") arguments.Remove("repos-branches");

        if(arguments.Count >= 2) {
            
            reposOwner = arguments[0];
            reposName = arguments[1];

            arguments.Clear();
        }

        if(reposOwner == "" || reposName == "") {

            new Context().Error("Command syntax error! Please, check your command and it's arguments!");
        }

        IReadOnlyList<Branch> branches = await Client.Repository.Branch.GetAll(reposOwner, reposName);

        Repository repos = await Client.Repository.Get(reposOwner, reposName);

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(repos.Name + "'s branches");

        for(int i = 0; i < branches.Count; i++) {

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Branch: " + branches[i].Name);
        }
    }

    public async void BranchInformation(List<string> arguments) {

        string ApiToken = Storage.ApiToken;

        GitHubClient Client = new GitHubClient(new ProductHeaderValue("Lucrum"));

        Credentials TokenAuth = new Credentials(ApiToken);
        Client.Credentials = TokenAuth;

        string reposOwner = "", reposName = "", branchName = "";

        if(arguments[0] == "branch-info") arguments.Remove("branch-info");

        if(arguments.Count >= 3) {
            
            reposOwner = arguments[0];
            reposName = arguments[1];
            branchName = arguments[2];

            arguments.Clear();
        }

        if(reposOwner == "" || reposName == "" || branchName == "") {

            new Context().Error("Command syntax error! Please, check your command and it's arguments!");
        }

        Branch branch = await Client.Repository.Branch.Get(reposOwner, reposName, branchName);

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Branch's information");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Branch's Name: " + branch.Name);
        Console.WriteLine("Protected: " + $"{branch.Protected}");
        Console.WriteLine("Last SHA: " + $"{branch.Commit.Sha}");
    }
}
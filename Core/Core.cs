using System;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

public class Core {

    internal void CommandHandler() {

        string command = "";

        while(command != "shutdown") {

            Thread.Sleep(2000);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter command: ");

            command = Console.ReadLine();

            new Logger().Logging("User typed new command: " + command + '.');

            List<string> arguments = command.Split(' ').ToList();

            switch(arguments[0]) {

                case "gitapi-limit":
                    new API().Information(arguments);
                    break;

                case "repos-branches":
                    new BranchContext().RepositoryBranches(arguments);
                    break;

                case "branch-info":
                    new BranchContext().BranchInformation(arguments);
                    break;

                case "repos-releases":
                    new ReleaseContext().RepositoryReleases(arguments);
                    break;

                case "release-info":
                    new ReleaseContext().ReleaseInformation(arguments);
                    break;

                case "shutdown":
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong command! Please, check your input and write again!");
                    break;
            }
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Console has ended it's work. Press any button to close.");
        Console.WriteLine("For work-history, open app.log file and see system's logs");

        Console.ForegroundColor = ConsoleColor.Black;

        new Logger().Logging("Console has ended it's work.");

        Environment.Exit(0);
    }
}
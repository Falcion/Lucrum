using System;

public class Core {

    private string prefix = Storage.Prefix;

    internal void CommandHandler() {

        string command = "";

        while(command != prefix + "shutdown") {

            command = Console.ReadLine();

            if(!command.StartsWith(prefix)) return;

            new Logger().Logging("User typed new command: " + command + '.');

            int prefixLength = prefix.Length;

            command = command.Remove(0, prefixLength);

            string[] args = command.Split(' ');

            switch(args[0]) {

                case "shutdown":
                    command = prefix + "shutdown";
                    break;

            }
        }

        Console.WriteLine("Console has ended it's work. Press any button to close.");
        Console.WriteLine("For work-history, open app.log file and see system's logs");

        new Logger().Logging("Console has ended it's work.");

        Environment.Exit(0);
    }
}
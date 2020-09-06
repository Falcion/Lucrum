using System;

public class Core {

    internal void CommandHandler() {

        string command = "";

        while(command != "shutdown") {

            Console.Write("Enter command: ");

            command = Console.ReadLine();

            new Logger().Logging("User typed new command: " + command + '.');

            string[] args = command.Split(' ');

            switch(args[0]) {

            }
        }

        Console.WriteLine("Console has ended it's work. Press any button to close.");
        Console.WriteLine("For work-history, open app.log file and see system's logs");

        new Logger().Logging("Console has ended it's work.");

        Environment.Exit(0);
    }
}
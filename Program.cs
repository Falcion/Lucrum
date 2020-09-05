using System;
using System.IO;

public class Program {

    static void Main(string[] args)
        => Enumerator();

    static void Enumerator() {

        new Logger().Filling();
        new Configuration().Setting();

        Console.WriteLine("Please, type command or write help for commands list. For more information, check software's GitHub repository.");

        new Core().CommandHandler();
    }
}
using System;

public class Context {

    public void Error(string error) {

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(error);

        new Logger().Logging(error);
    }
}
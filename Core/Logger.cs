using System;
using System.IO;

public class Logger {

    const string logFile = "app.log";

    public void Filling() {

        if(!File.Exists("app.log")) {

            File.Create("app.log", 4096)
                                        .Close();

            File.WriteAllText(logFile, "[" + $"{DateTime.Now}" + "] Logging file created successfully.");
        }
    }

    public virtual void Logging(string logMessage) {

        string fileLines = File.ReadAllText("app.log");

        if(fileLines.Length == 0) return;

        fileLines = fileLines + "\n" + "[" + $"{DateTime.Now}" + "] " + logMessage;

        File.WriteAllText(logFile, fileLines);
    } 
}
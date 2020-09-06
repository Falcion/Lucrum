using System;
using System.IO;

public class Configuration {

    public void Setting() {

        if(!File.Exists("app.conf")) {

            File.Create("app.conf", 1024)
                                        .Close();

            File.WriteAllText("app.conf", "apiToken = \"\"");

            Console.WriteLine("Configuration's file created successfully.");
            Console.WriteLine("Please, type your API token in this file.");

            new Logger().Logging("Configuration's file created successfully.");

            Environment.Exit(0);
        }

        if(File.Exists("app.conf")) {

            string[] configArray = File.ReadAllLines("app.conf");

            for(int i = 0; i < configArray.Length; i++) {

                if(configArray[i].StartsWith("apiToken = ")) {

                    configArray[i] = configArray[i].Remove(0, 11);

                    if(configArray[i].StartsWith("\"")) 
                            configArray[i] = configArray[i].Remove(0, 1);

                    else Error("Wrong token's format! Please, rewrite it or reboot configuration file!");

                    if(configArray[i].EndsWith("\"")) {

                        int charIndex = configArray[i].LastIndexOf('\"');

                        configArray[i] = configArray[i].Remove(charIndex, 1);
                    }

                    else Error("Wrong token's format! Please, rewrite it or reboot configuration file!");

                    if(configArray[i] == " " || configArray[i] == null) Error("Token is null! Please, check configuration file and re-write your's GitHub API token!"); 

                    Storage.ApiToken = configArray[i];

                    Console.WriteLine("Token was read successfully, configuration completed.");

                    new Logger().Logging("Token was read successfully, configuration completed.");
                }
            }
        }
    }

    private void Error(string error) {

        Console.WriteLine(error);

        new Logger().Logging(error);

        Environment.Exit(0);
    }
}
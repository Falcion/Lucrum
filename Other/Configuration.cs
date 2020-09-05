using System;
using System.IO;

public class Configuration {

    public void Setting() {

        if(!File.Exists("app.conf")) {

            File.Create("app.conf", 1024)
                                        .Close();

            File.WriteAllText("app.conf", "prefix = \"$ \"" + "\n" + "apiToken = \"\"");

            Console.WriteLine("Configuration's file created successfully.");
            Console.WriteLine("Please, type your API token in this file.");

            new Logger().Logging("Configuration's file created successfully.");

            Environment.Exit(0);
        }

        if(File.Exists("app.conf")) {

            string[] configArray = File.ReadAllLines("app.conf");

            for(int i = 0; i < configArray.Length; i++) {

                if(configArray[i].StartsWith("prefix = ")) {

                    configArray[i] = configArray[i].Remove(0, 9);

                    if(configArray[i].StartsWith("\"")) 
                            configArray[i] = configArray[i].Remove(0, 1);

                    else Error("Wrong prefix's format! Please, rewrite it or reboot configuration file!");

                    if(configArray[i].EndsWith("\"")) {

                        int charIndex = configArray[i].LastIndexOf('\"');

                        configArray[i] = configArray[i].Remove(charIndex, 1);
                    }

                    else Error("Wrong prefix's format! Please, rewrite it or reboot configuration file!");

                    Storage.Prefix = configArray[i];

                    Console.WriteLine("Prefix was read successfully, waiting token's parsing result.");

                    new Logger().Logging("Prefix was read successfully, waiting token's parsing result.");
                }

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
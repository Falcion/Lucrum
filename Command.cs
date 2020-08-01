using System.Threading.Tasks;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Octokit;
using System.IO;

namespace Lucrum
{
    public class Command
    {
        GitHubClient gitClient = new GitHubClient(new ProductHeaderValue("Lucrum"));

        public async Task Handler()
        {
            string gitToken = File.ReadAllText("api-token.conf");

            var tokenAuth = new Credentials(gitToken);
            gitClient.Credentials = tokenAuth;

            Console.OutputEncoding = Encoding.UTF8;
            string userPrefix = Global.userPrefix;
            string cmdTask = "";

            while (cmdTask != $"{userPrefix}shutdown")
            {
                userPrefix = Global.userPrefix;
                Console.Write("[Пользовательский ввод] ");
                cmdTask = Console.ReadLine();

                if (cmdTask.StartsWith(userPrefix)) await Schedule(cmdTask, userPrefix);
                else Console.WriteLine($"[{DateTime.Now}] Пропущен командный префикс.");
            }

            Console.WriteLine($"[{DateTime.Now}] Консоль успешно завершила свою работу.");
        }

        private async Task Schedule(string cmd, string prefix)
        {
            List<string> args = new List<string>();

            string newCmd = cmd.Remove(0, prefix.Length).Split(' ')[0];

            switch(newCmd)
            {
                case "git":
                    args = cmd.Remove(0, prefix.Length).Split(' ').ToList();
                    args.Remove(newCmd);

                    switch(args[0])
                    {
                        case "repos":

                            args.Remove("repos");

                            switch(args[0]) 
                            {
                                case "info":
                                    args.Remove("info");

                                    Repository repos = await gitClient.Repository.Get(args[0], args[1]);

                                    Console.WriteLine("-----[ Репозитория GitHub ]-----");
                                    Console.WriteLine($"Описание репозитории: {repos.Description}");
                                    Console.WriteLine($"ID репозитории: {repos.Id}");
                                    Console.WriteLine($"Лицензия репозитории: {repos.License.Name}");
                                    Console.WriteLine($"Дата создания: {repos.CreatedAt.DateTime}");
                                    Console.WriteLine($"Наименование репозитории: {repos.Name}");
                                    Console.WriteLine($"Последнее обновление: {repos.UpdatedAt.DateTime}");
                                    Console.WriteLine($"-----[ {repos.HtmlUrl} ]-----");
                                    break;

                                case "branches":
                                    args.Remove("branches");

                                    var branches = await gitClient.Repository.Branch.GetAll(args[0], args[1]);

                                    Console.WriteLine("-----[ Ветки репозитории ]-----");
                                    for(int i = 0; i < branches.Count; i++) 
                                    {
                                        var branch = branches[i];
                                        Console.WriteLine($"Ветка #{i+1}: {branch.Name}");
                                    }
                                    Console.WriteLine($"-----[ https://github.com/{args[0]}/{args[1]}/branches ]-----");
                                    break;

                                case "releases":
                                    args.Remove("releases");

                                    var releases = await gitClient.Repository.Release.GetAll(args[0], args[1]);

                                    int releaseCount = int.Parse(args[2]);

                                    if(releaseCount > releases.Count) releaseCount = releases.Count;

                                    if(releaseCount == 0) 
                                    {
                                        await Error("у данной репозитории нет выпусков!");
                                        return;
                                    }

                                    Console.WriteLine("-----[ Выпуски репозитории ]-----");
                                    for(int i = 0; i < releaseCount; i++) 
                                    {
                                        var release = releases[i];
                                        Console.WriteLine($"{release.Name} | {release.HtmlUrl}");
                                    }
                                    Console.WriteLine($"-----[ https://github.com/{args[0]}/{args[1]}/releases ]-----");
                                    break;

                                case "issues":
                                    args.Remove("issues");

                                    int days = int.Parse(args[2]);

                                    var recentlyIssues = new RepositoryIssueRequest
                                    {
                                    Filter = IssueFilter.All,
                                    State = ItemStateFilter.Open,
                                    Since = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(days))
                                    };

                                    args.Remove(args[2]);

                                    var issues = await gitClient.Issue.GetAllForRepository(args[0], args[1], recentlyIssues);

                                    Console.WriteLine("-----[ Темы репозитории ]-----");
                                    for(int i = 0; i < issues.Count; i++) {
                                        var issue = issues[i];

                                        Console.WriteLine($"{issue.Title} | {issue.HtmlUrl}");
                                    }
                                    Console.WriteLine($"-----[ https://github.com/{args[0]}/{args[1]}/issues ]-----");
                                    break;
                            }
                            break;

                        case "api-limit":
                            var miscRate = await gitClient.Miscellaneous.GetRateLimits();

                            var coreRate = miscRate.Resources.Core;

                            var corePerHour = coreRate.Limit;
                            var coreLeft = coreRate.Remaining;
                            var coreReset = coreRate.Reset;

                            Console.WriteLine("-----[ GitHub API ]-----");
                            Console.WriteLine($"Основных запросов в час: {corePerHour}");
                            Console.WriteLine($"Осталось основных запросов: {coreLeft}");
                            Console.WriteLine($"Сброс основных запросов: {coreReset}");
                            Console.WriteLine("-----[ Основные запросы ]-----");

                            var searchRate = miscRate.Resources.Search;

                            var searchPerMinute = searchRate.Limit;
                            var searchLeft = searchRate.Remaining;
                            var searchResest = searchRate.Reset;

                            Console.WriteLine($"Поисковых запросов в минуту: {searchPerMinute}");
                            Console.WriteLine($"Осталось поисковых запросов: {searchLeft}");
                            Console.WriteLine($"Сброс поисковых запросов: {searchResest}");
                            Console.WriteLine("-----[ Поисковые запросы ]-----");
                            break;
                    }
                    break;

                case "help":
                    Console.WriteLine("-----[ Помощь по командам ]-----");
                    Console.WriteLine($"[{DateTime.Now}] Командный префикс: {prefix}");
                    Console.WriteLine("-----[ Команды Git ]-----");
                    Console.WriteLine($"{prefix}git repos info [автор репозитории] [название репозитории]");
                    Console.WriteLine($"{prefix}git repos branches [автор репозитории] [название репозитории]");
                    Console.WriteLine($"{prefix}git repos releases [автор репозитории] [название репозитории] [количество выпусков]");
                    Console.WriteLine($"{prefix}git repos issues [автор репозитории] [название репозитории] [количество дней с момента открытия темы]");
                    Console.WriteLine("-----[ Системные команды ]-----");
                    Console.WriteLine($"{prefix}help");
                    Console.WriteLine($"{prefix}prefix [командный префикс]");
                    Console.WriteLine("-----[ Файловые команды ]-----");
                    Console.WriteLine($"{prefix}file create [полный путь] [название файла]");
                    Console.WriteLine($"{prefix}file delete [полный путь] [название файла]");
                    Console.WriteLine($"{prefix}file move [полный путь с названием файла] [полный путь с названием файла]");
                    Console.WriteLine($"{prefix}file replace [полный путь с названием файла] [полный путь с названием файла]");
                    Console.WriteLine($"{prefix}file rewrite [полный путь с названием файла] [текст]");
                    break;

                case "prefix":
                    args = cmd.Remove(0, prefix.Length).Split(' ').ToList();
                    args.Remove(newCmd);

                    Console.WriteLine($"[{DateTime.Now}] Командный префикс изменён на: {args[0]}");

                    string[] fileArray = File.ReadAllLines("settings.conf");

                    string oldPrefix = null;

                    for(int i = 0; i < fileArray.Length; i++)
                    {
                        if (fileArray[i].StartsWith('#')) continue;

                        if (fileArray[i].StartsWith("USERPREFIX="))
                        {
                            oldPrefix = fileArray[i].Remove(0, 11);
                        }
                    }

                    string fileText = File.ReadAllText("settings.conf");

                    fileText.Replace($"USERPREFIX={oldPrefix}", $"USERPREFIX={args[0]}");

                    File.WriteAllText("settings.conf", fileText);
                    Global.userPrefix = args[0];
                    break;

                case "file":
                    args.Remove("file");
                    
                    switch(args[0]) {
                        case "create":
                            args.Remove("create");

                            File.Create(args[0] + args[1]).Close();
                            Console.WriteLine($"[{DateTime.Now}] Файл успешно создан в пути: {args[0] + args[1]}");
                            break;

                        case "delete":
                            args.Remove("delete");

                            if(File.Exists(args[0] + args[1])) 
                            {
                                File.Delete(args[0] + args[1]);
                                Console.WriteLine($"[{DateTime.Now}] Файл успешно удалён");
                            }
                            else await Error("Такого файла в такой директории не существует!");
                            break;

                        case "move":
                            args.Remove("move");

                            if(args[0] == args[1]) 
                            {
                                await Error("Аргументы переноса файлов имеют одинаковые значения.");
                                return;
                            }

                            if(File.Exists(args[0])) 
                            {
                                File.Move(args[0], args[1]);
                                Console.WriteLine($"[{DateTime.Now}] Файл успешно передвинут. Пути: {args[0]} в {args[1]}");
                            }
                            else await Error("Такого файла в такой директории не существует!");
                            break;

                        case "replace":
                            args.Remove("replace");

                            if(File.Exists(args[0])) 
                            {
                                File.Replace(args[0], args[1], $"backup/replace-{DateTime.Now}");
                                Console.WriteLine($"[{DateTime.Now}] Данные файла успешно заменены. Файл резервного сохранения");
                            }
                            else await Error("Такого файла в такой директории не существует!");
                            break;

                        case "rewrite":
                            args.Remove("rewrite");

                            if(File.Exists(args[0])) 
                            {
                                File.WriteAllText(args[0], $"{args.Skip(1)}");
                                Console.WriteLine($"[{DateTime.Now}] Файл успешно перезаписан.");
                            }
                            else await Error("Такого файла в такой директории не существует!");
                            break;
                    }
                    break;

                default:
                    await Error("Неизвестная команда или неправильно указанные аргументы.");
                    break;
            }
        }

        public Task Error(string errDesc)
        {
            Console.WriteLine($"[{DateTime.Now}] Произошла непредвиденная ошибка! Дополнительная информация: {errDesc}");

            return Task.CompletedTask;
        }
    }
}

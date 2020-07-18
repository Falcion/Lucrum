using System.Threading.Tasks;
using System;
using System.Text;
using System.IO;

namespace Lucrum
{
    public class Program
    {
        static void Main(string[] args)
                => new Program().Initialize().GetAwaiter().GetResult();

        private async Task Initialize()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine($"[{DateTime.Now}] Консоль успешно инициализирована.");

            if(!File.Exists("settings.conf"))
            {
                Console.WriteLine($"[{DateTime.Now}] Файл конфигурации не найден. Создаём его.");

                File.Create("settings.conf").Close();

                File.WriteAllText("settings.conf", "###################" + Environment.NewLine
                                                 + "# TERMINAL CONFIG #" + Environment.NewLine
                                                 + "###################" + Environment.NewLine
                                                 + "USERPREFIX=+");

                Console.WriteLine($"[{DateTime.Now}] Файл конфигурации успешно создан.");

                string[] confArray = File.ReadAllLines("settings.conf");

                int confCount = confArray.Length;

                for(int i = 0; i < confCount; i++)
                {
                    string confLine = confArray[i];

                    if (confLine.StartsWith('#')) return;

                    if (confLine.StartsWith("USERPREFIX=")) Global.userPrefix = confLine.Remove(0, 11);
                }

                Console.WriteLine($"[{DateTime.Now}] Конфигурация успешно завершена.");
            }
            else
            {
                Console.WriteLine($"[{DateTime.Now}] Файл конфигурации найден. Считываем.");

                string[] confArray = File.ReadAllLines("settings.conf");

                int confCount = confArray.Length;

                for (int i = 0; i < confCount; i++)
                {
                    string confLine = confArray[i];

                    if (confLine.StartsWith("USERPREFIX=")) Global.userPrefix = confLine.Remove(0, 11);
                }

                Console.WriteLine($"[{DateTime.Now}] Конфигурация успешно завершена.");
            }

            Console.WriteLine($"[{DateTime.Now}] Запуск командного модуля...");
            Console.WriteLine($"[{DateTime.Now}] Командный префикс: {Global.userPrefix}");

            await new Command().Handler();
        }
    }
}

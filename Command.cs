using System.Threading.Tasks;
using System;
using System.Text;

namespace Lucrum
{
    public class Command
    {
        public async Task Handler()
        {
            Console.OutputEncoding = Encoding.UTF8;

            string userPrefix = Global.userPrefix;
            string cmdTask = "";

            while (cmdTask != $"{userPrefix}shutdown")
            {
                Console.Write("[Пользовательский ввод] ");
                cmdTask = Console.ReadLine();

                if (cmdTask.StartsWith(userPrefix)) await Schedule(cmdTask, userPrefix);
                else Console.WriteLine($"[{DateTime.Now}] Пропущен командный префикс.");
            }

            Console.WriteLine($"[{DateTime.Now}] Консоль успешно завершила свою работу.");
        }

        private async Task Schedule(string cmd, string prefix)
        {
            int prefixCount = prefix.Length;
            string newCmd = cmd.Remove(0, prefixCount);

            switch(newCmd)
            {
                case "help":
                    Console.WriteLine($"[{DateTime.Now}] Помощь по командам:");
                    Console.WriteLine($"[{DateTime.Now}] Префикс команд: {prefix}");
                    break;

                case "usage":
                    break;

                default:
                    Console.WriteLine($"[{DateTime.Now}] Ошибка: неправильно написана команда или её аргументы.");
                    break;
            }
        }
    }
}

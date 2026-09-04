using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTNKDxf
{
    public static class ConsoleAnimation
    {
        public static void RunWithSpinner(string message, Action action)
        {
            Console.Write(message);

            using (var cts = new CancellationTokenSource())
            {
                var animTask = Task.Run(async () =>
                {
                    char[] spinner = new char[] { '|', '/', '-', '\\' };
                    int counter = 0;
                    while (!cts.Token.IsCancellationRequested)
                    {
                        Console.Write(spinner[counter++ % 4]);
                        await Task.Delay(100);

                        try
                        {
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        }
                        catch { }
                    }

                    Console.Write(" ");
                    try
                    {
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    }
                    catch { }
                });

                action();

                cts.Cancel();
                animTask.Wait();
            }

            Console.WriteLine("Concluído!");
        }
    }
}

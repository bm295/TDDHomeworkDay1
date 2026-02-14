using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var tasks = new List<Task>();

        for (int i = 0; i < 3; i++)
        {
            tasks.Add(Task.Run(async () =>
            {
                await Task.Delay(10);
                Console.Write(i + " ");
            }));
        }

        await Task.WhenAll(tasks);
    }
}

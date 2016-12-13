using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Slalom.Stacks.Configuration;
using Slalom.Stacks.Data.EntityFramework;
// ReSharper disable AccessToDisposedClosure

#pragma warning disable 1998
#pragma warning disable 4014

namespace ConsoleClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Factory.StartNew(() => new Program().Start());
            Console.WriteLine("Running application.  Press any key to halt...");
            Console.ReadKey();
        }

        public async Task Start()
        {
            try
            {
                var watch = new Stopwatch();
                using (var container = new ApplicationContainer(this))
                {
                    container.UseEntityFrameworkSearch();

                    watch.Start();
                    for (int i = 0; i < 100; i++)
                    {
                        await Task.Run(() => container.Search.AddAsync(new ItemSearchResult()).ConfigureAwait(false));
                    }

                    var target = container.Search.OpenQuery<ItemSearchResult>().Where(e => e.Name.Contains("a")).ToList();
                    watch.Stop();
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Execution completed successfully in {watch.Elapsed}.  Press any key to exit...");
                Console.ResetColor();
            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exception);
                Console.ResetColor();
            }
        }
    }
}
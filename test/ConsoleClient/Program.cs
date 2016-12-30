using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Slalom.Stacks;
using Slalom.Stacks.Data.EntityFramework;
using Slalom.Stacks.Messaging;
using Slalom.Stacks.Test.Examples.Actors.Items.Add;
using Slalom.Stacks.Test.Examples.Domain;
using Slalom.Stacks.Test.Examples.Search;

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
                var count = 1000;
                using (var container = new ApplicationContainer(typeof(Item)))
                {
                    container.UseEntityFrameworkSearch(e =>
                    {
                        e.WithAutoAddSearchResults();
                        e.WithForcedMigrations();
                    });

                    await container.Search.ClearAsync<ItemSearchResult>();

                    watch.Start();

                    var tasks = new List<Task<CommandResult>>(count);
                    Parallel.For(0, count, new ParallelOptions { MaxDegreeOfParallelism = 4 }, a =>
                    {
                        tasks.Add(container.SendAsync(new AddItemCommand("test " + a)));
                    });
                    await Task.WhenAll(tasks);

                    var failed = tasks.Where(e => !e.Result.IsSuccessful).Select(e => e.Result);

                    Console.WriteLine(failed.Count());

                    watch.Stop();

                    var actual = container.Search.OpenQuery<ItemSearchResult>().Count();
                    if (actual != count)
                    {
                        throw new Exception($"The expected count, {count}, did not equal the actual count, {actual}.");
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Execution for {count:N0} items completed successfully in {watch.Elapsed} - {Math.Ceiling(count / watch.Elapsed.TotalSeconds):N0} per second.  Press any key to exit...");
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
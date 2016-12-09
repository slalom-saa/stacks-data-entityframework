using System;
using System.Linq;
using System.Threading.Tasks;
using Slalom.Stacks.Configuration;
using Slalom.Stacks.Data.EntityFramework;

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
                using (var container = new ApplicationContainer(this))
                {
                    container.RegisterModule(new EntityFrameworkSearchModule());

                    await container.Search.AddAsync(new ItemSearchResult());

                    await container.Search.RebuildIndexAsync<ItemSearchResult>();

                    var count = container.Search.CreateQuery<ItemSearchResult>().Count();
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Execution completed successfully.  Press any key to exit...");
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
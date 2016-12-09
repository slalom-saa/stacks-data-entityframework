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
            new Program().Run();
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }

        public async Task Run()
        {
            try
            {
                using (var container = new ApplicationContainer(this))
                {
                    container.RegisterModule(new EntityFrameworkSearchModule());

                    await container.Search.AddAsync(new ItemSearchResult());

                    await container.Search.RebuildIndexAsync<ItemSearchResult>();

                    var search = container.Search.CreateQuery<ItemSearchResult>();

                    Console.WriteLine(search.Count());
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            Console.WriteLine("Done with async execution.");
        }
    }
}
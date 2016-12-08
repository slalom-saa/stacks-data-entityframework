using System;
using System.Linq;
using System.Threading.Tasks;
using Slalom.Stacks.Configuration;
using Slalom.Stacks.Data.EntityFramework;
using Slalom.Stacks.Search;
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
                    container.RegisterModule(new EntityFrameworkSearchModule(c =>
                    {
                        c.WithSearchResults(typeof(ItemSearchResult));
                    }));

                    //container.Register<ISearchIndexer<ItemSearchResult>>(c => new ItemSearchResultIndexer(c.Resolve<SearchContext>()));

                    await container.Search.AddAsync(new ItemSearchResult());

                    var search = container.Search.CreateQuery<ItemSearchResult>();

                    Console.WriteLine(search.Count());
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            Console.WriteLine("Done executing");
        }

        public class ItemSearchResult : ISearchResult
        {
            public int Id { get; set; }
        }

        public class ItemSearchResultIndexer : SearchIndexer<ItemSearchResult>
        {
            public ItemSearchResultIndexer(ISearchContext context)
                : base(context)
            {
            }

            public override Task AddAsync(ItemSearchResult[] instances)
            {
                return base.AddAsync(instances);
            }
        }
    }
}
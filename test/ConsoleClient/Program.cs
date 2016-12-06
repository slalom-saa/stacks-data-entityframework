using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Slalom.Stacks.Configuration;
using Slalom.Stacks.Data.EntityFramework;
using Slalom.Stacks.Search;

#pragma warning disable 4014

namespace ConsoleClient
{
    public class Program
    {
        private Context context;

        public class Item : ISearchResult
        {
            public int Id { get; set; }
        }

        public class Context : DbContext
        {
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);

                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Search;Integrated Security=True");
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<Item>()
                            .ToTable("Items")
                            .HasKey(e => e.Id);
            }
        }

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
                    container.Register(c => new Context());

                    context = container.Resolve<Context>();

                    await context.EnsureMigrationsAsync();

                    container.Register(c => new EntityFrameworkSearchContext(c.Resolve<Context>()));

                    container.Register<ISearchIndex<Item>>(c => new SearchIndex<Item>(c.Resolve<EntityFrameworkSearchContext>()));

                    await container.Search.AddAsync(new Item());
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            Console.WriteLine("Done executing");
        }
    }
}
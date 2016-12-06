using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Slalom.Stacks.Data.EntityFramework
{
    public static class DbContextExtensions
    {
        public static void EnsureMigrations(this DbContext context)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        public static async Task EnsureMigrationsAsync(this DbContext context)
        {
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
        }
    }
}
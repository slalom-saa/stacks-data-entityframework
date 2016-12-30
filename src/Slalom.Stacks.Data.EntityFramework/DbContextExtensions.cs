using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Slalom.Stacks.Data.EntityFramework
{
    /// <summary>
    /// Extensions for <see cref="DbContext"/> classes.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Ensures that the database is created and no migrations are pending.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="force">If set to <c>true</c> then force the migrations.</param>
        public static void EnsureMigrations(this DbContext context, bool force = false)
        {
            context.Database.EnsureCreated();
            if (force || context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        /// <summary>
        /// Ensures that the database is created and no migrations are pending.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Task.</returns>
        /// <param name="force">If set to <c>true</c> then force the migrations.</param>
        public static async Task EnsureMigrationsAsync(this DbContext context, bool force = false)
        {
            await context.Database.EnsureCreatedAsync();
            if (force || (await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
        }
    }
}
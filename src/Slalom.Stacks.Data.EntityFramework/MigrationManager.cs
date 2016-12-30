using Microsoft.EntityFrameworkCore;

namespace Slalom.Stacks.Data.EntityFramework
{
    internal class MigrationManager
    {
        public bool Migrated { get; set; }

        public void EnsureMigrations(DbContext context, EntityFrameworkSearchOptions options)
        {
            if (!this.Migrated)
            {
                this.Migrated = true;
                if (options.WithMigration)
                {
                    context.Database.EnsureCreated();
                }
            }
        }
    }
}
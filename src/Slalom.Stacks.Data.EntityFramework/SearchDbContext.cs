using System;
using Microsoft.EntityFrameworkCore;

namespace Slalom.Stacks.Data.EntityFramework
{
    public class SearchDbContext : DbContext
    {
        private readonly EntityFrameworkSearchOptions _options;

        public SearchDbContext(EntityFrameworkSearchOptions options)
        {
            _options = options;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_options.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var item in _options.ResultTypes)
            {
                modelBuilder.Entity(item)
                            .HasKey("Id");
            }
        }
    }
}
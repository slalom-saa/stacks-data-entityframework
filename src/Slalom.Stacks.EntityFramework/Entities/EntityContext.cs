using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Slalom.Stacks.Domain;
using Slalom.Stacks.Reflection;

namespace Slalom.Stacks.EntityFramework.Entities
{
    public class EntityContext : DbContext, IEntityContext
    {

        private readonly Assembly[] _assemblies;

        public EntityContext(EntityFrameworkOptions options) : base(options.ConnectionString)
        {
            _assemblies = options.Assemblies.ToArray();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityMethod = typeof(DbModelBuilder).GetMethod("Entity");

            foreach (var assembly in _assemblies)
            {
                foreach (var type in assembly.SafelyGetTypes(typeof(IAggregateRoot)))
                {
                    if (type.IsAbstract || type.IsInterface)
                    {
                        continue;
                    }
                    entityMethod.MakeGenericMethod(type)
                        .Invoke(modelBuilder, new object[] { });
                }
            }
        }


        public Task Add<TEntity>(TEntity[] instances) where TEntity : class, IAggregateRoot
        {
            this.Set<TEntity>().AddRange(instances);

            return this.SaveChangesAsync();
        }

        public Task Clear<TEntity>() where TEntity : class, IAggregateRoot
        {
            var set = this.Set<TEntity>();
            set.RemoveRange(set);

            return this.SaveChangesAsync();
        }

        public Task<bool> Exists<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, IAggregateRoot
        {
            var set = this.Set<TEntity>();

            return QueryableExtensions.AnyAsync(set, expression);
        }

        public Task<bool> Exists<TEntity>(string id) where TEntity : class, IAggregateRoot
        {
            var set = this.Set<TEntity>();

            return QueryableExtensions.AnyAsync<TEntity>(set, e => e.Id == id);
        }

        public Task<TEntity> Find<TEntity>(string id) where TEntity : class, IAggregateRoot
        {
            var set = this.Set<TEntity>();

            return set.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> Find<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, IAggregateRoot
        {
            var set = this.Set<TEntity>();

            var result =  await Queryable.Where(set, expression).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<TEntity>> Find<TEntity>() where TEntity : class, IAggregateRoot
        {
            var set = this.Set<TEntity>();

            var result = await QueryableExtensions.ToListAsync<TEntity>(set);

            return result;
        }

        public Task Remove<TEntity>(TEntity[] instances) where TEntity : class, IAggregateRoot
        {
            var ids = instances.Select(e => e.Id).ToList();

            var set = this.Set<TEntity>();
            set.RemoveRange(Queryable.Where<TEntity>(set, e => ids.Contains(e.Id)));

            return this.SaveChangesAsync();
        }

        public Task Update<TEntity>(TEntity[] instances) where TEntity : class, IAggregateRoot
        {
            DbSetMigrationsExtensions.AddOrUpdate(this.Set<TEntity>(), instances);

            return this.SaveChangesAsync();
        }
    }
}
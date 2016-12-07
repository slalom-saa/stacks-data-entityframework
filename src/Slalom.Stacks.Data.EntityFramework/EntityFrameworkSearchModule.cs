using System;
using Autofac;
using Slalom.Stacks.Search;

namespace Slalom.Stacks.Data.EntityFramework
{
    public class EntityFrameworkSearchModule : Module
    {
        private readonly EntityFrameworkSearchOptions _options = new EntityFrameworkSearchOptions();

        public EntityFrameworkSearchModule()
        {
        }

        public EntityFrameworkSearchModule(Action<EntityFrameworkSearchOptions> configuration)
        {
            configuration(_options);
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c =>
            {
                var context = new SearchDbContext(_options);
                context.EnsureMigrations();
                return context;
            }).SingleInstance();

            builder.Register(c => new EntityFrameworkSearchContext(c.Resolve<SearchDbContext>()))
                   .AsSelf()
                   .As<ISearchContext>()
                   .SingleInstance();

            foreach (var type in _options.ResultTypes)
            {
                builder.Register(c => Activator.CreateInstance(typeof(SearchIndexer<>).MakeGenericType(type), c.Resolve<EntityFrameworkSearchContext>()))
                       .As(typeof(ISearchIndexer<>).MakeGenericType(type));
            }
        }
    }
}
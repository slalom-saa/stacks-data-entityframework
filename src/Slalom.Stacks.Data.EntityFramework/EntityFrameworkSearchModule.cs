using System;
using System.Linq;
using Autofac;
using Microsoft.Extensions.Configuration;
using Slalom.Stacks.Reflection;
using Slalom.Stacks.Search;
using Slalom.Stacks.Validation;
using System.Reflection;
using Module = Autofac.Module;

namespace Slalom.Stacks.Data.EntityFramework
{
    /// <summary>
    /// An Autofac module for configuring the Entity Framework Search module.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    public class EntityFrameworkSearchModule : Module
    {
        private readonly EntityFrameworkSearchOptions _options = new EntityFrameworkSearchOptions();

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkSearchModule"/> class.
        /// </summary>
        public EntityFrameworkSearchModule()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkSearchModule"/> class.
        /// </summary>
        /// <param name="configuration">The configuration routine.</param>
        public EntityFrameworkSearchModule(Action<EntityFrameworkSearchOptions> configuration)
        {
            Argument.NotNull(() => configuration);

            configuration(_options);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkSearchModule"/> class.
        /// </summary>
        /// <param name="options">The options to use.</param>
        public EntityFrameworkSearchModule(EntityFrameworkSearchOptions options)
        {
            Argument.NotNull(() => options);

            _options = options;
        }

        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        /// <remarks>Note that the ContainerBuilder parameter is unique to this module.</remarks>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new MigrationMarker()).SingleInstance();

            builder.Register(c => new SearchContext(_options))
                   .AsSelf()
                   .As<ISearchContext>()
                   .OnActivated(e =>
                   {
                       var marker = e.Context.Resolve<MigrationMarker>();
                       if (!marker.Migrated)
                       {
                           marker.Migrated = true;
                           e.Instance.EnsureMigrations();
                       }
                       e.Instance.ChangeTracker.AutoDetectChangesEnabled = false;
                   }).OnPreparing(e =>
                   {
                       var configuration = e.Context.Resolve<IConfiguration>();
                       _options.ConnectionString = configuration["Stacks:Data:EntityFramework:ConnectionString"] ?? _options.ConnectionString;
                       _options.AutoAddSearchResults = Convert.ToBoolean(configuration["Stacks:Data:EntityFramework:AutoAddSearchResults"] ?? _options.AutoAddSearchResults.ToString());
                       if (_options.AutoAddSearchResults)
                       {
                           var types = e.Context.Resolve<IDiscoverTypes>().Find<ISearchResult>()
                                        .Where(x => !x.GetTypeInfo().IsAbstract && !x.GetTypeInfo().IsInterface);
                           _options.SearchResultTypes.AddRange(types);
                       }
                   });
        }
    }
}
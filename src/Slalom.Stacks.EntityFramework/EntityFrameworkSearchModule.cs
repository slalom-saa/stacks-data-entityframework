using System;
using Autofac;
using Autofac.Core;
using Slalom.Stacks.Reflection;
using Slalom.Stacks.Search;
using Slalom.Stacks.Validation;

#if core
using Slalom.Stacks.Configuration;
using Microsoft.Extensions.Configuration;
#else
using System.Configuration;
#endif

namespace Slalom.Stacks.EntityFramework
{
    /// <summary>
    /// An Autofac module for configuring the Entity Framework Search module.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    internal class EntityFrameworkSearchModule : Module
    {
        private readonly EntityFrameworkSearchOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkSearchModule"/> class.
        /// </summary>
        /// <param name="options">The options to use.</param>
        public EntityFrameworkSearchModule(EntityFrameworkSearchOptions options)
        {
            Argument.NotNull(options, nameof(options));

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

            builder.Register(c => new MigrationManager())
                .SingleInstance();

            builder.Register(c => new DiscoveredSearchResultTypes(c.Resolve<IDiscoverTypes>()))
                   .SingleInstance();

            builder.Register(c => new SearchContext(_options))
                   .AsSelf()
                   .As<ISearchContext>()
                   .OnActivated(this.OnContextActivated)
                   .OnPreparing(this.OnContextPreparing)
                   .AllPropertiesAutowired();

        }

        private void OnContextActivated(IActivatedEventArgs<SearchContext> e)
        {
            var manager = e.Context.Resolve<MigrationManager>();
            manager.EnsureMigrations(e.Instance, _options);
            e.Instance.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        private void OnContextPreparing(PreparingEventArgs e)
        {
            
#if !core
            var connectionString = ConfigurationManager.AppSettings["Stacks:Data:EntityFramework:ConnectionString"];
            if (!String.IsNullOrWhiteSpace(connectionString))
            {
                _options.ConnectionString = connectionString;
            }

            var autoFind = ConfigurationManager.AppSettings["Stacks:Data:EntityFramework:ConnectionString"];
            if (!String.IsNullOrWhiteSpace(autoFind))
            {
                _options.AutoAddSearchResults = Convert.ToBoolean(autoFind);
            }
#else
            var configuration = e.Context.Resolve<IConfiguration>();

            var connectionString = configuration["Stacks:Data:EntityFramework:ConnectionString"];
            if (!String.IsNullOrWhiteSpace(connectionString))
            {
                _options.ConnectionString = connectionString;
            }

            var autoFind = configuration["Stacks:Data:EntityFramework:ConnectionString"];
            if (!String.IsNullOrWhiteSpace(autoFind))
            {
                _options.AutoAddSearchResults = Convert.ToBoolean(autoFind);
            }
#endif

            if (_options.AutoAddSearchResults)
            {
                _options.SearchResultTypes = e.Context.Resolve<DiscoveredSearchResultTypes>().Items.Value;
            }
        }
    }
}
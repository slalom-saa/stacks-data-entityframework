using System;
using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Slalom.Stacks.Configuration;
using Slalom.Stacks.Reflection;
using Slalom.Stacks.Search;
using Slalom.Stacks.Validation;

namespace Slalom.Stacks.Data.EntityFramework
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

            builder.Register(c => new MigrationMarker())
                .SingleInstance();

            builder.Register(c => new DiscoveredSearchResultTypes(c.Resolve<IDiscoverTypes>()))
                   .SingleInstance();

            builder.Register(c => new SearchContext(_options))
                   .AsSelf()
                   .As<ISearchContext>()
                   .OnActivated(this.OnContextActivated)
                   .OnPreparing(this.OnContextPreparing);
        }

        private void OnContextActivated(IActivatedEventArgs<SearchContext> e)
        {
            var marker = e.Context.Resolve<MigrationMarker>();
            if (!marker.Migrated)
            {
                marker.Migrated = true;
                e.Instance.EnsureMigrations(_options.ForceMigrations);
            }
            e.Instance.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        private void OnContextPreparing(PreparingEventArgs e)
        {
            var configuration = e.Context.Resolve<IConfiguration>();
            _options.ConnectionString = configuration.GetOptionalSetting("Stacks:Data:EntityFramework:ConnectionString", _options.ConnectionString);
            _options.AutoAddSearchResults = Convert.ToBoolean(configuration.GetOptionalSetting("Stacks:Data:EntityFramework:AutoAddSearchResults", "true"));
            if (_options.AutoAddSearchResults)
            {
                _options.SearchResultTypes = e.Context.Resolve<DiscoveredSearchResultTypes>().Items.Value;
            }
        }
    }
}
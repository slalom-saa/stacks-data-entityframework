using System;
using Autofac;
using Slalom.Stacks.Search;
using Slalom.Stacks.Validation;

namespace Slalom.Stacks.Data.EntityFramework
{
    /// <summary>
    /// An Autofac module for configuring the Entity Framework Search module.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    public class EntityFrameworkSearchModule : Module
    {
        private readonly SearchOptions _options = new SearchOptions();

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
        public EntityFrameworkSearchModule(Action<SearchOptions> configuration)
        {
            Argument.NotNull(() => configuration);

            configuration(_options);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkSearchModule"/> class.
        /// </summary>
        /// <param name="options">The options to use.</param>
        public EntityFrameworkSearchModule(SearchOptions options)
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

            builder.Register(c => new SearchContext(_options))
                   .AsSelf()
                   .As<ISearchContext>()
                   .SingleInstance().OnActivated(e =>
                   {
                       e.Instance.EnsureMigrations();
                   });
        }
    }
}
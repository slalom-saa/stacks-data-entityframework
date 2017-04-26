using System;
using System.Configuration;
using Autofac;
using Autofac.Core;
using Slalom.Stacks.Search;
using Slalom.Stacks.Validation;
#if core
using Slalom.Stacks.Configuration;
using Microsoft.Extensions.Configuration;
#else

#endif

namespace Slalom.Stacks.EntityFramework.Search
{
    /// <summary>
    /// An Autofac module for configuring the Entity Framework Search module.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    internal class EntityFrameworkSearchModule : Module
    {
        private readonly Stack _stack;
        private readonly EntityFrameworkOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkSearchModule" /> class.
        /// </summary>
        /// <param name="stack">The configured stack.</param>
        /// <param name="options">The options to use.</param>
        public EntityFrameworkSearchModule(Stack stack, EntityFrameworkOptions options)
        {
            Argument.NotNull(options, nameof(options));

            _stack = stack;
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
                   .OnPreparing(this.OnContextPreparing)
                   .AllPropertiesAutowired();

        }

        private void OnContextPreparing(PreparingEventArgs e)
        {
            
#if !core
            var connectionString = ConfigurationManager.AppSettings["Stacks:Data:EntityFramework:ConnectionString"];
            if (!String.IsNullOrWhiteSpace(connectionString))
            {
                _options.ConnectionString = connectionString;
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
        }
    }
}
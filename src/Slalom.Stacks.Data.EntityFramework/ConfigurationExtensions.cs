using System;
using System.Linq;
using Slalom.Stacks.Configuration;
using Slalom.Stacks.Validation;

namespace Slalom.Stacks.Data.EntityFramework
{
    /// <summary>
    /// Contains extension methods to add Entity Framework Data blocks.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Adds the Entity Framework Search block.
        /// </summary>
        /// <param name="instance">The container instance.</param>
        /// <param name="configuration">The configuration routine.</param>
        /// <returns>Returns the container instance for method chaining.</returns>
        public static ApplicationContainer UseEntityFrameworkSearch(this ApplicationContainer instance, Action<EntityFrameworkSearchOptions> configuration = null)
        {
            Argument.NotNull(instance, nameof(instance));

            var options = new EntityFrameworkSearchOptions();
            configuration?.Invoke(options);

            instance.RegisterModule(new EntityFrameworkSearchModule(options));
            return instance;
        }

        /// <summary>
        /// Adds the Entity Framework Search block.
        /// </summary>
        /// <param name="instance">The container instance.</param>
        /// <param name="options">The options to use.</param>
        /// <returns>Returns the container instance for method chaining.</returns>
        public static ApplicationContainer UseEntityFrameworkSearch(this ApplicationContainer instance, EntityFrameworkSearchOptions options)
        {
            Argument.NotNull(instance, nameof(instance));
            Argument.NotNull(options, nameof(options));

            instance.RegisterModule(new EntityFrameworkSearchModule(options));
            return instance;
        }
    }
}
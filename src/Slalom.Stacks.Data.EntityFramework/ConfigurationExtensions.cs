using System;
using System.Linq;
using Slalom.Stacks.Configuration;
using Slalom.Stacks.Validation;

namespace Slalom.Stacks.Data.EntityFramework
{
    /// <summary>
    /// Contains extension methods for adding Entity Framework Data blocks.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Adds the Entity Framework Search block to the container.
        /// </summary>
        /// <param name="instance">The container instance.</param>
        /// <returns>Returns the container instance for method chaining.</returns>
        public static ApplicationContainer UseEntityFrameworkSearch(this ApplicationContainer instance)
        {
            Argument.NotNull(() => instance);

            instance.RegisterModule(new EntityFrameworkSearchModule());
            return instance;
        }

        /// <summary>
        /// Adds the Entity Framework Search block to the container.
        /// </summary>
        /// <param name="instance">The container instance.</param>
        /// <param name="configuration">The configuration routine.</param>
        /// <returns>Returns the container instance for method chaining.</returns>
        public static ApplicationContainer UseEntityFrameworkSearch(this ApplicationContainer instance, Action<EntityFrameworkSearchOptions> configuration)
        {
            Argument.NotNull(() => instance);

            instance.RegisterModule(new EntityFrameworkSearchModule(configuration));
            return instance;
        }

        /// <summary>
        /// Adds the Entity Framework Search block to the container.
        /// </summary>
        /// <param name="instance">The container instance.</param>
        /// <param name="options">The options to use.</param>
        /// <returns>Returns the container instance for method chaining.</returns>
        public static ApplicationContainer UseEntityFrameworkSearch(this ApplicationContainer instance, EntityFrameworkSearchOptions options)
        {
            Argument.NotNull(() => instance);

            instance.RegisterModule(new EntityFrameworkSearchModule(options));
            return instance;
        }
    }
}
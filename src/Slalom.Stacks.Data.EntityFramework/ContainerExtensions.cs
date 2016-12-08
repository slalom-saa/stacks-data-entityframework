using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slalom.Stacks.Data.EntityFramework;
using Slalom.Stacks.Validation;

namespace Slalom.Stacks.Configuration
{
    /// <summary>
    /// Contains extension methods for adding Entity Framework blocks.
    /// </summary>
    public static class ContainerExtensions
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
    }
}

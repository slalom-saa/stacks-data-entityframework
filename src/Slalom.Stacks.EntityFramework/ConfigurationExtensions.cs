/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using Slalom.Stacks.EntityFramework.Entities;
using Slalom.Stacks.EntityFramework.Search;
using Slalom.Stacks.Validation;

namespace Slalom.Stacks.EntityFramework
{
    /// <summary>
    /// Contains extension methods to add Entity Framework.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Adds Entity Framework.
        /// </summary>
        /// <param name="instance">The container instance.</param>
        /// <param name="configuration">The configuration routine.</param>
        /// <returns>Returns the container instance for method chaining.</returns>
        public static Stack UseEntityFramework(this Stack instance, Action<EntitySettings> configuration = null)
        {
            Argument.NotNull(instance, nameof(instance));

            var options = new EntityFrameworkOptions();
            configuration?.Invoke(options.Data);
            instance.Configuration.GetSection("Stacks:EntityFramework").Bind(options);
            options.Assemblies = instance.Assemblies;

            instance.Include(typeof(EntityContext));

            instance.Use(e => { e.RegisterModule(new EntityFrameworkEntitiesModule(instance, options)); });
            return instance;
        }

        /// <summary>
        /// Adds Entity Framework Search.
        /// </summary>
        /// <param name="instance">The container instance.</param>
        /// <param name="configuration">The configuration routine.</param>
        /// <returns>Returns the container instance for method chaining.</returns>
        public static Stack UseEntityFrameworkSearch(this Stack instance, Action<SearchSettings> configuration = null)
        {
            Argument.NotNull(instance, nameof(instance));

            var options = new EntityFrameworkOptions();
            configuration?.Invoke(options.Search);
            instance.Configuration.GetSection("Stacks:EntityFramework").Bind(options);
            options.Assemblies = instance.Assemblies;

            instance.Include(typeof(SearchContext));

            instance.Use(e => { e.RegisterModule(new EntityFrameworkSearchModule(instance, options)); });

            return instance;
        }
    }
}
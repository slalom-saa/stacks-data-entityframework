/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;
using System.Reflection;
using Slalom.Stacks.Serialization;

namespace Slalom.Stacks.EntityFramework
{
    /// <summary>
    /// Settings for Entity Framework search.
    /// </summary>
    public class SearchSettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; set; } = "Data Source=.;Initial Catalog=Stacks.Search;Integrated Security=True;MultipleActiveResultSets=True";

        /// <summary>
        /// Gets or sets a value that indicates whether or not EF code first migrations should apply.
        /// </summary>
        /// <value>A value that indicates whether or not EF code first migrations should apply.</value>
        public bool EnableMigrations { get; set; } = false;

        /// <summary>
        /// Sets the connection string to use.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>Returns this instance for method chaining.</returns>
        public SearchSettings WithConnectionString(string connectionString)
        {
            this.ConnectionString = connectionString;

            return this;
        }
    }

    /// <summary>
    /// Settings for Entity Framework data.
    /// </summary>
    public class EntitySettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; set; } = "Data Source=.;Initial Catalog=Stacks;Integrated Security=True;MultipleActiveResultSets=True";

        /// <summary>
        /// Gets or sets a value that indicates whether or not EF code first migrations should apply.
        /// </summary>
        /// <value>A value that indicates whether or not EF code first migrations should apply.</value>
        public bool EnableMigrations { get; set; } = false;

        /// <summary>
        /// Sets the connection string to use.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>Returns this instance for method chaining.</returns>
        public EntitySettings WithConnectionString(string connectionString)
        {
            this.ConnectionString = connectionString;

            return this;
        }
    }

    /// <summary>
    /// Options for Entity Framework.
    /// </summary>
    public class EntityFrameworkOptions
    {
        /// <summary>
        /// Gets or sets the data settings.
        /// </summary>
        /// <value>The data settings.</value>
        public EntitySettings Data { get; set; } = new EntitySettings();

        /// <summary>
        /// Gets or sets the search settings.
        /// </summary>
        /// <value>The search settings.</value>
        public SearchSettings Search { get; set; } = new SearchSettings();

        internal IEnumerable<Assembly> Assemblies { get; set; }
    }
}
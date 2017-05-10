/* 
 * Copyright (c) Stacks Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Collections.Generic;
using System.Reflection;

namespace Slalom.Stacks.EntityFramework
{
    /// <summary>
    /// Settings for search.
    /// </summary>
    public class SearchSettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; set; } = "Data Source=.;Initial Catalog=Stacks.Search;Integrated Security=True;MultipleActiveResultSets=True";
    }

    /// <summary>
    /// Settings for data.
    /// </summary>
    public class EntitySettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; set; } = "Data Source=.;Initial Catalog=Stacks;Integrated Security=True;MultipleActiveResultSets=True";
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
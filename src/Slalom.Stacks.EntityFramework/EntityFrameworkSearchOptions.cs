using System;
using System.Collections.Generic;
using Slalom.Stacks.Search;
using Slalom.Stacks.Validation;
using System.Reflection;

namespace Slalom.Stacks.EntityFramework
{
    /// <summary>
    /// Options for the Entity Framework Search module.
    /// </summary>
    public class EntityFrameworkSearchOptions
    {
        internal bool AutoAddSearchResults { get; set; } = true;

        internal string ConnectionString { get; set; } = "Data Source=localhost;Initial Catalog=Stacks.Search;Integrated Security=True;MultipleActiveResultSets=True";

        internal bool WithMigration { get; set; }

        /// <summary>
        /// Gets the result types.
        /// </summary>
        /// <value>The result types.</value>
        internal List<Type> SearchResultTypes { get; set; } = new List<Type>();

        /// <summary>
        /// Sets the context to automatically add search result types.
        /// </summary>
        /// <param name="auto">if set to <c>true</c>, automatically add types.</param>
        /// <returns>Returns this instance for chaining.</returns>
        public EntityFrameworkSearchOptions WithAutoRegistration(bool auto = true)
        {
            this.AutoAddSearchResults = auto;

            return this;
        }

        /// <summary>
        /// Sets the connection string to use.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <returns>Returns this instance for chaining.</returns>
        public EntityFrameworkSearchOptions WithConnectionString(string connectionString)
        {
            Argument.NotNullOrWhiteSpace(connectionString, nameof(connectionString));

            this.ConnectionString = connectionString;

            return this;
        }

        /// <summary>
        /// Sets the options to force migrations.
        /// </summary>
        /// <param name="force">If set to <c>true</c> then force the migrations.</param>
        /// <returns>Returns this instance for chaining.</returns>
        public EntityFrameworkSearchOptions WithMigrations(bool force = true)
        {
            this.WithMigration = force;

            return this;
        }

        /// <summary>
        /// Ensures that the search context can use the search result types.  Sets the context to not automatically load types.
        /// </summary>
        /// <param name="types">The types to ensure.</param>
        /// <returns>Returns this instance for chaining.</returns>
        /// <exception cref="System.ArgumentException">All the specified types must be search results.</exception>
        public EntityFrameworkSearchOptions WithSearchResultTypes(params Type[] types)
        {
            Argument.NotNull(types, nameof(types));

            this.AutoAddSearchResults = false;

            foreach (var type in types)
            {
                if (!typeof(ISearchResult).IsAssignableFrom(type))
                {
                    throw new ArgumentException("All the specified types must be search results.");
                }
            }
            this.SearchResultTypes.AddRange(types);
            return this;
        }
    }
}
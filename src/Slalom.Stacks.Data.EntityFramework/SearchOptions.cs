using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Slalom.Stacks.Search;
using Slalom.Stacks.Validation;

namespace Slalom.Stacks.Data.EntityFramework
{
    /// <summary>
    /// Options for the Entity Framework Search module.
    /// </summary>
    public class SearchOptions
    {
        private readonly List<Type> _searchResults = new List<Type>();

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        internal string ConnectionString { get; private set; } = "Data Source=localhost;Initial Catalog=Stacks.Search;Integrated Security=True";

        /// <summary>
        /// Gets the result types.
        /// </summary>
        /// <value>The result types.</value>
        internal IEnumerable<Type> SearchResults => _searchResults.AsEnumerable();

        /// <summary>
        /// Sets the connection string to use.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <returns>Returns this instance for chaining.</returns>
        public SearchOptions WithConnectionString(string connectionString)
        {
            Argument.NotNullOrWhiteSpace(() => connectionString);

            this.ConnectionString = connectionString;

            return this;
        }

        /// <summary>
        /// Ensures that the search context can use the search result types.
        /// </summary>
        /// <param name="types">The types to ensure.</param>
        /// <returns>Returns this instance for chaining.</returns>
        /// <exception cref="System.ArgumentException">All the specified types must be search results.</exception>
        public SearchOptions EnsureSearchResults(params Type[] types)
        {
            Argument.NotNull(() => types);

            foreach (var type in types)
            {
                if (!typeof(ISearchResult).IsAssignableFrom(type))
                {
                    throw new ArgumentException("All the specified types must be search results.");
                }
            }
            _searchResults.AddRange(types);
            return this;
        }
    }
}
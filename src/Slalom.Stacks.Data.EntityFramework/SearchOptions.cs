using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slalom.Stacks.Search;

namespace Slalom.Stacks.Data.EntityFramework
{
    /// <summary>
    /// Options for the Entity Framework Search module.
    /// </summary>
    public class SearchOptions
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        internal string ConnectionString { get; private set; } = "Data Source=localhost;Initial Catalog=Stacks.Search;Integrated Security=True";

        private readonly List<Type> _resultTypes = new List<Type>();

        /// <summary>
        /// Gets the result types.
        /// </summary>
        /// <value>The result types.</value>
        internal IEnumerable<Type> ResultTypes => _resultTypes.AsEnumerable();

        /// <summary>
        /// Adds the search result to the loaded set.
        /// </summary>
        /// <typeparam name="T">The type of search result.</typeparam>
        /// <returns>The instance for chaining.</returns>
        public SearchOptions UseSearchResult<T>() where T : ISearchResult
        {
            _resultTypes.Add(typeof(T));
            return this;
        }
    }
}

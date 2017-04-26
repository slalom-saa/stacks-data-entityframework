using System.Collections.Generic;
using System.Reflection;
using Slalom.Stacks.Validation;

namespace Slalom.Stacks.EntityFramework
{
    /// <summary>
    /// Options for the Entity Framework Search module.
    /// </summary>
    public class EntityFrameworkOptions
    {
        internal string ConnectionString { get; set; } = "Data Source=localhost;Initial Catalog=Stacks.Search;Integrated Security=True;MultipleActiveResultSets=True";

        internal IEnumerable<Assembly> Assemblies { get; set; }   

        /// <summary>
        /// Sets the connection string to use.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        /// <returns>Returns this instance for chaining.</returns>
        public EntityFrameworkOptions WithConnectionString(string connectionString)
        {
            Argument.NotNullOrWhiteSpace(connectionString, nameof(connectionString));

            this.ConnectionString = connectionString;

            return this;
        }
    }
}
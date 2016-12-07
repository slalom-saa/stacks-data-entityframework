using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slalom.Stacks.Search;

namespace Slalom.Stacks.Data.EntityFramework
{
    public class EntityFrameworkSearchOptions
    {
        public string ConnectionString { get; set; } = "Data Source=localhost;Initial Catalog=Stacks.Search;Integrated Security=True";

        private readonly List<Type> _resultTypes = new List<Type>();

        internal IEnumerable<Type> ResultTypes => _resultTypes.AsEnumerable();

        public EntityFrameworkSearchOptions WithResult<T>() where T : ISearchResult
        {
            _resultTypes.Add(typeof(T));
            return this;
        }
    }
}

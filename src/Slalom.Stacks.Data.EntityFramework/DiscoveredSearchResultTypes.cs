using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using Slalom.Stacks.Reflection;
using Slalom.Stacks.Search;

namespace Slalom.Stacks.Data.EntityFramework
{
    internal class DiscoveredSearchResultTypes
    {
        public readonly Lazy<List<Type>> Items;

        public DiscoveredSearchResultTypes(IDiscoverTypes types)
        {
            Items = new Lazy<List<Type>>(() =>
            {
                return types.Find<ISearchResult>()
                            .Where(x => !x.GetTypeInfo().IsAbstract && !x.GetTypeInfo().IsInterface).ToList();
            });
        }
    }
}
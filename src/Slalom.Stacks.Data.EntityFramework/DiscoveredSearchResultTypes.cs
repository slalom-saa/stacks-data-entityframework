using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Slalom.Stacks.Reflection;
using Slalom.Stacks.Search;

namespace Slalom.Stacks.Data.EntityFramework
{
    internal class DiscoveredSearchResultTypes
    {
        public DiscoveredSearchResultTypes(IDiscoverTypes types)
        {
            Items = new Lazy<List<Type>>(() =>
            {
                return types.Find<ISearchResult>()
                            .Where(x => !IntrospectionExtensions.GetTypeInfo(x).IsAbstract && !IntrospectionExtensions.GetTypeInfo(x).IsInterface).ToList();
            });
        }

        public readonly Lazy<List<Type>> Items;
    }
}
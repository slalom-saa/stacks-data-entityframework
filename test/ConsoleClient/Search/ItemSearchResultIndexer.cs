using System.Threading.Tasks;
using Slalom.Stacks.Search;

namespace ConsoleClient
{
    public class ItemSearchResultIndexer : SearchIndexer<ItemSearchResult>
    {
        public ItemSearchResultIndexer(ISearchContext context)
            : base(context)
        {
        }

        public override Task AddAsync(ItemSearchResult[] instances)
        {
            return base.AddAsync(instances);
        }

        public override Task RebuildIndexAsync()
        {
            return base.RebuildIndexAsync();
        }
    }
}
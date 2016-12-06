using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Slalom.Stacks.Search;

namespace Slalom.Stacks.Data.EntityFramework
{
    public class EntityFrameworkSearchContext : ISearchContext
    {
        private readonly DbContext _context;

        public EntityFrameworkSearchContext(DbContext context)
        {
            _context = context;
        }

        public Task AddAsync<TSearchResult>(TSearchResult[] instances) where TSearchResult : class
        {
            _context.Set<TSearchResult>().AddRange(instances);
            return _context.SaveChangesAsync();
        }

        public Task ClearAsync<TSearchResult>() where TSearchResult : class
        {
            _context.Set<TSearchResult>().RemoveRange(_context.Set<TSearchResult>());

            return _context.SaveChangesAsync();
        }

        public IQueryable<TSearchResult> CreateQuery<TSearchResult>() where TSearchResult : class
        {
            return _context.Set<TSearchResult>().AsNoTracking();
        }

        public Task DeleteAsync<TSearchResult>(Expression<Func<TSearchResult, bool>> predicate) where TSearchResult : class
        {
            _context.Set<TSearchResult>().RemoveRange(_context.Set<TSearchResult>().Where(predicate));

            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync<TSearchResult>(TSearchResult[] instances) where TSearchResult : class
        {
            _context.Set<TSearchResult>().RemoveRange(instances);
            return _context.SaveChangesAsync();
        }

        public Task<TSearchResult> FindAsync<TSearchResult>(int id) where TSearchResult : class
        {
            return _context.Set<TSearchResult>().FindAsync(id);
        }

        public Task UpdateAsync<TSearchResult>(TSearchResult[] instances) where TSearchResult : class
        {
            _context.Set<TSearchResult>().UpdateRange(instances);

            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync<TSearchResult>(Expression<Func<TSearchResult, bool>> predicate, Expression<Func<Type, Type>> expression) where TSearchResult : class
        {
            throw new NotImplementedException();
        }
    }
}

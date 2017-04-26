using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Slalom.Stacks;
using Slalom.Stacks.EntityFramework;
using Slalom.Stacks.Search;
using Slalom.Stacks.Services.Messaging;
using Autofac;

namespace ConsoleClient
{
    public class User
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }
    }

    public class UserSearchResult : ISearchResult
    {
        public int Id { get; set; }

        public bool Crawled { get; set; }

        public string FirstName { get; set; }

        public string UserId { get; set; }
    }

    public class UserSearchIndex : SearchIndex<UserSearchResult>
    {
        public UserSearchIndex(ISearchContext context) : base(context)
        {
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            using (var stack = new ConsoleStack())
            {
                stack.UseEntityFrameworkSearch();

                stack.Use(e =>
                {
                    e.RegisterType<UserSearchIndex>().AsSelf().AsImplementedInterfaces().AllPropertiesAutowired();
                });

                stack.Search.AddAsync(new UserSearchResult {FirstName = "a"}).Wait();
            }
        }
    }
}
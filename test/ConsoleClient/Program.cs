﻿using System;
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
using Slalom.Stacks.Domain;
using Slalom.Stacks.Validation;

namespace ConsoleClient
{
    public class Description : ConceptAs<string>
    {
        public override IEnumerable<ValidationError> Validate()
        {
            yield break;
        }
    }

    public class User : AggregateRoot
    {
        public string FirstName { get; set; }

        public Description Description { get; set; } = new Description();
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
                stack.UseEntityFramework();
                stack.UseEntityFrameworkSearch(e =>
                {
                    e.WithConnectionString("Data Source=localhost;Initial Catalog=Stacks.Search;Integrated Security=True;MultipleActiveResultSets=True");
                });

                stack.Domain.Add(new User()).Wait();


                //stack.UseEntityFrameworkSearch();

                //stack.Use(e =>
                //{
                //    e.RegisterType<UserSearchIndex>().AsSelf().AsImplementedInterfaces().AllPropertiesAutowired();
                //});

                stack.Search.AddAsync(new UserSearchResult { FirstName = "a" }).Wait();
            }
        }
    }
}
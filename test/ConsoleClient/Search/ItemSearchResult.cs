using System;
using System.Linq;
using Slalom.Stacks.Search;

namespace ConsoleClient
{
    public class ItemSearchResult : ISearchResult
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public ItemSearchResult()
        {
            Name = RandomString(20);
            Content = RandomString(300);
        }
    }
}
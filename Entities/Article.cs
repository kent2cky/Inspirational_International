using System;
using System.Collections.Generic;

namespace Inspiration_International.Entities
{
    public class Article
    {
        public int ArticleID { get; set; }
        public string ArticleBody { get; set; }
        public string Title { get; set; }
        public DateTime DateTimePosted { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public string Author { get; set; }
    }
}
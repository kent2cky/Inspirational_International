using System;

namespace Inspiration_International.Entities
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string Name { get; set; }
        public string CommentBody { get; set; }
        public int ArticleID { get; set; }
        public DateTime DateTimePosted { get; set; }
    }
}
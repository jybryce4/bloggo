using System;

namespace Bloggo.Models 
{
    public class Post
    {
        public int PostID { get; set; }

        public string Username { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Content { get; set; }

        public DateTime DatePosted { get; set; } // YYYY-MM-DD

        public int Reblogs { get; set; }

        public int Upvotes { get; set; }
    }
}
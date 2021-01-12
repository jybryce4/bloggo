namespace Bloggo.Models 
{
    public class Post
    {
        public string PostID { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Content { get; set; }

        public string DatePosted { get; set; } // YYYY-MM-DD

        public int Reblogs { get; set; }

        public int Upvotes { get; set; }
    }
}
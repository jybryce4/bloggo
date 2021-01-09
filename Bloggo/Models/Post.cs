namespace Bloggo.Models 
{
    public class Post
    {
        public string PostId { get; set; }

        public string UserName { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string DatePosted { get; set; } // YYYY-MM-DD
    }
}
namespace Bloggo.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password{ get; set; } // field used to set password
        public string PasswordHash{ get; set; } // hash from db
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
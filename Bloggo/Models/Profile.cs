namespace Bloggo.Models
{
    public class Profile
    {
        public int ProfileID { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfileURL { get; set; }

        public string ProfileImageURL { get; set; }

        public string CoverImageURL { get; set; }

        public string UserBio { get; set; }

        public string Website { get; set; }

        public int NumFollowers { get; set; }

        public int Coins { get; set; }
    }
}
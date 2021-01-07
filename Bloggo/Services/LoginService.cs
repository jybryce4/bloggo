using Bloggo.Models;

namespace Bloggo.Services
{
    public class LoginService
    {
        private UserDatabaseService _userDatabaseService;
        public User User { get; private set; }

        public LoginService(UserDatabaseService userDatabaseService)
        {
            _userDatabaseService = userDatabaseService;
        }

        public void Login(Login model)
        {
            
        }
    }
}
using Bloggo.Models;
using System;
using System.Threading.Tasks;
using Bloggo.Services.Database;

namespace Bloggo.Services
{
    public static class AccountService
    {
        // public static IDatabaseService<User, User, string> _userDatabaseService = new UserDatabaseService();
        
        public static bool loggedIn = false;

        public static User User { get; private set; }

        public static IDatabaseService<User, User, string> userDatabaseService { get; set; }
        public static IDatabaseService<Profile, User, string> profileDatabaseService { get; set; }
        public static IDatabaseService<Post, Post, int> postDatabaseService { get; set; }


        public static async Task Login(
            Login model, 
            string passwordHash, 
            IDatabaseService<User, User, string> userDB, 
            IDatabaseService<Profile, User, string> profileDB,
            IDatabaseService<Post, Post, int> postDB
            )
        {
            if (VerifyPassword(model.Password, passwordHash) && model != null)
            {   
                userDatabaseService = userDB;
                User = await userDatabaseService.GetItem(model.Username);

                profileDatabaseService = profileDB;
                postDatabaseService = postDB;
                

                Console.WriteLine($"{User.Username} logged in.");
               
                loggedIn = true;

            }
            else 
            {
                await userDatabaseService.CloseConnection();
                await profileDatabaseService.CloseConnection();
                await postDatabaseService.CloseConnection();
                throw new Exception("Invalid login information");
            }
            
        }

        public static async Task Logout()
        {
            User = null;
            loggedIn = false;
            await userDatabaseService.CloseConnection();
            await profileDatabaseService.CloseConnection();
            await postDatabaseService.CloseConnection();
        }

        private static bool VerifyPassword(string enteredPassword, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, passwordHash);
        }
    }
}
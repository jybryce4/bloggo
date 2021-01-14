using Bloggo.Models;
using System;
using Bloggo.Services.Database;

namespace Bloggo.Services
{
    public static class AccountService
    {
        public static IDatabaseService<User, User> _userDatabaseService = new UserDatabaseService();
        
        public static bool loggedIn = false;

        public static User User { get; private set; }


        public static void Login(Login model, string passwordHash, IDatabaseService<User, User> db)
        {
            if (VerifyPassword(model.Password, passwordHash) && model != null)
            {
                _userDatabaseService = db;
                User = _userDatabaseService.GetItem(model.Username);
                
                Console.WriteLine($"{User.Username} logged in.");
               
                loggedIn = true;

                db.CloseConnection();
            }
            else 
            {
                throw new Exception("Invalid login information");
            }
            
        }

        public static void Logout()
        {
            User = null;
            loggedIn = false;
            _userDatabaseService.CloseConnection();
        }

        private static bool VerifyPassword(string enteredPassword, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, passwordHash);
        }
    }
}
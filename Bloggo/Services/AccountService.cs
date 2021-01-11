using Bloggo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bloggo.Services
{
    public static class AccountService
    {
        // private static UserDatabaseService _userDatabaseService = new UserDatabaseService();
        
        public static bool loggedIn = false;
        public static User User { get; private set; }

        // public AccountService(UserDatabaseService userDatabaseService)
        // {
        //     _userDatabaseService = userDatabaseService;
        //     //_httpHandler = new HttpHandler(_userDatabaseService);
        // }

        public static void Login(Login model, string passwordHash)
        {
            //User = await _httpHandler.Post<User>("/account/authenticate", model);
            if (VerifyPassword(model.Password, passwordHash) && model != null)
            {
                UserDatabaseService.OpenConnection();
                User = UserDatabaseService.GetUser(model.Username);
                Console.WriteLine($"{User.Username} logged in.");
                loggedIn = true;
                UserDatabaseService.CloseConnection();
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
        }

        private static bool VerifyPassword(string enteredPassword, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, passwordHash);
        }
    }
}
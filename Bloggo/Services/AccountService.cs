using Bloggo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bloggo.Services
{
    public class AccountService
    {
        private UserDatabaseService _userDatabaseService;
        
        public static bool loggedIn = false;
        public static User User { get; private set; }

        public AccountService(UserDatabaseService userDatabaseService)
        {
            _userDatabaseService = userDatabaseService;
            //_httpHandler = new HttpHandler(_userDatabaseService);
        }

        public void Login(Login model, string passwordHash)
        {
            //User = await _httpHandler.Post<User>("/account/authenticate", model);
            if (VerifyPassword(model.Password, passwordHash))
            {
                User = _userDatabaseService.GetUser(model.Username);
                Console.WriteLine($"{User.Username} logged in.");
                loggedIn = true;
            }
            else 
            {

            }
            
        }

        public static void Logout()
        {
            User = null;
            loggedIn = false;
        }

        private bool VerifyPassword(string enteredPassword, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, passwordHash);
        }
    }
}
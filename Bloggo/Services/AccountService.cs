using Bloggo.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bloggo.Services
{
    public class AccountService
    {
        private UserDatabaseService _userDatabaseService;
        // private AlertService _alertService = new AlertService();
        public User User { get; private set; }

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
                // _alertService.Success("Successfully logged in.");
            }
            else 
            {

            }
            
        }

        public void Logout()
        {
            User = null;
        }

        private bool VerifyPassword(string enteredPassword, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, passwordHash);
        }
    }
}
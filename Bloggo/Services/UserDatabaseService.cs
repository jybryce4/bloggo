using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using Bloggo.Models;
using System.Collections.Generic;

namespace Bloggo.Services
{

    public class UserDatabaseService
    {
        private SqlConnection connection;
        
        public UserDatabaseService() 
        {
            // Replace BLOGGO_DB with the environment variable of your connection string
            connection = new SqlConnection(Environment.GetEnvironmentVariable("BLOGGO_DB"));
        }

        public User GetUser(string username)
        {
            connection.Open();

            // we need to check to make sure the user exists
            string checkUser = $"SELECT count(*) FROM [dbo].[Users] WHERE UserName={username}";
            SqlCommand cmd = new SqlCommand(checkUser, connection);
            try {
                int tmp = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                if (tmp != 1)
                {
                    Console.WriteLine("Database error.");
                    connection.Close();
                    return default; // null
                }
                else 
                {   
                    User user = null;
                    string selectUser = $"SELECT * FROM [dbo].[Users] WHERE UserName={username}";
                    SqlCommand query = new SqlCommand(selectUser, connection);
                    
                    var userDataReader = query.ExecuteReader();
                    
                    // reading the data back into the frontend
                    user.Id = userDataReader.GetFieldValue<string>(0);
                    user.Username = userDataReader.GetFieldValue<string>(1);
                    user.PasswordHash = userDataReader.GetFieldValue<string>(2);
                    user.FirstName = userDataReader.GetFieldValue<string>(3);
                    user.LastName = userDataReader.GetFieldValue<string>(4);
                    user.ProfileImageURL = userDataReader.GetFieldValue<string>(5);
                    user.CoverImageURL = userDataReader.GetFieldValue<string>(6);
                    user.Email = userDataReader.GetFieldValue<string>(7);
                    user.Bio = userDataReader.GetFieldValue<string>(8);
                    
                    connection.Close();

                    return user;

                }
            }
            catch (SqlException se)
            {
                Console.WriteLine("Error: " + se.ToString());
            }
            
            return default;
        }

        public async Task<IList<string>> GetAllRows()
        {
            IList<string> userList = null;
            string query = "SELECT * FROM [dbo].[Users]";
            SqlCommand cmd = new SqlCommand(query, connection);
            var sqlOutput = await cmd.ExecuteReaderAsync();
            
            foreach (var item in sqlOutput)
            {
                userList.Add(item.ToString());
            }

            return userList;
        }

        public async Task CreateRow(User user)
        {
            PasswordHasher pwh = new PasswordHasher(user.Password); // encrypt the password

            string sql = "INSERT INTO [dbo].[Users] ([UserName], [PasswordHash], [FirstName], [LastName], [Email])" 
                        + $"VALUES ('{user.Username}', '{pwh.GetHash()}', '{user.FirstName}', '{user.LastName}', '{user.Email}')";

            SqlCommand cmd = new SqlCommand(sql, connection);

            await connection.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
            
            await cmd.DisposeAsync();
            await connection.CloseAsync();
            
        }
        
        public async Task EditRow(string pk, string columnName, string value)
        {
            string sql = $"UPDATE [dbo].[Users] set {columnName}='{value}' WHERE UserID={pk}";
            
            SqlCommand cmd = new SqlCommand(sql, connection);

            await cmd.ExecuteNonQueryAsync();
            
            await cmd.DisposeAsync();
            await connection.CloseAsync();

        }

        public async Task DeleteRow(string pk)
        {
            string sql = $"DELETE [dbo].[Users] WHERE UserID={pk}";

            SqlCommand cmd = new SqlCommand(sql, connection);

            await cmd.ExecuteNonQueryAsync();

            await cmd.DisposeAsync();
            await connection.CloseAsync();
        }
        
    }
}
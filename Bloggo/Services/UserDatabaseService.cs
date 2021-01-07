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
            string checkUser = $"SELECT count(*) FROM [dbo].[Users] WHERE UserName='{username}'";
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
                    User user = new User();
                    string selectUser = $"SELECT * FROM [dbo].[Users] WHERE UserName='{username}'";
                    SqlCommand sql = new SqlCommand(selectUser, connection);
                    
                    // reading the data back into the frontend
                    using (SqlDataReader reader = sql.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Id = reader["UserID"].ToString();
                            user.Username = reader["UserName"].ToString();
                            user.PasswordHash = reader["PasswordHash"].ToString();
                            user.FirstName = reader["FirstName"].ToString();
                            user.LastName = reader["LastName"].ToString();
                            user.ProfileImageURL = reader["ProfileImageURL"].ToString();
                            user.CoverImageURL = reader["CoverImageURL"].ToString();
                            user.Email = reader["Email"].ToString();
                            user.Bio = reader["Bio"].ToString();
                        }
                    }

                    
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
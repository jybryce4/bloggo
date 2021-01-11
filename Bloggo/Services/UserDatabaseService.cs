using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Bloggo.Models;
using Bloggo.Helpers;
using SettingsReader.Readers;


namespace Bloggo.Services
{
    public static class UserDatabaseService
    {
        
        // To do: make this more secure!!!!
        private static string connectionString = "Data Source=tcp:bloggodev.database.windows.net,1433;Initial Catalog=Bloggo-Dev-DB;User Id=bloggodba@bloggodev;Password=Catruya#4961";
        
        private static SqlConnection connection = new SqlConnection(connectionString);
        // public UserDatabaseService() 
        // {
        //     // Replace BLOGGO_DB with the environment variable of your connection string
        //     // connection = new SqlConnection(Environment.GetEnvironmentVariable("BLOGGO_DB"));
        //     connection = new SqlConnection(connectionString);
        // }

        public static User GetUser(string username)
        {
            connection.Open();

            // we need to check to make sure the user exists
            //string checkUser = $"SELECT count(*) FROM [dbo].[Users] WHERE UserName='{username}'";
            // SqlCommand cmd = new SqlCommand(checkUser, connection);
            try {
                // int tmp = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                // if (tmp != 1)
                // {
                //     Console.WriteLine("Database error.");
                //     connection.Close();
                //     return default; // null
                // }
                // else 
                // {   
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
                            user.Birthday = reader["Birthday"].ToString();
                            user.Email = reader["Email"].ToString();
                        }
                    }

                    
                    connection.Close();

                    return user;

                
            }
            catch (SqlException se)
            {
                Console.WriteLine("Error: " + se.ToString());
            }
            
            return default;
        }

        public static async Task<IList<string>> GetAllRows()
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

        // This creates a new user (registration)
        public static async Task CreateRow(User user)
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
        
        public static async Task EditRow(string pk, string columnName, string value)
        {
            string sql = $"UPDATE [dbo].[Users] set {columnName}='{value}' WHERE UserName={pk}";
            
            SqlCommand cmd = new SqlCommand(sql, connection);

            await cmd.ExecuteNonQueryAsync();
            
            await cmd.DisposeAsync();
            await connection.CloseAsync();

        }

        public static async Task DeleteRow(string pk)
        {
            string sql = $"DELETE [dbo].[Users] WHERE UserID={pk}";

            SqlCommand cmd = new SqlCommand(sql, connection);

            await cmd.ExecuteNonQueryAsync();

            await cmd.DisposeAsync();
            await connection.CloseAsync();
        }
        
    }
}
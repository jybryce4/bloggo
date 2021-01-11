using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Bloggo.Models;


namespace Bloggo.Services
{
    public static class UserDatabaseService
    {
        
        // To do: make this more secure!!!!
        private static string connectionString = "Server=tcp:bloggodev.database.windows.net,1433;Initial Catalog=Bloggo-Dev-DB;Persist Security Info=False;User ID=bloggodba;Password=Sunanoken@1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        private static SqlConnection connection = new SqlConnection(connectionString);
        // public UserDatabaseService() 
        // {
        //     // Replace BLOGGO_DB with the environment variable of your connection string
        //     // connection = new SqlConnection(Environment.GetEnvironmentVariable("BLOGGO_DB"));
        //     connection = new SqlConnection(connectionString);
        // }

        public static void OpenConnection()
        {
            connection.Open();
        }

        public static void CloseConnection()
        {
            connection.Close();
        }

        public static User GetUser(string username)
        {

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
                    string selectUser = $"SELECT * FROM [dbo].[User] WHERE UserName='{username}'";
                    SqlCommand sql = new SqlCommand(selectUser, connection);
                    
                    // reading the data back into the frontend
                    using (SqlDataReader reader = sql.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Username = reader["UserName"].ToString();
                            user.PasswordHash = reader["PasswordHash"].ToString();
                            user.FirstName = reader["FirstName"].ToString();
                            user.LastName = reader["LastName"].ToString();
                            user.Email = reader["Email"].ToString();
                        }
                    }

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
            string query = "SELECT * FROM [dbo].[User]";
            SqlCommand cmd = new SqlCommand(query, connection);
            var sqlOutput = await cmd.ExecuteReaderAsync();
            
            foreach (var item in sqlOutput)
            {
                userList.Add(item.ToString());
            }

            return userList;
        }

        // This creates a new user (registration)
        public static void CreateRow(User user)
        {
            PasswordHasher pwh = new PasswordHasher(user.Password); // encrypt the password

            string sql = "INSERT INTO [dbo].[User] ([UserName], [PasswordHash], [FirstName], [LastName], [Email])" 
                        + $"VALUES ('{user.Username}', '{pwh.GetHash()}', '{user.FirstName}', '{user.LastName}', '{user.Email}')";

            SqlCommand cmd = new SqlCommand(sql, connection);

            cmd.ExecuteNonQuery();
            
            cmd.Dispose();
            
            
        }
        
        public static void EditRow(string key, string columnName, string value)
        {
            string sql = $"UPDATE [dbo].[User] SET {columnName}='{value}' WHERE UserName={key}";
            
            SqlCommand cmd = new SqlCommand(sql, connection);

            cmd.ExecuteNonQuery();
            
            cmd.Dispose();

        }

        // public static async Task DeleteRow(string pk)
        // {
        //     string sql = $"DELETE [dbo].[Users] WHERE UserID={pk}";

        //     SqlCommand cmd = new SqlCommand(sql, connection);

        //     await cmd.ExecuteNonQueryAsync();

        //     await cmd.DisposeAsync();
        //     await connection.CloseAsync();
        // }
        
    }
}
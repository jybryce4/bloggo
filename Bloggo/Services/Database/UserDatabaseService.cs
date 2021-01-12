using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using Bloggo.Models;


namespace Bloggo.Services.Database
{
    public class UserDatabaseService : IDatabaseService<User>
    {
        static string ConnectionString = Environment.GetEnvironmentVariable("BLOGGO_DB");
        
        SqlConnection Connection = new SqlConnection(ConnectionString);
        // public UserDatabaseService() 
        // {
        //     // Replace BLOGGO_DB with the environment variable of your connection string
        //     // connection = new SqlConnection(Environment.GetEnvironmentVariable("BLOGGO_DB"));
        //     connection = new SqlConnection(connectionString);
        // }

        public void OpenConnection()
        {
            Connection.Open();
        }

        public void CloseConnection()
        {
            Connection.Close();
        }

        public User GetItem(string primaryKey)
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
                    string selectUser = $"SELECT * FROM [dbo].[User] WHERE UserName='{primaryKey}'";
                    SqlCommand sql = new SqlCommand(selectUser, Connection);
                    
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

        public IList<User> GetAllRows()
        {
            IList<User> userList = null;
            string query = "SELECT * FROM [dbo].[User]";
            SqlCommand cmd = new SqlCommand(query, Connection);
            var sqlOutput = cmd.ExecuteReader();
            
            // To do

            return userList;
        }

        // This creates a new user (registration)
        public void CreateRow(User user)
        {
            PasswordHasher pwh = new PasswordHasher(user.Password); // encrypt the password

            string sql = "INSERT INTO [dbo].[User] ([UserName], [PasswordHash], [FirstName], [LastName], [Email])" 
                        + $"VALUES ('{user.Username}', '{pwh.GetHash()}', '{user.FirstName}', '{user.LastName}', '{user.Email}')";

            SqlCommand cmd = new SqlCommand(sql, Connection);

            cmd.ExecuteNonQuery();
            
            cmd.Dispose();
            
            
        }
        
        public void EditRow(string primaryKey, string columnName, string value)
        {
            string sql = $"UPDATE [dbo].[User] SET {columnName}='{value}' WHERE UserName={primaryKey}";
            
            SqlCommand cmd = new SqlCommand(sql, Connection);

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
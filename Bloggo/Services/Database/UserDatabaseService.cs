using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bloggo.Models;
using Bloggo.Helpers;


namespace Bloggo.Services.Database
{
    public class UserDatabaseService : IDatabaseService<User, User, string>
    {
        static string ConnectionString = Environment.GetEnvironmentVariable("BLOGGO_DB");
        
        SqlConnection Connection = new SqlConnection(ConnectionString);
    
        // public UserDatabaseService()
        // {
        //     OpenConnection();
        // }

        public async Task OpenConnection()
        {
            await Connection.OpenAsync();
        }

        public async Task CloseConnection()
        {
            await Connection.CloseAsync();
        }

        public async Task<User> GetItem(string primaryKey)
        {

            // we need to check to make sure the user exists
            string checkUser = $"SELECT count(*) FROM [dbo].[User] WHERE UserName='{primaryKey}'";
            SqlCommand cmd = new SqlCommand(checkUser, Connection);
            try {
                int tmp = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                if (tmp != 1)
                {
                    Console.WriteLine("Database error.");
                    
                    return default; // null
                }
                else 
                {   
                    User user = new User();
                    string selectUser = $"SELECT * FROM [dbo].[User] WHERE UserName='{primaryKey}'";
                    SqlCommand sql = new SqlCommand(selectUser, Connection);
                    
                    // reading the data back into the frontend
                    using (SqlDataReader reader = await sql.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
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
            }
            catch (SqlException se)
            {
                Console.WriteLine("Error: " + se.ToString());
            }

            
            return default;
        }

        public async Task<IList<User>> GetAllRows(string value = null)
        {
            IList<User> userList = null;
            string query = "SELECT * FROM [dbo].[User]";
            SqlCommand cmd = new SqlCommand(query, Connection);
            var sqlOutput = cmd.ExecuteReader();
            
            // To do

            return userList;
        }

        // This creates a new user (registration)
        public async Task CreateRow(User user)
        {
            PasswordHasher pwh = new PasswordHasher(user.Password); // encrypt the password

            string sql = "INSERT INTO [dbo].[User] ([UserName], [PasswordHash], [FirstName], [LastName], [Email])" 
                        + $"VALUES ('{user.Username}', '{pwh.GetHash()}', '{user.FirstName}', '{user.LastName}', '{user.Email}')";

            SqlCommand cmd = new SqlCommand(sql, Connection);

            await cmd.ExecuteNonQueryAsync();
            
            await cmd.DisposeAsync();
            
            
        }
        
        public async Task EditRow(string primaryKey, string columnName, string value)
        {
            string sql = $"UPDATE [dbo].[User] SET {columnName}='{value}' WHERE UserName='{primaryKey}'";
            
            SqlCommand cmd = new SqlCommand(sql, Connection);

            await cmd.ExecuteNonQueryAsync();
            
            await cmd.DisposeAsync();

        }

        public async Task DeleteRow(string primaryKey)
        {
            string sql = $"DELETE [dbo].[User] WHERE UserName='{primaryKey}'";

            SqlCommand cmd = new SqlCommand(sql, Connection);

            await cmd.ExecuteNonQueryAsync();
            await cmd.DisposeAsync();

        }
        
    }
}
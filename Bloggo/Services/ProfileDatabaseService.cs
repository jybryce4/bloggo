using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bloggo.Models;
using Microsoft.Data.SqlClient;

namespace Bloggo.Services
{
    public class ProfileDatabaseService
    {
        private SqlConnection connection;

        public ProfileDatabaseService()
        {
            // SECURE THIS
            connection =
                new SqlConnection("Data Source=tcp:bloggodev.database.windows.net,1433;Initial Catalog=Bloggo-Dev-DB;User Id=bloggodba@bloggodev;Password=Catruya#4961");
        }

        public Profile GetProfile(string username)
        {
            connection.Open();

            Profile profile = new Profile();
            string selectProfile =
                $"SELECT * FROM [dbo].[Profiles] WHERE UserName='{username}'";
            SqlCommand sql = new SqlCommand(selectProfile, connection);

            // reading the data back into the frontend
            using (SqlDataReader reader = sql.ExecuteReader())
            {
                while (reader.Read())
                {
                    profile.ProfileID = reader["ProfileID"].ToString();
                    profile.Username = reader["UserName"].ToString();
                    profile.FirstName = reader["FirstName"].ToString();
                    profile.LastName = reader["LastName"].ToString();
                    profile.ProfileURL = reader["ProfileURL"].ToString();
                    profile.ProfileImageURL = reader["ProfileImageURL"].ToString();
                    profile.CoverImageURL = reader["CoverImageURL"].ToString();
                    profile.UserBio = reader["UserBio"].ToString();
                    profile.Website = reader["Website"].ToString();
                    profile.NumFollowers = reader["NumFollowers"].ToString();
                    profile.Coins = reader["Coins"].ToString();
                }
            }

            connection.Close();

            return profile;
        }

        public async Task<IList<string>> GetAllRows()
        {
            IList<string> profileList = null;
            string query = "SELECT * FROM [dbo].[Profiles]";
            SqlCommand cmd = new SqlCommand(query, connection);
            var sqlOutput = await cmd.ExecuteReaderAsync();
            
            foreach (var item in sqlOutput)
            {
                profileList.Add(item.ToString());
            }

            return profileList;
        }

        // Creates a new profile in the database when the user registers
        public async Task CreateRow(User user)
        {

            string sql = "INSERT INTO [dbo].[Profiles] ([UserName], [FirstName], [LastName], [ProfileURL], [ProfileImageURL], [CoverImageURL], [UserBio], [Website], [NumFollowers], [Coins])" 
                        + $"VALUES ('{user.Username}', '{user.FirstName}', '{user.LastName}', 'users/{user.Username}', 'img/blank_profile.jpg', 'img/blank_cover.jpg', 'This person might be shy (no about me found!)', '<no website given>', 0, 0)";

            SqlCommand cmd = new SqlCommand(sql, connection);

            await connection.OpenAsync();

            await cmd.ExecuteNonQueryAsync();
            
            await cmd.DisposeAsync();
            await connection.CloseAsync();
            
        }

        public async Task EditRow(string key, string columnName, string value)
        {
            string sql = $"UPDATE [dbo].[Profiles] SET {columnName}='{value}' WHERE UserName={key}";
            
            SqlCommand cmd = new SqlCommand(sql, connection);

            await cmd.ExecuteNonQueryAsync();
            
            await cmd.DisposeAsync();
            await connection.CloseAsync();

        }
    }
}

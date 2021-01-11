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

        // SECURE THIS
        private string connectionString = "Server=tcp:bloggodev.database.windows.net,1433;Initial Catalog=Bloggo-Dev-DB;Persist Security Info=False;User ID=bloggodba;Password=Sunanoken@1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public ProfileDatabaseService()
        {
            connection = new SqlConnection(connectionString);
            OpenConnection();
        }

        public void OpenConnection()
        {
            connection.Open();
        }

        public void CloseConnection()
        {
            connection.Close();
        }

        public Profile GetProfile(string username)
        {
            

            Profile profile = new Profile();
            string selectProfile =
                $"SELECT * FROM [dbo].[Profile] WHERE UserName='{username}'";
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

            //connection.Close();

            return profile;
        }

        public IList<Profile> GetAllRows()
        {
            //connection.Open();

            IList<Profile> profileList = new List<Profile>();
           
            string query = "SELECT * FROM [dbo].[Profile]";
            SqlCommand cmd = new SqlCommand(query, connection);
            //var reader = cmd.ExecuteReader();
            
            
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Profile profile = new Profile();
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

                    //profileList.Add(GetProfile(reader["UserName"].ToString()));
                    profileList.Add(profile);
                }
                   
            }
            
            // connection.Close();
                     
            return profileList;
            
        }

        // Creates a new profile in the database when the user registers
        public void CreateRow(User user)
        {
            //connection.Open();

            string sql = "INSERT INTO [dbo].[Profile] ([UserName], [FirstName], [LastName], [ProfileURL], [ProfileImageURL], [CoverImageURL], [UserBio], [Website], [NumFollowers], [Coins])" 
                        + $"VALUES ('{user.Username}', '{user.FirstName}', '{user.LastName}', 'users/{user.Username}', 'img/blank_profile.jpg', 'img/blank_cover.jpg', 'This person might be shy (no about me found!)', '<no website given>', 0, 0)";

            SqlCommand cmd = new SqlCommand(sql, connection);

            cmd.ExecuteNonQuery();
            
            cmd.Dispose();
            //connection.Close();
            
            
        }

        public void EditRow(string key, string columnName, string value)
        {
            string sql = $"UPDATE [dbo].[Profile] SET {columnName}='{value}' WHERE UserName={key}";
            
            SqlCommand cmd = new SqlCommand(sql, connection);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            //connection.Close();

        }

    }
}

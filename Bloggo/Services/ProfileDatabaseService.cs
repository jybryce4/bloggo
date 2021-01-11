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
            connection = new SqlConnection("Data Source=tcp:bloggodev.database.windows.net,1433;Initial Catalog=Bloggo-Dev-DB;User Id=bloggodba@bloggodev;Password=Catruya#4961");
            
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

            //connection.Close();

            return profile;
        }

        public IList<Profile> GetAllRows()
        {
            //connection.Open();

            IList<Profile> profileList = new List<Profile>();
           
            string query = "SELECT * FROM [dbo].[Profiles]";
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

            string sql = "INSERT INTO [dbo].[Profiles] ([UserName], [FirstName], [LastName], [ProfileURL], [ProfileImageURL], [CoverImageURL], [UserBio], [Website], [NumFollowers], [Coins])" 
                        + $"VALUES ('{user.Username}', '{user.FirstName}', '{user.LastName}', 'users/{user.Username}', 'img/blank_profile.jpg', 'img/blank_cover.jpg', 'This person might be shy (no about me found!)', '<no website given>', 0, 0)";

            SqlCommand cmd = new SqlCommand(sql, connection);

            cmd.ExecuteNonQuery();
            
            cmd.Dispose();
            //connection.Close();
            
            
        }

        public void EditRow(string key, string columnName, string value)
        {
            string sql = $"UPDATE [dbo].[Profiles] SET {columnName}='{value}' WHERE UserName={key}";
            
            SqlCommand cmd = new SqlCommand(sql, connection);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            //connection.Close();

        }

    }
}

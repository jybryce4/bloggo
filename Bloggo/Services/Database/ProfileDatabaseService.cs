using System;
using System.Collections.Generic;
using Bloggo.Models;
using Microsoft.Data.SqlClient;

namespace Bloggo.Services.Database
{
    public class ProfileDatabaseService : IDatabaseService<Profile, User, string>
    {
        static string ConnectionString = Environment.GetEnvironmentVariable("BLOGGO_DB");
        SqlConnection Connection = new SqlConnection(ConnectionString);

        public ProfileDatabaseService()
        {
            OpenConnection();
        }
        public void OpenConnection()
        {
            Connection.Open();
        }

        public void CloseConnection()
        {
            Connection.Close();
        }

        public Profile GetItem(string primaryKey)
        {
            Profile profile = new Profile();
            string selectProfile =
                $"SELECT * FROM [dbo].[Profile] WHERE UserName='{primaryKey}'";
            SqlCommand sql = new SqlCommand(selectProfile, Connection);

            // reading the data back into the frontend
            using (SqlDataReader reader = sql.ExecuteReader())
            {
                while (reader.Read())
                {
                    profile.ProfileID = Convert.ToInt32(reader["ProfileID"].ToString());
                    profile.Username = reader["UserName"].ToString();
                    profile.FirstName = reader["FirstName"].ToString();
                    profile.LastName = reader["LastName"].ToString();
                    profile.ProfileURL = reader["ProfileURL"].ToString();
                    profile.ProfileImageURL = reader["ProfileImageURL"].ToString();
                    profile.CoverImageURL = reader["CoverImageURL"].ToString();
                    profile.UserBio = reader["UserBio"].ToString();
                    profile.Website = reader["Website"].ToString();
                    profile.NumFollowers = Convert.ToInt32(reader["NumFollowers"].ToString());
                    profile.Coins = Convert.ToInt32(reader["Coins"].ToString());
                }
            }

            //connection.Close();

            return profile;
        }

        public IList<Profile> GetAllRows(string value = null)
        {
            //connection.Open();

            IList<Profile> profileList = new List<Profile>();
           
            string query = "SELECT * FROM [dbo].[Profile]";
            SqlCommand cmd = new SqlCommand(query, Connection);
            //var reader = cmd.ExecuteReader();
            
            
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Profile profile = new Profile();
                    profile.ProfileID = Convert.ToInt32(reader["ProfileID"].ToString());
                    profile.Username = reader["UserName"].ToString();
                    profile.FirstName = reader["FirstName"].ToString();
                    profile.LastName = reader["LastName"].ToString();
                    profile.ProfileURL = reader["ProfileURL"].ToString();
                    profile.ProfileImageURL = reader["ProfileImageURL"].ToString();
                    profile.CoverImageURL = reader["CoverImageURL"].ToString();
                    profile.UserBio = reader["UserBio"].ToString();
                    profile.Website = reader["Website"].ToString();
                    profile.NumFollowers = Convert.ToInt32(reader["NumFollowers"].ToString());
                    profile.Coins = Convert.ToInt32(reader["Coins"].ToString());

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

            SqlCommand cmd = new SqlCommand(sql, Connection);

            cmd.ExecuteNonQuery();
            
            cmd.Dispose();
            //connection.Close();
            
            
        }

        public void EditRow(string primaryKey, string columnName, string value)
        {
            string sql = $"UPDATE [dbo].[Profile] SET {columnName}='{value}' WHERE UserName={primaryKey}";
            
            SqlCommand cmd = new SqlCommand(sql, Connection);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public void DeleteRow(string primaryKey)
        {
            string sql = $"DELETE [dbo].[Profile] WHERE UserName='{primaryKey}'";

            SqlCommand cmd = new SqlCommand(sql, Connection);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

    }
}

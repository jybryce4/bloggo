using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using Bloggo.Models;

namespace Bloggo.Services.Database
{
    public class UserPostDatabaseService : IDatabaseService<UserPost, Post>
    {
        private User _user;
        static string ConnectionString = Environment.GetEnvironmentVariable("BLOGGO_DB");
        SqlConnection Connection = new SqlConnection(ConnectionString);

        public UserPostDatabaseService(User user)
        {
            _user = user;
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

        public UserPost GetItem(string primaryKey)
        {
            UserPost userPost = new UserPost();
            string selectUserPost =
                $"SELECT * FROM [dbo].[UserPost] WHERE UserPostID='{primaryKey}'";
            SqlCommand sql = new SqlCommand(selectUserPost, Connection);

            // reading the data back into the frontend
            using (SqlDataReader reader = sql.ExecuteReader())
            {
                while (reader.Read())
                {
                    userPost.UserPostID = Convert.ToInt32(reader["UserPostID"].ToString());
                    userPost.PostID = Convert.ToInt32(reader["PostID"].ToString());
                    userPost.UserName = reader["UserName"].ToString();
                }
            }

            return userPost;
        }

        public IList<UserPost> GetAllRows()
        {
            IList<UserPost> userPostList = new List<UserPost>();
           
            string query = "SELECT * FROM [dbo].[UserPost]";
            SqlCommand cmd = new SqlCommand(query, Connection);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    UserPost userPost = new UserPost();

                    userPost.UserPostID = Convert.ToInt32(reader["UserPostID"].ToString());
                    userPost.PostID = Convert.ToInt32(reader["PostID"].ToString());
                    userPost.UserName = reader["UserName"].ToString();
                    
                    userPostList.Add(userPost);
                }
                   
            }

            return userPostList;
        }

        public void CreateRow(Post post)
        {
            string sql = "INSERT INTO [dbo].[UserPost] ([PostID], [UserName])" 
                        + $"VALUES ({post.PostID}, '{_user.Username}')";

            SqlCommand cmd = new SqlCommand(sql, Connection);

            cmd.ExecuteNonQuery();
            
            cmd.Dispose();
        }

        public void EditRow(string primaryKey, string columnName, string value)
        {
            string sql = $"UPDATE [dbo].[UserPost] SET {columnName}='{value}' WHERE UserPostID={primaryKey}";
            
            SqlCommand cmd = new SqlCommand(sql, Connection);

            cmd.ExecuteNonQuery();
            cmd.Dispose();

        }
    }
}
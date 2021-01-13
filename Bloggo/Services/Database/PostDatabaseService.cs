using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using Bloggo.Models;

namespace Bloggo.Services.Database
{
    public class PostDatabaseService : IDatabaseService<Post, Post>
    {
        static string ConnectionString = Environment.GetEnvironmentVariable("BLOGGO_DB");
        SqlConnection Connection = new SqlConnection(ConnectionString);

        public PostDatabaseService()
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

        public Post GetItem(string primaryKey)
        {
            Post post = new Post();
            string selectPost =
                $"SELECT * FROM [dbo].[Post] WHERE PostID={Convert.ToInt32(primaryKey)}";
            SqlCommand sql = new SqlCommand(selectPost, Connection);

            // reading the data back into the frontend
            using (SqlDataReader reader = sql.ExecuteReader())
            {
                while (reader.Read())
                {
                    post.PostID = Convert.ToInt32(reader["PostID"].ToString());
                    post.Username = reader["UserName"].ToString();
                    post.Title = reader["Title"].ToString();
                    post.Subtitle = reader["Subtitle"].ToString();
                    post.Content = reader["Content"].ToString();
                    post.DatePosted = reader["DatePosted"].ToString();
                    post.Reblogs = Convert.ToInt32(reader["Reblogs"].ToString());
                    post.Upvotes = Convert.ToInt32(reader["Upvotes"].ToString());
                }
            }

            return post;
        }

        public IList<Post> GetAllRows()
        {
            IList<Post> postList = new List<Post>();
           
            string query = "SELECT * FROM [dbo].[Post]";
            SqlCommand cmd = new SqlCommand(query, Connection);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Post post = new Post();

                    post.PostID = Convert.ToInt32(reader["PostID"].ToString());
                    post.Username = reader["UserName"].ToString();
                    post.Title = reader["Title"].ToString();
                    post.Subtitle = reader["Subtitle"].ToString();
                    post.Content = reader["Content"].ToString();
                    post.DatePosted = reader["DatePosted"].ToString();
                    post.Reblogs = Convert.ToInt32(reader["Reblogs"].ToString());
                    post.Upvotes = Convert.ToInt32(reader["Upvotes"].ToString());

                    postList.Add(post);
                }
                   
            }

            return postList;
        }
        
        public void CreateRow(Post post)
        {
            string sql = "INSERT INTO [dbo].[Post] ([UserName], [Title], [Subtitle], [Content], [DatePosted], [Reblogs], [Upvotes])" 
                        + $"VALUES (LOWER('{post.Username}'), '{post.Title}', '{post.Subtitle}', '{post.Content}', '{post.DatePosted}', '{post.Reblogs}', '{post.Upvotes}')";

            SqlCommand cmd = new SqlCommand(sql, Connection);

            cmd.ExecuteNonQuery();
            
            cmd.Dispose();
        }
        public void EditRow(string primaryKey, string columnName, string value)
        {
            string sql = $"UPDATE [dbo].[Post] SET {columnName}='{value}' WHERE PostID={primaryKey}";
            
            SqlCommand cmd = new SqlCommand(sql, Connection);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
    }
}
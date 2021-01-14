using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bloggo.Models;

namespace Bloggo.Services.Database
{
    public class PostDatabaseService : IDatabaseService<Post, Post, int>
    {
        static string ConnectionString = Environment.GetEnvironmentVariable("BLOGGO_DB");
        SqlConnection Connection = new SqlConnection(ConnectionString);

        // public PostDatabaseService()
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

        public async Task<Post> GetItem(int primaryKey)
        {
            Post post = new Post();
            string selectPost =
                $"SELECT * FROM [dbo].[Post] WHERE PostID={primaryKey}";
            SqlCommand sql = new SqlCommand(selectPost, Connection);

            // reading the data back into the frontend
            using (SqlDataReader reader = await sql.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    post.PostID = Convert.ToInt32(reader["PostID"].ToString());
                    post.Username = reader["UserName"].ToString();
                    post.Title = reader["Title"].ToString();
                    post.Subtitle = reader["Subtitle"].ToString();
                    post.Content = reader["Content"].ToString();
                    post.DatePosted = DateTime.Parse(reader["DatePosted"].ToString());
                    post.Reblogs = Convert.ToInt32(reader["Reblogs"].ToString());
                    post.Upvotes = Convert.ToInt32(reader["Upvotes"].ToString());
                }
            }

            return post;
        }

        public async Task<IList<Post>> GetAllRows(string username = null)
        {
            IList<Post> postList = new List<Post>();

            string query = string.Empty;
            if (username != null) 
            {
                query = $"SELECT * FROM [dbo].[Post] WHERE UserName='{username}'";
            } 
            else 
            {
                query = "SELECT * FROM [dbo].[Post]";
            }
           
            
            SqlCommand cmd = new SqlCommand(query, Connection);

            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Post post = new Post();

                    post.PostID = Convert.ToInt32(reader["PostID"].ToString());
                    post.Username = reader["UserName"].ToString();
                    post.Title = reader["Title"].ToString();
                    post.Subtitle = reader["Subtitle"].ToString();
                    post.Content = reader["Content"].ToString();
                    post.DatePosted = DateTime.Parse(reader["DatePosted"].ToString());
                    post.Reblogs = Convert.ToInt32(reader["Reblogs"].ToString());
                    post.Upvotes = Convert.ToInt32(reader["Upvotes"].ToString());

                    postList.Add(post);
                }
                   
            }

            return postList;
        }
        
        public async Task CreateRow(Post post)
        {
            string sql = "INSERT INTO [dbo].[Post] ([UserName], [Title], [Subtitle], [Content], [DatePosted], [Reblogs], [Upvotes])" 
                        + $"VALUES (LOWER('{post.Username}'), '{post.Title}', '{post.Subtitle}', '{post.Content}', '{DateTime.Now}', '{post.Reblogs}', '{post.Upvotes}')";

            SqlCommand cmd = new SqlCommand(sql, Connection);

            await cmd.ExecuteNonQueryAsync();
            
            await cmd.DisposeAsync();
        }
        public async Task EditRow(int primaryKey, string columnName, string value)
        {
            string sql = $"UPDATE [dbo].[Post] SET {columnName}='{value}' WHERE PostID={primaryKey}";
            
            SqlCommand cmd = new SqlCommand(sql, Connection);

            await cmd.ExecuteNonQueryAsync();
            await cmd.DisposeAsync();
        }

        public async Task DeleteRow(int primaryKey)
        {
            string sql = $"DELETE [dbo].[Post] WHERE PostID={primaryKey}";

            SqlCommand cmd = new SqlCommand(sql, Connection);

            await cmd.ExecuteNonQueryAsync();
            await cmd.DisposeAsync();
        }
    }
}
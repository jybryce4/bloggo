using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Bloggo.Models;

namespace Bloggo.Services.Database
{
    public interface IDatabaseService<T>
    {
        static string ConnectionString { get; set; }

        static SqlConnection Connection { get; set; }

        abstract void OpenConnection();
        abstract void CloseConnection();

        abstract T GetItem(string primaryKey);
        abstract IList<T> GetAllRows();
        
        abstract void CreateRow(User user);
        abstract void EditRow(string primaryKey, string columnName, string value);
    }
}
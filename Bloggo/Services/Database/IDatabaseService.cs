using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Bloggo.Services.Database
{
    public interface IDatabaseService<T, M>
    {
        static string ConnectionString { get; set; }

        static SqlConnection Connection { get; set; }

        abstract void OpenConnection();
        abstract void CloseConnection();

        abstract T GetItem(string primaryKey);

        abstract IList<T> GetAllRows(string value = null);
        
        abstract void CreateRow(M model);
        abstract void EditRow(string primaryKey, string columnName, string value);
        abstract void DeleteRow(string primaryKey);
    }
}
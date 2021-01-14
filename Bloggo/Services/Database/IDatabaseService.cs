using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Bloggo.Services.Database
{
    public interface IDatabaseService<T, M, K> // Type of service, Model to create, primary Key type
    {
        static string ConnectionString { get; set; }

        static SqlConnection Connection { get; set; }

        abstract Task OpenConnection();
        abstract Task CloseConnection();

        abstract Task<T> GetItem(K primaryKey);

        abstract Task<IList<T>> GetAllRows(string value = null);
        
        abstract Task CreateRow(M model);
        abstract Task EditRow(K primaryKey, string columnName, string value);
        abstract Task DeleteRow(K primaryKey);
    }
}
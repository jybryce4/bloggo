using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Bloggo.Models;

namespace Bloggo.Services.Database
{
    public interface IAccountDatabaseService<T> : IDatabaseService<T>
    {
        abstract void CreateRow(User user);
    
    }
}
using Bloggo.Models;

namespace Bloggo.Services.Database
{
    public interface IContentDatabaseService<T> : IDatabaseService<T>
    {
        abstract void CreateRow(Post post);
    }
}
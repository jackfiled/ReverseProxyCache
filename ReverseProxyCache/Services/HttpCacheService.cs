using LiteDB;
using ReverseProxyCache.Models;

namespace ReverseProxyCache.Services;

public class HttpCacheService
{
    private readonly ILiteCollection<HttpCache> _collection;

    public HttpCacheService(LiteDbContext context)
    {
        _collection = context.Database.GetCollection<HttpCache>("cache");
        _collection.EnsureIndex(x => x.Path);
    }

    public HttpCache? GetCacheByPath(string path)
    {
        return _collection.FindOne(Query.EQ("Path", path));
    }

    public void InsertCache(HttpCache cache)
    {
        _collection.Insert(cache);
    }

    public void UpdateCache(HttpCache cache)
    {
        _collection.Update(cache);
    }

    public void DeleteCache(int id)
    {
        _collection.Delete(id);
    }
}
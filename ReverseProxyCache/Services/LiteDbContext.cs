using LiteDB;
using Microsoft.Extensions.Configuration;

namespace ReverseProxyCache.Services;

public class LiteDbContext
{
    public LiteDatabase Database { get; }

    public LiteDbContext(IConfiguration configuration)
    {
        string path = configuration["ReverseProxyPath:Database"] ?? "lite.db";
        path = Path.Join(Environment.CurrentDirectory, path);

        Database = new LiteDatabase(path);
    }
}
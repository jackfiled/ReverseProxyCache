using LiteDB;

namespace ReverseProxyCache.Services;

public class LocalFileService
{
    private readonly LiteDbContext _context;

    public LocalFileService(LiteDbContext context)
    {
        _context = context;
    }

    public Stream? ReadFile(string path)
    {
        path = '$' + path;

        if (_context.Database.FileStorage.Exists(path))
        {
            return _context.Database.FileStorage.OpenRead(path);
        }

        return null;
    }

    public void WriteFile(string filename, Stream content)
    {
        var path = '$' + filename;

        _context.Database.FileStorage.Upload(path, filename, content);
    }
}
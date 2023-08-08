using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReverseProxyCache.Models;

namespace ReverseProxyCache.Services;

public class CacheCleanService : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly LiteDbContext _context;
    private readonly HttpCacheService _cacheService;
    private readonly ILogger<CacheCleanService> _logger;
    private readonly int _refreshTime;

    public CacheCleanService(LiteDbContext context,
        ILogger<CacheCleanService> logger,
        HttpCacheService cacheService,
        IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _cacheService = cacheService;
        
        // 获得刷新的时间间隔
        string timeStr = configuration["ReverseProxyCache:RefreshTime"] ?? "7";
        if (!int.TryParse(timeStr, out _refreshTime))
        {
            _refreshTime = 7;
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start timed cache clean task.");

        _timer = new Timer(CleanCache, null, TimeSpan.FromDays(_refreshTime), 
            TimeSpan.FromDays(_refreshTime));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stop timed cache clean task.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    private void CleanCache(object? state)
    {
        _logger.LogInformation("Deleting cached content.");
        var caches = _cacheService.GetCaches();

        foreach (HttpCache cache in caches)
        {
            var filepath = '$' + cache.Path;
            _context.Database.FileStorage.Delete(filepath);
        }
        
        _cacheService.DeleteCache();
        _logger.LogInformation("Deleted cached content.");
        
    }
}
using Microsoft.Extensions.DependencyInjection;
using ReverseProxyCache.Services;

namespace ReverseProxyCache;

public static class ReverseProxyCacheServiceExtensions
{
    public static IServiceCollection AddReverseProxyCache(this IServiceCollection services)
    {
        services.AddHttpClient();
        
        services.AddSingleton<LiteDbContext>();
        services.AddTransient<HttpCacheService>();
        services.AddTransient<LocalFileService>();
        services.AddSingleton<ReverseProxyOptionService>();
        services.AddHostedService<CacheCleanService>();

        return services;
    }
}
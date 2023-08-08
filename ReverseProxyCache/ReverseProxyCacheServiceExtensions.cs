using Microsoft.Extensions.DependencyInjection;
using ReverseProxyCache.Services;

namespace ReverseProxyCache;

public static class ReverseProxyCacheServiceExtensions
{
    public static IServiceCollection AddReverseProxyCache(this IServiceCollection services)
    {
        services.AddSingleton<LiteDbContext>();
        services.AddTransient<HttpCacheService>();
        services.AddTransient<LocalFileService>();

        return services;
    }
}
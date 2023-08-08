using Microsoft.AspNetCore.Builder;

namespace ReverseProxyCache;

public static class ReverseProxyCacheExtensions
{
    public static IApplicationBuilder UseReverseProxyCache(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ReverseProxyCacheMiddleware>();
    }

    public static IApplicationBuilder UseReverseProxyRefresh(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ReverseProxyRefreshMiddleware>();
    }
}
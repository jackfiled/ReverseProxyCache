using Microsoft.AspNetCore.Builder;

namespace ReverseProxyCache;

public static class ReverseProxyCacheMiddleExtensions
{
    public static IApplicationBuilder UseReverseProxyCache(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ReverseProxyCacheMiddleware>();
    }
}
using Microsoft.AspNetCore.Http;

namespace ReverseProxyCache;

public class ReverseProxyCacheMiddleware
{
    private readonly RequestDelegate _next;

    public ReverseProxyCacheMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
    }
}
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ReverseProxyCache.Models;
using ReverseProxyCache.Services;

namespace ReverseProxyCache;

public class ReverseProxyRefreshMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ReverseProxyRefreshMiddleware> _logger;
    private readonly LiteDbContext _context;
    private readonly ReverseProxyOptionService _optionService;

    public ReverseProxyRefreshMiddleware(RequestDelegate next,
        ILogger<ReverseProxyRefreshMiddleware> logger,
        LiteDbContext context,
        ReverseProxyOptionService optionService)
    {
        _next = next;
        _logger = logger;
        _context = context;
        _optionService = optionService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 判断是否为刷新请求URL
        if (context.Request.Method == "POST" && context.Request.Path == "/_/refresh")
        {
            var reader = new StreamReader(context.Request.Body);
            string request = await reader.ReadToEndAsync();

            if (string.IsNullOrEmpty(request))
            {
                await _next(context);
                return;
            }
            
            _logger.LogInformation("Receive Refresh request: {}", request);

            string? router = null;
            foreach (ReverseProxyOptions option in _optionService.Options)
            {
                if (option.Router.Contains(request))
                {
                    router = option.Router;
                    break;
                }
            }

            context.Response.ContentType = "text/plain";

            if (router == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync($"Unknown request: {request}.");
            }
            else
            {
                _logger.LogInformation("Delete cache for {}.", router);
                
                CleanCache(router);
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("Clean Successfully!");
            }
            
            return;
        }

        await _next(context);
    }

    private void CleanCache(string router)
    {
        ILiteCollection<HttpCache> collection = _context.Database.GetCollection<HttpCache>("cache");
        collection.EnsureIndex(x => x.Router);

        var caches = collection.Find(Query.EQ("Router", router));

        foreach (HttpCache cache in caches)
        {
            var path = '$' + cache.Path;
            _context.Database.FileStorage.Delete(path);
        }

        collection.DeleteMany(Query.EQ("Router", router));
    }
}
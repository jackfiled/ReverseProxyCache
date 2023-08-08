using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ReverseProxyCache.Models;
using ReverseProxyCache.Services;

namespace ReverseProxyCache;

public class ReverseProxyCacheMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly LocalFileService _fileService;
    private readonly HttpCacheService _httpCacheService;
    private readonly ILogger<ReverseProxyCacheMiddleware> _logger;

    public ReverseProxyCacheMiddleware(
        RequestDelegate next, 
        IHttpClientFactory httpClientFactory,
        LocalFileService fileService,
        HttpCacheService httpCacheService,
        ILogger<ReverseProxyCacheMiddleware> logger)
    {
        _next = next;
        _httpClientFactory = httpClientFactory;
        _fileService = fileService;
        _httpCacheService = httpCacheService;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.ToString();
        if (path.StartsWith("/blog/"))
        {
            await ReverseProxy("https://jackfiled.github.io", path, context);
            return;
        }
        
        await _next(context);
    }

    private async Task ReverseProxy(string baseurl, string path, HttpContext context)
    {
        Stream? stream = _fileService.ReadFile(path);
        if (stream != null)
        {
            _logger.LogInformation("Path {path} cached!", path);
            
            // 读取数据库
            HttpCache? cache = _httpCacheService.GetCacheByPath(path);
            if (cache == null)
            {
                throw new ReverseProxyException($"Failed to get info from database from {path}");
            }

            context.Response.ContentType = cache.ContentType;
            await stream.CopyToAsync(context.Response.Body);
            return;
        }
            
        _logger.LogInformation("Failed to get cache for {path}, go to upstream", path);
        var client = _httpClientFactory.CreateClient();
        HttpResponseMessage message = await client.GetAsync(baseurl + path);

        context.Response.ContentType = message.Content.Headers.ContentType?.ToString();
        await message.Content.CopyToAsync(context.Response.Body);
        
        _fileService.WriteFile(path, await message.Content.ReadAsStreamAsync());
        var newCache = new HttpCache
        {
            ContentType = message.Content.Headers.ContentType?.ToString(),
            Path = path,
            RefreshTime = DateTime.Now,
            Baseurl = baseurl
        };
        _httpCacheService.InsertCache(newCache);
    }
}
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReverseProxyCache.Models;

namespace ReverseProxyCache.Services;

public class ReverseProxyOptionService
{
    public List<ReverseProxyOptions> Options { get; } = new();

    public ReverseProxyOptionService(
        IConfiguration configuration,
        ILogger<ReverseProxyOptionService> logger)
    {
        var result = configuration.GetSection("ReverseProxy").Get<IList<ReverseProxyOptions>>();
        if (result != null)
        {
            Options.AddRange(result);

            var builder = new StringBuilder("Read reverse proxy options:\n");
            foreach (ReverseProxyOptions option in result)
            {
                builder.Append(option).Append('\n');
            }
            logger.LogInformation(builder.ToString());
        }
        else
        {
            logger.LogWarning("No configuration for ReverseProxy. Program will do nothing!");
        }
    }
}
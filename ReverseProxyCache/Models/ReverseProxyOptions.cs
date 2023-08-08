namespace ReverseProxyCache.Models;

public class ReverseProxyOptions
{
    public string Baseurl { get; set; } = string.Empty;
    public string Router { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Reverse for {Router} to {Baseurl + Router}";
    }
}
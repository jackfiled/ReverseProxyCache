namespace ReverseProxyCache.Models;

public class HttpCache
{
    public int Id { get; set; }
    public string? Path { get; set; }
    public string? Router { get; set; }
    public string? ContentType { get; set; }
    public DateTime? RefreshTime { get; set; }
}
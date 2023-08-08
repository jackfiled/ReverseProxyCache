using ReverseProxyCache;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddReverseProxyCache();

WebApplication app = builder.Build();

app.UseReverseProxyCache();
app.UseReverseProxyRefresh();

app.MapGet("/", () => "Hello World!");

app.Run();

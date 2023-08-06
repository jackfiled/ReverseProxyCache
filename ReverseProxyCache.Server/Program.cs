using ReverseProxyCache;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();

app.UseReverseProxyCache();

app.MapGet("/", () => "Hello World!");

app.Run();

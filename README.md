# ReverseProxyCache

一个~~略显傻逼~~的反向代理工具。

## 设计目的

由于我个人有大量使用`Github Pages`搭建的静态网站需要经常访问，常常有人（包括我）向我反馈说访问网站好慢啊。于是我就设计了这个反向代理+自动缓存的工具部署在我个人的云服务器上提升访问速度。

## 主要功能

- 缓存
- 使用配置文件配置反向代理规则
- `HTTP`钩子清除缓存文件，便于在`Actions`运行完成之后及时刷新缓存

### 配置文件示例

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ReverseProxy": [
    {
      "Baseurl": "https://jackfiled.github.io",
      "Router": "/blog/"
    },
    {
      "Baseurl": "https://jackfiled.github.io",
      "Router": "/wiki/"
    }
  ]
}
```

`ReverseProxy`需要提供了一个列表，其中每一个对象提供`Baseurl`和`Router`两个属性，以第一个配置项为例，将把所有对于`/blog/`的请求转发到`https://jackfiled.github.io/blog/`，同时将上游服务器返回的结果缓存下来。

### `HTTP`刷新钩子

 使用`POST`方法请求终结点`/_/refresh`，在请求体中指定需要刷新的路由，即可清除该路由下对应的所有缓存数据。

如下面这个`curl`指令将清除上述配置文件中`blog`路由下对应的缓存文件：

```shell
curl https://[yourdomainname]/_/refresh -X POST -d "blog"
```

## License

MIT

using RestSharp;

namespace BF1ServerTools.Helpers;

public static class HttpHelper
{
    private static readonly RestClient _client = null;

    /// <summary>
    /// 初始化
    /// </summary>
    static HttpHelper()
    {
        if (_client != null)
            return;

        // 不抛出相关错误
        var options = new RestClientOptions()
        {
            MaxTimeout = 5000,
            ThrowOnAnyError = false,
            ThrowOnDeserializationError = false
        };

        // 判断是否使用代理
        if (Globals.IsUseProxy)
        {
            // 判断代理是否配置正确
            if (Globals.IPAddress != default && Globals.Port != default)
            {
                var proxy = new WebProxy()
                {
                    Address = new Uri($"http://{Globals.IPAddress}:{Globals.Port}"),
                };
                options.Proxy = proxy;
            }
        }

        _client = new RestClient(options);
    }

    /// <summary>
    /// 获取网络图片字节数组
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static async Task<byte[]> GetWebImageBytes(string url)
    {
        var request = new RestRequest(url);

        var response = await _client.ExecuteGetAsync(request);
        if (response.IsSuccessStatusCode)
            return response.RawBytes;
        else
            return null;
    }
}

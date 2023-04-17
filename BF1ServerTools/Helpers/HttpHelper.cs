using RestSharp;

namespace BF1ServerTools.Helpers;

public static class HttpHelper
{
    private static RestClient _client;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="ip">代理ip</param>
    /// <param name="port">代理端口</param>
    public static void Initialize(IPAddress ip = default, int port = default)
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
        if (ip != default && port != default)
        {
            var proxy = new WebProxy()
            {
                Address = new Uri($"http://{ip}:{port}"),
            };
            options.Proxy = proxy;
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

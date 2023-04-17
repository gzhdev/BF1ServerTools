using RestSharp;

namespace BF1ServerTools.API;

public static class EA2API
{
    private const string _host1 = "https://accounts.ea.com/connect/auth?response_type=token&locale=zh_CN&client_id=ORIGIN_JS_SDK&redirect_uri=nucleus%3Arest";
    private const string _host2 = "https://gateway.ea.com/proxy/identity/personas?namespaceName=cem_ea_id&displayName=";

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
    /// 通用 GET 请求
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static async Task<RespContent> GetAsync(RestRequest request)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respContent = new RespContent();

        try
        {
            var response = await _client.ExecuteGetAsync(request);
            respContent.HttpCode = response.StatusCode;
            respContent.Content = response.Content;

            if (response.StatusCode == HttpStatusCode.OK)
                respContent.IsSuccess = true;
        }
        catch (Exception ex)
        {
            respContent.Content = ex.Message;
        }

        sw.Stop();
        respContent.ExecTime = sw.Elapsed.TotalSeconds;

        return respContent;
    }

    /// <summary>
    /// 使用Cookies获取access_token
    /// </summary>
    /// <param name="remid"></param>
    /// <param name="sid"></param>
    /// <returns></returns>
    public static async Task<RespContent> GetAccessToken(string remid, string sid)
    {
        var request = new RestRequest(_host1)
            .AddHeader("Cookie", $"remid={remid};sid={sid};");

        return await GetAsync(request);
    }

    /// <summary>
    /// 获取玩家数字id
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="playerName"></param>
    /// <returns></returns>
    public static async Task<RespContent> GetPlayerPersonaId(string accessToken, string playerName)
    {
        var request = new RestRequest($"{_host2}{playerName}")
            .AddHeader("X-Expand-Results", true)
            .AddHeader("Authorization", $"Bearer {accessToken}");

        return await GetAsync(request);
    }
}

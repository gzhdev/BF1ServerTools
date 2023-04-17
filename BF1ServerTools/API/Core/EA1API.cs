using RestSharp;

namespace BF1ServerTools.API;

public static class EA1API
{
    private const string _host = "https://accounts.ea.com/connect/auth?client_id=sparta-backend-as-user-pc&response_type=code&release_type=none";

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
            FollowRedirects = false,
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
    /// 使用Cookies获取authcode，同时更新Cookies
    /// </summary>
    /// <param name="remid"></param>
    /// <param name="sid"></param>
    /// <returns></returns>
    public static async Task<RespAuth> GetAuthCode(string remid, string sid)
    {
        var sw = new Stopwatch();
        sw.Start();
        var respAuth = new RespAuth();

        try
        {
            var request = new RestRequest(_host)
                .AddHeader("Cookie", $"remid={remid};sid={sid}");

            var response = await _client.ExecuteGetAsync(request);
            respAuth.HttpCode = response.StatusCode;
            respAuth.Content = response.Content;

            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                string localtion = response.Headers.ToList()
                    .Find(x => x.Name == "Location")
                    .Value.ToString();

                if (localtion.Contains("127.0.0.1/success?code="))
                {
                    if (response.Cookies.Count == 2)
                    {
                        respAuth.Remid = response.Cookies[0].Value;
                        respAuth.Sid = response.Cookies[1].Value;
                    }
                    else
                    {
                        respAuth.Sid = response.Cookies[0].Value;
                    }

                    respAuth.IsSuccess = true;
                    respAuth.Code = localtion.Replace("http://127.0.0.1/success?code=", "")
                        .Replace("https://127.0.0.1/success?code=", "");
                }

                respAuth.Content = localtion;
            }
        }
        catch (Exception ex)
        {
            respAuth.Content = ex.Message;
        }

        sw.Stop();
        respAuth.ExecTime = sw.Elapsed.TotalSeconds;

        return respAuth;
    }
}

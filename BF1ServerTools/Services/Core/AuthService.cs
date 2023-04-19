using BF1ServerTools.API;
using BF1ServerTools.SDK;
using BF1ServerTools.Helpers;

namespace BF1ServerTools.Services;

public static class AuthService
{
    public static event Action UpdateMode2DataEvent;

    /// <summary>
    /// 模式1 更新SessionId信息
    /// </summary>
    public static async void UpdateMode1SessionId()
    {
        if (!Globals.IsUseMode1)
            return;

        var sessionId = await Scan.GetGatewaySession();
        if (sessionId != string.Empty)
        {
            Globals.SessionId1 = sessionId;
            LoggerHelper.Info($"内存扫描SessionId成功 {Globals.SessionId}");
        }
        else
        {
            LoggerHelper.Error("内存扫描SessionId失败");
        }
    }

    /// <summary>
    /// 模式2 更新SessionId信息
    /// </summary>
    public static async void UpdateMode2SessionId()
    {
        if (Globals.IsUseMode1)
            return;

        if (string.IsNullOrEmpty(Globals.Remid) || string.IsNullOrEmpty(Globals.Sid))
        {
            LoggerHelper.Warn("Remid或Sid为空，更新取消");
            return;
        }

        var respAuth = await EA1API.GetAuthCode(Globals.Remid, Globals.Sid);
        if (respAuth.IsSuccess)
        {
            if (!string.IsNullOrEmpty(respAuth.Remid))
                Globals.Remid = respAuth.Remid;
            if (!string.IsNullOrEmpty(respAuth.Sid))
                Globals.Sid = respAuth.Sid;

            var result = await EA2API.GetAccessToken(Globals.Remid, Globals.Sid);
            if (result.IsSuccess)
            {
                var jNode = JsonNode.Parse(result.Content);
                Globals.AccessToken = jNode["access_token"].GetValue<string>();
                LoggerHelper.Info("刷新玩家access_token成功");
            }

            result = await BF1API.GetEnvIdViaAuthCode(respAuth.Code);
            if (result.IsSuccess)
            {
                var envIdViaAuthCode = JsonHelper.JsonDeserialize<EnvIdViaAuthCode>(result.Content);
                Globals.SessionId2 = envIdViaAuthCode.result.sessionId;
                Globals.PersonaId2 = long.Parse(envIdViaAuthCode.result.personaId);

                result = await BF1API.GetPersonasByIds(Globals.SessionId2, Globals.PersonaId);
                if (result.IsSuccess)
                {
                    var jNode = JsonNode.Parse(result.Content);
                    var personas = jNode["result"]![$"{Globals.PersonaId}"];
                    if (personas != null)
                    {
                        Globals.Avatar2 = personas!["avatar"].GetValue<string>();
                        Globals.DisplayName2 = personas!["displayName"].GetValue<string>();

                        LoggerHelper.Info("刷新玩家Cookies数据成功");

                        ////////////////////// 通知事件 //////////////////////

                        UpdateMode2DataEvent?.Invoke();
                    }
                }
            }
        }
    }
}

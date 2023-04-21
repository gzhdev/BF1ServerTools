using BF1ServerTools.Helpers;

namespace BF1ServerTools.Services;

public static class AuthUtil
{
    /// <summary>
    /// 检查SessionId
    /// </summary>
    /// <returns></returns>
    public static bool CheckPlayerSesId()
    {
        if (!GameUtil.IsInGame())
        {
            NotifierHelper.Show(NotifierType.Warning, "GameId为空，请先进入服务器");
            return false;
        }

        if (!GameUtil.IsValidSessionId())
        {
            NotifierHelper.Show(NotifierType.Warning, "请先获取玩家SessionId后，再执行本操作");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 检查玩家SessionId和管理员授权
    /// </summary>
    /// <returns></returns>
    public static bool CheckPlayerAuth()
    {
        if (!CheckPlayerSesId())
            return false;

        if (!Globals.LoginPlayerIsAdmin)
        {
            NotifierHelper.Show(NotifierType.Warning, $"玩家 {Globals.DisplayName} 不是当前服务器的管理员");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 检查玩家SessionId、管理员授权和ServerId
    /// </summary>
    /// <returns></returns>
    public static bool CheckPlayerAuth2()
    {
        if (!CheckPlayerAuth())
            return false;

        if (!GameUtil.IsValidServerId())
        {
            NotifierHelper.Show(NotifierType.Warning, "ServerId为空，请重新获取服务器详细信息");
            return false;
        }

        return true;
    }
}

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
        if (Globals.GameId == 0)
        {
            NotifierHelper.Show(NotifierType.Warning, "GameId为空，请先进入服务器");
            return false;
        }

        if (string.IsNullOrEmpty(Globals.SessionId))
        {
            NotifierHelper.Show(NotifierType.Warning, "请先获取玩家SessionId后，再执行本操作");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 检查SessionId1
    /// </summary>
    /// <returns></returns>
    public static bool CheckPlayerSesId1()
    {
        if (Globals.GameId == 0)
        {
            NotifierHelper.Show(NotifierType.Warning, "GameId为空，请先进入服务器");
            return false;
        }

        if (string.IsNullOrEmpty(Globals.SessionId1))
        {
            NotifierHelper.Show(NotifierType.Warning, "请先获取玩家SessionId1后，再执行本操作");
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

        if (Globals.ServerId == 0)
        {
            NotifierHelper.Show(NotifierType.Warning, "ServerId为空，请重新获取服务器详细信息");
            return false;
        }

        return true;
    }

    /// <summary>
    /// 获取队伍信息
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    public static string GetTeamInfo(Team team)
    {
        return team switch
        {
            Team.Team01 => "观战",
            Team.Team02 => "载入中",
            Team.Team1 => "队伍1",
            Team.Team2 => "队伍2",
            _ => string.Empty,
        };
    }
}

using BF1ServerTools.Data;

namespace BF1ServerTools.Services;

public static class GameUtil
{
    /// <summary>
    /// 判断是否为观战玩家
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public static bool IsSpectator(byte flag)
    {
        return flag == 0x01;
    }

    /// <summary>
    /// 判断是否进入游戏
    /// </summary>
    /// <returns></returns>
    public static bool IsInGame()
    {
        return Globals.GameId != 0;
    }

    /// <summary>
    /// 判断SessionId是否有效
    /// </summary>
    /// <returns></returns>
    public static bool IsValidSessionId()
    {
        return !string.IsNullOrWhiteSpace(Globals.SessionId);
    }

    /// <summary>
    /// 判断ServerId是否有效
    /// </summary>
    /// <returns></returns>
    public static bool IsValidServerId()
    {
        return Globals.ServerId != 0;
    }

    /// <summary>
    /// 转为mm:ss字符串格式，传入时间为秒
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public static string GetMMSSStrBySecond(float second)
    {
        var mm = second / 60;
        var ss = second % 60;

        return $"{mm:00}:{ss:00}";
    }

    /// <summary>
    /// 转为分钟数，传入时间为秒
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public static int GetMinuteBySecond(float second)
    {
        // 排除负数和大于10小时的情况
        if (second <= 0 || second > 36000)
            return 0;

        var ts = TimeSpan.FromSeconds(second);
        return (int)ts.TotalMinutes;
    }

    /// <summary>
    /// 判断玩家是不是管理员
    /// </summary>
    /// <param name="personaId"></param>
    /// <returns></returns>
    public static bool IsServerAdmin(long personaId)
    {
        return Globals.ServerAdmins_PID.Contains(personaId);
    }

    /// <summary>
    /// 判断玩家是不是VIP
    /// </summary>
    /// <param name="personaId"></param>
    /// <returns></returns>
    public static bool IsServerVIP(long personaId)
    {
        return Globals.ServerVIPs_PID.Contains(personaId);
    }

    /// <summary>
    /// 判断玩家是不是白名单
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool IsServerWhite(string name)
    {
        return Globals.CustomWhites_Name.Contains(name);
    }

    /// <summary>
    /// 查找生涯数据，如果未查到则返回null
    /// </summary>
    /// <param name="personaId"></param>
    /// <returns></returns>
    public static LifeCache FindPlayerLifeCache(long personaId)
    {
        if (Globals.PlayerLifeCaches.Count == 0)
            return null;

        return Globals.PlayerLifeCaches.Find(item => item.PersonaId == personaId);
    }

    /// <summary>
    /// 获取玩家生涯KD
    /// </summary>
    /// <param name="personaId"></param>
    /// <returns></returns>
    public static float GetLifeKD(long personaId)
    {
        var lifeCache = FindPlayerLifeCache(personaId);
        if (lifeCache != null)
            return lifeCache.KD;

        return 0.0f;
    }

    /// <summary>
    /// 获取玩家生涯KPM
    /// </summary>
    /// <param name="personaId"></param>
    /// <returns></returns>
    public static float GetLifeKPM(long personaId)
    {
        var lifeCache = FindPlayerLifeCache(personaId);
        if (lifeCache != null)
            return lifeCache.KPM;

        return 0.0f;
    }

    /// <summary>
    /// 获取玩家生涯时长
    /// </summary>
    /// <param name="personaId"></param>
    /// <returns></returns>
    public static int GetLifeTime(long personaId)
    {
        var lifeCache = FindPlayerLifeCache(personaId);
        if (lifeCache != null)
            return lifeCache.Time;

        return 0;
    }

    /// <summary>
    /// 获取玩家主武器生涯星数
    /// </summary>
    /// <param name="personaId"></param>
    /// <param name="weaponName"></param>
    /// <returns></returns>
    public static int GetLifeStar(long personaId, string weaponName)
    {
        var guid = ClientUtil.GetWeaponGuid(weaponName);
        if (string.IsNullOrWhiteSpace(guid))
            return 0;

        var lifeCache = FindPlayerLifeCache(personaId);
        if (lifeCache != null)
        {
            var weapon = lifeCache.WeaponStats.Find(x => x.guid == guid);
            if (weapon != null)
                return weapon.star;

            var vehicel = lifeCache.VehicleStats.Find(x => x.guid == guid);
            if (vehicel != null)
                return vehicel.star;
        }

        return 0;
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

using BF1ServerTools.Data;

namespace BF1ServerTools.Services;

public static class GameUtil
{
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
}

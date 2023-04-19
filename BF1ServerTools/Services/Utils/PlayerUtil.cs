namespace BF1ServerTools.Services;

public static class PlayerUtil
{
    /// <summary>
    /// 获取玩家游玩时间，传入时间为秒，返回分钟数或小时数
    /// </summary>
    /// <param name="second">秒</param>
    /// <returns></returns>
    public static string GetPlayTimeStrBySecond(float second)
    {
        var ts = TimeSpan.FromSeconds(second);

        // 低于一小时，则显示分钟数
        if (ts.TotalHours < 1)
            return $"{ts.TotalMinutes:0} 分钟";

        return $"{ts.TotalHours:0} 小时";
    }

    /// <summary>
    /// 获取游玩小时数，传入时间为秒
    /// </summary>
    /// <param name="second">秒</param>
    /// <returns></returns>
    public static int GetPlayHoursBySecond(float second)
    {
        var ts = TimeSpan.FromSeconds(second);
        return (int)ts.TotalHours;
    }

    /// <summary>
    /// 计算玩家KD
    /// </summary>
    /// <param name="kill">击杀数</param>
    /// <param name="dead">死亡数</param>
    /// <returns></returns>
    public static float GetPlayerKD(int kill, int dead)
    {
        if (kill == 0 && dead >= 0)
            return 0.0f;
        else if (kill > 0 && dead == 0)
            return kill;
        else
            return (float)kill / dead;
    }

    /// <summary>
    /// 计算玩家KPM，传入时间为秒
    /// </summary>
    /// <param name="kill">击杀数</param>
    /// <param name="second">秒</param>
    /// <returns></returns>
    public static float GetPlayerKPMBySecond(float kill, float second)
    {
        if (second <= 0.0f)
            return 0.0f;

        var ts = TimeSpan.FromSeconds(second);
        return kill / (float)ts.TotalMinutes;
    }

    /// <summary>
    /// 获取百分比
    /// </summary>
    /// <param name="num1">被除数</param>
    /// <param name="num2">除数，不可为0</param>
    /// <returns></returns> 
    public static float GetPlayerPercent(float num1, float num2)
    {
        if (num2 == 0)
            return 0.0f;

        return num1 / num2 * 100;
    }

    /// <summary>
    /// 获取击杀星数
    /// </summary>
    /// <param name="kills">击杀数</param>
    /// <returns></returns>
    public static int GetKillStar(float kills)
    {
        if (kills < 100.0f)
            return 0;

        return (int)kills / 100;
    }
}

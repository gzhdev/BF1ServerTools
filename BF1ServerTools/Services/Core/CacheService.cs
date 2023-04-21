using BF1ServerTools.API;
using BF1ServerTools.SDK;
using BF1ServerTools.Data;
using BF1ServerTools.Utils;
using BF1ServerTools.Helpers;

namespace BF1ServerTools.Services;

public static class CacheService
{
    /// <summary>
    /// 生涯数据缓存时间，单位分钟
    /// </summary>
    private const int CacheTime = 60;

    /// <summary>
    /// 更新当前服务器玩家生涯信息缓存线程
    /// </summary>
    public static async void UpdateLifeCacheThread()
    {
        while (true)
        {
            if (ServiceApp.IsDispose)
                return;

            // 移除超过预设时间的玩家
            // 倒叙删除，因为每次删除list的下标号会改变，倒叙就可以避免这个问题
            for (int i = Globals.PlayerLifeCaches.Count - 1; i >= 0; i--)
            {
                if (MiscUtil.DiffMinutes(Globals.PlayerLifeCaches[i].CreateTime, DateTime.Now) > CacheTime)
                {
                    Globals.PlayerLifeCaches.RemoveAt(i);
                }
            }

            // 排除 未进入服务器 且 SessionId为空
            if (!GameUtil.IsInGame() || !GameUtil.IsValidSessionId())
                goto LOOP;

            // 遍历玩家列表
            foreach (var item in Player.GetPlayerCache())
            {
                // 排除已经缓存过的数据
                if (Globals.PlayerLifeCaches.Find(x => x.PersonaId == item.PersonaId) != null)
                    continue;

                var lifeCache = new LifeCache
                {
                    BaseStats = new(),
                    WeaponStats = new(),
                    VehicleStats = new()
                };

                lifeCache.Name = item.Name;
                lifeCache.PersonaId = item.PersonaId;

                // 缓存玩家生涯KD、KPM
                var result = await BF1API.DetailedStatsByPersonaId(Globals.SessionId, item.PersonaId);
                if (result.IsSuccess)
                {
                    MakeDetailedStats(lifeCache, result.Content);

                    // 缓存玩家生涯武器数据
                    result = await BF1API.GetWeaponsByPersonaId(Globals.SessionId, item.PersonaId);
                    if (result.IsSuccess)
                    {
                        MakeGetWeapons(lifeCache, result.Content);

                        // 缓存玩家生涯载具数据
                        result = await BF1API.GetVehiclesByPersonaId(Globals.SessionId, item.PersonaId);
                        if (result.IsSuccess)
                        {
                            MakeGetVehicles(lifeCache, result.Content);

                            lifeCache.CreateTime = DateTime.Now;
                            Globals.PlayerLifeCaches.Add(lifeCache);
                        }
                    }
                }

                Thread.Sleep(100);
            }

        LOOP:
            Thread.Sleep(5000);
        }
    }

    /// <summary>
    /// 处理生涯综合数据
    /// </summary>
    /// <param name="lifeCache"></param>
    /// <param name="content"></param>
    private static void MakeDetailedStats(LifeCache lifeCache, string content)
    {
        var detailedStats = JsonHelper.JsonDeserialize<DetailedStats>(content);

        var result = detailedStats.result;
        var basicStats = result.basicStats;

        lifeCache.KD = PlayerUtil.GetPlayerKD(basicStats.kills, basicStats.deaths);
        lifeCache.KPM = basicStats.kpm;
        lifeCache.Time = PlayerUtil.GetPlayHoursBySecond(basicStats.timePlayed);

        // basicStats
        lifeCache.BaseStats.timePlayed = basicStats.timePlayed;
        lifeCache.BaseStats.wins = basicStats.wins;
        lifeCache.BaseStats.losses = basicStats.losses;
        lifeCache.BaseStats.kills = basicStats.kills;
        lifeCache.BaseStats.deaths = basicStats.deaths;
        lifeCache.BaseStats.kpm = basicStats.kpm;
        lifeCache.BaseStats.skill = basicStats.skill;

        lifeCache.BaseStats.favoriteClass = ClientUtil.GetClassChs(result.favoriteClass);

        lifeCache.BaseStats.awardScore = result.awardScore;
        lifeCache.BaseStats.bonusScore = result.bonusScore;
        lifeCache.BaseStats.squadScore = result.squadScore;
        lifeCache.BaseStats.avengerKills = result.avengerKills;
        lifeCache.BaseStats.saviorKills = result.saviorKills;
        lifeCache.BaseStats.highestKillStreak = result.highestKillStreak;
        lifeCache.BaseStats.dogtagsTaken = result.dogtagsTaken;
        lifeCache.BaseStats.roundsPlayed = result.roundsPlayed;
        lifeCache.BaseStats.flagsCaptured = result.flagsCaptured;
        lifeCache.BaseStats.flagsDefended = result.flagsDefended;
        lifeCache.BaseStats.accuracyRatio = result.accuracyRatio;
        lifeCache.BaseStats.headShots = result.headShots;
        lifeCache.BaseStats.longestHeadShot = result.longestHeadShot;
        lifeCache.BaseStats.nemesisKills = result.nemesisKills;
        lifeCache.BaseStats.nemesisKillStreak = result.nemesisKillStreak;
        lifeCache.BaseStats.revives = result.revives;
        lifeCache.BaseStats.heals = result.heals;
        lifeCache.BaseStats.repairs = result.repairs;
        lifeCache.BaseStats.suppressionAssist = result.suppressionAssist;
        lifeCache.BaseStats.kdr = result.kdr;
        lifeCache.BaseStats.killAssists = result.killAssists;

        lifeCache.BaseStats.kd = PlayerUtil.GetPlayerKD(basicStats.kills, basicStats.deaths);
        lifeCache.BaseStats.winPercent = PlayerUtil.GetPlayerPercent(basicStats.wins, result.roundsPlayed);
        lifeCache.BaseStats.headshotsVKills = PlayerUtil.GetPlayerPercent(result.headShots, basicStats.kills);
    }

    /// <summary>
    /// 处理生涯武器数据
    /// </summary>
    /// <param name="lifeCache"></param>
    /// <param name="content"></param>
    private static void MakeGetWeapons(LifeCache lifeCache, string content)
    {
        var getWeapons = JsonHelper.JsonDeserialize<GetWeapons>(content);

        foreach (var res in getWeapons.result)
        {
            foreach (var wea in res.weapons)
            {
                // 过滤击杀数为0的数据
                if (wea.stats.values.kills == 0)
                    continue;

                var weaponStat = new WeaponStat()
                {
                    guid = wea.guid,
                    name = ChsHelper.ToSimplified(wea.name),
                    imageUrl = ClientUtil.GetWebWeaponImage(wea.imageUrl),

                    hits = wea.stats.values.hits,
                    shots = wea.stats.values.shots,
                    kills = wea.stats.values.kills,
                    headshots = wea.stats.values.headshots,
                    accuracy = wea.stats.values.accuracy,
                    seconds = wea.stats.values.seconds,
                };

                weaponStat.kpm = PlayerUtil.GetPlayerKPMBySecond(weaponStat.kills, weaponStat.seconds);
                weaponStat.headshotsVKills = PlayerUtil.GetPlayerPercent(weaponStat.headshots, weaponStat.kills);
                weaponStat.hitsVShots = PlayerUtil.GetPlayerPercent(weaponStat.hits, weaponStat.shots);
                weaponStat.hitVKills = weaponStat.hits / weaponStat.kills;

                weaponStat.star = PlayerUtil.GetKillStar(weaponStat.kills);
                weaponStat.time = PlayerUtil.GetPlayTimeStrBySecond(weaponStat.seconds);

                lifeCache.WeaponStats.Add(weaponStat);
            }
        }

        // 按击杀数降序排序
        lifeCache.WeaponStats.Sort((a, b) => b.kills.CompareTo(a.kills));
    }

    /// <summary>
    /// 处理生涯载具数据
    /// </summary>
    /// <param name="lifeCache"></param>
    /// <param name="content"></param>
    private static void MakeGetVehicles(LifeCache lifeCache, string content)
    {
        var getVehicles = JsonHelper.JsonDeserialize<GetVehicles>(content);

        foreach (var res in getVehicles.result)
        {
            foreach (var veh in res.vehicles)
            {
                // 过滤击杀数为0的数据
                if (veh.stats.values.kills == 0)
                    continue;

                var vehicleStat = new VehicleStat()
                {
                    guid = veh.guid,
                    name = ChsHelper.ToSimplified(veh.name),
                    imageUrl = ClientUtil.GetWebWeaponImage(veh.imageUrl),

                    seconds = veh.stats.values.seconds,
                    kills = veh.stats.values.kills,
                    destroyed = veh.stats.values.destroyed,
                };

                vehicleStat.kpm = PlayerUtil.GetPlayerKPMBySecond(vehicleStat.kills, vehicleStat.seconds);

                vehicleStat.star = PlayerUtil.GetKillStar(vehicleStat.kills);
                vehicleStat.time = PlayerUtil.GetPlayTimeStrBySecond(vehicleStat.seconds);

                lifeCache.VehicleStats.Add(vehicleStat);
            }
        }

        // 按击杀数降序排序
        lifeCache.VehicleStats.Sort((a, b) => b.kills.CompareTo(a.kills));
    }
}

using BF1ServerTools.API;
using BF1ServerTools.SDK;
using BF1ServerTools.Utils;

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
            // 倒叙删除，因为每次删除list的下标号会改变，倒叙就不存在这个问题
            for (int i = Globals.PlayerLifeCaches.Count - 1; i >= 0; i--)
            {
                if (MiscUtil.DiffMinutes(Globals.PlayerLifeCaches[i].CreateTime, DateTime.Now) > CacheTime)
                {
                    Globals.PlayerLifeCaches.RemoveAt(i);
                }
            }

            // 遍历玩家列表
            foreach (var item in Player.GetPlayerCache())
            {
                // 排除已经缓存过的数据
                if (Globals.PlayerLifeCaches.Find(x => x.PersonaId == item.PersonaId) != null)
                    continue;

                // 缓存玩家生涯KD、KPM
                var result1 = await BF1API.DetailedStatsByPersonaId(Globals.SessionId, item.PersonaId);
                if (result1.IsSuccess)
                {
                    // 缓存玩家生涯武器数据
                    var result2 = await BF1API.GetWeaponsByPersonaId(Globals.SessionId, item.PersonaId);
                    if (result2.IsSuccess)
                    {
                        // 缓存玩家生涯载具数据
                        var result3 = await BF1API.GetVehiclesByPersonaId(Globals.SessionId, item.PersonaId);
                        if (result3.IsSuccess)
                        {
                            Globals.PlayerLifeCaches.Add(new()
                            {
                                Name = item.Name,
                                PersonaId = item.PersonaId,
                                DetailedStatsJson = result1.Content,
                                GetWeaponsJson = result2.Content,
                                GetVehiclesJson = result3.Content,
                                CreateTime = DateTime.Now
                            });
                        }
                    }
                }
            }

            Thread.Sleep(1000);
        }
    }
}

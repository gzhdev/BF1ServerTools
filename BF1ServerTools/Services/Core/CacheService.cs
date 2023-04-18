using BF1ServerTools.API;
using BF1ServerTools.SDK;
using BF1ServerTools.Utils;
using BF1ServerTools.SQLite;

namespace BF1ServerTools.Services;

public static class CacheService
{
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
            foreach (var item in SQLiteApp.QueryLifeCacheDb())
            {
                if (MiscUtil.DiffMinutes(item.CreateTime, DateTime.Now) > 30)
                {
                    SQLiteApp.DeleteLifeCacheDb(item.PersonaId);
                }
            }

            // 遍历玩家列表
            foreach (var item in Player.GetPlayerCache())
            {
                if (SQLiteApp.QueryLifeCacheDb(item.PersonaId) == null)
                {
                    // 缓存玩家生涯KD、KPM
                    var result = await BF1API.DetailedStatsByPersonaId(Globals.SessionId, item.PersonaId);
                    if (result.IsSuccess)
                    {
                        var LifeCacheDb = new LifeCacheDb
                        {
                            Name = item.Name,
                            PersonaId = item.PersonaId,
                            CreateTime = DateTime.Now,
                            DetailedStatsJson = result.Content
                        };
                        SQLiteApp.AddLifeCacheDb(LifeCacheDb);
                    }
                }
            }

            Thread.Sleep(1000);
        }
    }
}

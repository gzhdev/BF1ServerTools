using BF1ServerTools.Data;
using BF1ServerTools.SQLite;
using BF1ServerTools.Helpers;

namespace BF1ServerTools.Services;

public static class ServiceApp
{
    /// <summary>
    /// 是否停止线程循环
    /// </summary>
    public static bool IsDispose { get; private set; } = false;

    private static Timer AutoRefreshTimerModel1 = null;
    private static Timer AutoRefreshTimerModel2 = null;

    /// <summary>
    /// 初始化服务
    /// </summary>
    public static void Initialize()
    {
        // 从数据库读取生涯数据缓存
        foreach (var item in SQLiteApp.ReadLifeCacheDb())
        {
            var lifeCaches = JsonHelper.JsonDeserialize<LifeCache>(item.LifeCacheJson);
            Globals.PlayerLifeCaches.Add(lifeCaches);
        }

        ////////////////////////////////////////////

        new Thread(MainService.UpdateMainDataThread)
        {
            Name = "UpdateMainDataThread",
            IsBackground = true
        }.Start();

        new Thread(GameService.UpdateGameInfoThread)
        {
            Name = "UpdateGameInfoThread",
            IsBackground = true
        }.Start();

        new Thread(ServerService.UpdateServerDetilsThread)
        {
            Name = "UpdateServerDetilsThread",
            IsBackground = true
        }.Start();

        new Thread(MonitService.UpdateBreakPlayerThread)
        {
            Name = "UpdateBreakPlayerThread",
            IsBackground = true
        }.Start();

        new Thread(CacheService.UpdateLifeCacheThread)
        {
            Name = "UpdateLifeCacheThread",
            IsBackground = true
        }.Start();

        ////////////////////////////////////////////

        // 模式1 定时内存扫描SessionId 周期10分钟
        AutoRefreshTimerModel1 = new Timer
        {
            AutoReset = true,
            Interval = TimeSpan.FromMinutes(10).TotalMilliseconds
        };
        AutoRefreshTimerModel1.Elapsed += AutoRefreshTimerModel1_Elapsed;
        AutoRefreshTimerModel1.Start();

        // 模式2 定时更新玩家Cookies 周期30分钟
        AutoRefreshTimerModel2 = new Timer
        {
            AutoReset = true,
            Interval = TimeSpan.FromMinutes(30).TotalMilliseconds
        };
        AutoRefreshTimerModel2.Elapsed += AutoRefreshTimerModel2_Elapsed;
        AutoRefreshTimerModel2.Start();
    }

    /// <summary>
    /// 结束服务线程
    /// </summary>
    public static void Shutdown()
    {
        IsDispose = true;

        AutoRefreshTimerModel1?.Stop();
        AutoRefreshTimerModel2?.Stop();

        // 保存生涯数据缓存到数据库
        var lifeCacheDbs = new List<LifeCacheSheet>();
        for (int i = 0; i < Globals.PlayerLifeCaches.Count; i++)
        {
            var item = Globals.PlayerLifeCaches[i];

            var lifeCacheJson = JsonHelper.JsonSerialize(item);
            lifeCacheDbs.Add(new()
            {
                Name = item.Name,
                PersonaId = item.PersonaId,
                LifeCacheJson = lifeCacheJson,
                CreateTime = item.CreateTime,
            });
        }
        SQLiteApp.SaveLifeCacheDb(lifeCacheDbs);
    }

    private static void AutoRefreshTimerModel1_Elapsed(object sender, ElapsedEventArgs e)
    {
        AuthService.UpdateMode1SessionId();
    }

    private static void AutoRefreshTimerModel2_Elapsed(object sender, ElapsedEventArgs e)
    {
        AuthService.UpdateMode2SessionId();
    }
}

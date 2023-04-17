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

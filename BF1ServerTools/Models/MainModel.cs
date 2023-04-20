using CommunityToolkit.Mvvm.ComponentModel;

namespace BF1ServerTools.Models;

public class MainModel : ObservableObject
{
    #region 程序版本号 VersionInfo
    private Version versionInfo;
    /// <summary>
    /// 程序版本号
    /// </summary>
    public Version VersionInfo
    {
        get => versionInfo;
        set => SetProperty(ref versionInfo, value);
    }
    #endregion

    #region 程序运行时间 AppRunTime
    private string appRunTime;
    /// <summary>
    /// 程序运行时间
    /// </summary>
    public string AppRunTime
    {
        get => appRunTime;
        set => SetProperty(ref appRunTime, value);
    }
    #endregion

    #region 是否使用模式1 IsUseMode1
    private bool isUseMode1;
    /// <summary>
    /// 是否使用模式1
    /// </summary>
    public bool IsUseMode1
    {
        get => isUseMode1;
        set => SetProperty(ref isUseMode1, value);
    }
    #endregion

    ////////////////////////////////////

    #region 模式1/2 显示名称 DisplayName
    private string displayName;
    /// <summary>
    /// 模式1/2 显示名称
    /// </summary>
    public string DisplayName
    {
        get => displayName;
        set => SetProperty(ref displayName, value);
    }
    #endregion

    #region 模式1/2 数字Id PersonaId
    private long personaId;
    /// <summary>
    /// 模式1/2 数字Id
    /// </summary>
    public long PersonaId
    {
        get => personaId;
        set => SetProperty(ref personaId, value);
    }
    #endregion

    ////////////////////////////////////

    #region 生涯缓存数量 CacheCount
    private int cacheCount;
    /// <summary>
    /// 生涯缓存数量
    /// </summary>
    public int CacheCount
    {
        get => cacheCount;
        set => SetProperty(ref cacheCount, value);
    }
    #endregion

    #region 当前服务器管理员数量 AdminCount
    private int adminCount;
    /// <summary>
    /// 当前服务器管理员数量
    /// </summary>
    public int AdminCount
    {
        get => adminCount;
        set => SetProperty(ref adminCount, value);
    }
    #endregion

    ////////////////////////////////////

    #region CPU使用量 UseCPU
    private float useCPU;
    /// <summary>
    /// CPU使用量
    /// </summary>
    public float UseCPU
    {
        get => useCPU;
        set => SetProperty(ref useCPU, value);
    }
    #endregion

    #region 内存使用量 UseRAM
    private float useRAM;
    /// <summary>
    /// 内存使用量
    /// </summary>
    public float UseRAM
    {
        get => useRAM;
        set => SetProperty(ref useRAM, value);
    }
    #endregion
}

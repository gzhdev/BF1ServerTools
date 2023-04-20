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

    #region 模式1 显示名称 DisplayName1
    private string displayName1;
    /// <summary>
    /// 模式1 显示名称
    /// </summary>
    public string DisplayName1
    {
        get => displayName1;
        set => SetProperty(ref displayName1, value);
    }
    #endregion

    #region 模式1 数字Id PersonaId1
    private long personaId1;
    /// <summary>
    /// 模式1 数字Id
    /// </summary>
    public long PersonaId1
    {
        get => personaId1;
        set => SetProperty(ref personaId1, value);
    }
    #endregion

    ////////////////////////////////////

    #region 模式2 显示名称 DisplayName2
    private string displayName2;
    /// <summary>
    /// 模式2 显示名称
    /// </summary>
    public string DisplayName2
    {
        get => displayName2;
        set => SetProperty(ref displayName2, value);
    }
    #endregion

    #region 模式2 数字Id PersonaId2
    private long personaId2;
    /// <summary>
    /// 模式2 数字Id
    /// </summary>
    public long PersonaId2
    {
        get => personaId2;
        set => SetProperty(ref personaId2, value);
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
}

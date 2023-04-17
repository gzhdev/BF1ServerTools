using CommunityToolkit.Mvvm.ComponentModel;

namespace BF1ServerTools.Models;

public class LoadModel : ObservableObject
{
    #region 程序加载状态 LoadState
    private string loadState;
    /// <summary>
    /// 程序加载状态
    /// </summary>
    public string LoadState
    {
        get => loadState;
        set => SetProperty(ref loadState, value);
    }
    #endregion

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

    #region 程序最后编译时间 BuildDate
    private DateTime buildDate;
    /// <summary>
    /// 程序最后编译时间
    /// </summary>
    public DateTime BuildDate
    {
        get => buildDate;
        set => SetProperty(ref buildDate, value);
    }
    #endregion

    #region 是否初始化错误 IsInitError
    private bool isInitError;
    /// <summary>
    /// 是否初始化错误
    /// </summary>
    public bool IsInitError
    {
        get => isInitError;
        set => SetProperty(ref isInitError, value);
    }
    #endregion
}

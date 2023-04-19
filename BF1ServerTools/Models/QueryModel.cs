using CommunityToolkit.Mvvm.ComponentModel;

namespace BF1ServerTools.Models;

public class QueryModel : ObservableObject
{
    #region 玩家名称 PlayerName
    private string playerName;
    /// <summary>
    /// 玩家名称
    /// </summary>
    public string PlayerName
    {
        get => playerName;
        set => SetProperty(ref playerName, value);
    }
    #endregion

    #region 是否显示加载动画 IsLoading
    private bool isLoading;
    /// <summary>
    /// 是否显示加载动画
    /// </summary>
    public bool IsLoading
    {
        get => isLoading;
        set => SetProperty(ref isLoading, value);
    }
    #endregion

    #region 玩家头像 Avatar
    private string avatar;
    /// <summary>
    /// 玩家头像
    /// </summary>
    public string Avatar
    {
        get => avatar;
        set => SetProperty(ref avatar, value);
    }
    #endregion

    #region 玩家图章 Emblem
    private string emblem;
    /// <summary>
    /// 玩家图章
    /// </summary>
    public string Emblem
    {
        get => emblem;
        set => SetProperty(ref emblem, value);
    }
    #endregion

    #region 玩家显示名称 DisplayName
    private string displayName;
    /// <summary>
    /// 玩家显示名称
    /// </summary>
    public string DisplayName
    {
        get => displayName;
        set => SetProperty(ref displayName, value);
    }
    #endregion

    #region 玩家数字ID PersonaId
    private string personaId;
    /// <summary>
    /// 玩家数字ID
    /// </summary>
    public string PersonaId
    {
        get => personaId;
        set => SetProperty(ref personaId, value);
    }
    #endregion

    #region 玩家等级 Rank
    private string rank;
    /// <summary>
    /// 玩家等级
    /// </summary>
    public string Rank
    {
        get => rank;
        set => SetProperty(ref rank, value);
    }
    #endregion

    #region 玩家游玩时间 PlayTime
    private string playTime;
    /// <summary>
    /// 玩家游玩时间
    /// </summary>
    public string PlayTime
    {
        get => playTime;
        set => SetProperty(ref playTime, value);
    }
    #endregion

    #region 玩家正在游玩服务器 PlayingServer
    private string playingServer;
    /// <summary>
    /// 玩家正在游玩服务器
    /// </summary>
    public string PlayingServer
    {
        get => playingServer;
        set => SetProperty(ref playingServer, value);
    }
    #endregion
}

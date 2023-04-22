using CommunityToolkit.Mvvm.ComponentModel;

namespace BF1ServerTools.Models;

public class MonitBreakModel : ObservableObject
{
    #region 序号 Index
    private int index;
    /// <summary>
    /// 序号
    /// </summary>
    public int Index
    {
        get => index;
        set => SetProperty(ref index, value);
    }
    #endregion

    ///////////////////////////////////

    #region 玩家等级 Rank
    private int rank;
    /// <summary>
    /// 玩家等级
    /// </summary>
    public int Rank
    {
        get => rank;
        set => SetProperty(ref rank, value);
    }
    #endregion

    #region 玩家昵称 Name
    private string name;
    /// <summary>
    /// 玩家昵称
    /// </summary>
    public string Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }
    #endregion

    #region 玩家数字Id PersonaId
    private long personaId;
    /// <summary>
    /// 玩家数字Id
    /// </summary>
    public long PersonaId
    {
        get => personaId;
        set => SetProperty(ref personaId, value);
    }
    #endregion

    ///////////////////////////////////

    #region 是否为管理员 IsAdmin
    private bool isAdmin;
    /// <summary>
    /// 是否为管理员
    /// </summary>
    public bool IsAdmin
    {
        get => isAdmin;
        set => SetProperty(ref isAdmin, value);
    }
    #endregion

    #region 是否为白名单 IsWhite
    private bool isWhite;
    /// <summary>
    /// 是否为白名单
    /// </summary>
    public bool IsWhite
    {
        get => isWhite;
        set => SetProperty(ref isWhite, value);
    }
    #endregion

    ///////////////////////////////////

    #region 违规原因 Reason
    private string reason;
    /// <summary>
    /// 违规原因
    /// </summary>
    public string Reason
    {
        get => reason;
        set => SetProperty(ref reason, value);
    }
    #endregion

    #region 违规数量 Count
    private int count;
    /// <summary>
    /// 违规数量
    /// </summary>
    public int Count
    {
        get => count;
        set => SetProperty(ref count, value);
    }
    #endregion

    #region 全部违规原因 AllReason
    private string allReason;
    /// <summary>
    /// 全部违规原因
    /// </summary>
    public string AllReason
    {
        get => allReason;
        set => SetProperty(ref allReason, value);
    }
    #endregion
}

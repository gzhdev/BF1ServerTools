using CommunityToolkit.Mvvm.ComponentModel;

namespace BF1ServerTools.Models;

public class ScoreTeamModel : ObservableObject
{
    #region 队伍比分 AllScore
    private int allScore;
    /// <summary>
    /// 队伍比分
    /// </summary>
    public int AllScore
    {
        get => allScore;
        set => SetProperty(ref allScore, value);
    }
    #endregion

    #region 队伍比分图形宽度 ScoreWidth
    private double scoreWidth;
    /// <summary>
    /// 队伍比分图形宽度
    /// </summary>
    public double ScoreWidth
    {
        get => scoreWidth;
        set => SetProperty(ref scoreWidth, value);
    }
    #endregion

    #region 队伍从旗帜获取的得分 ScoreFlag
    private int scoreFlag;
    /// <summary>
    /// 队伍从旗帜获取的得分
    /// </summary>
    public int ScoreFlag
    {
        get => scoreFlag;
        set => SetProperty(ref scoreFlag, value);
    }
    #endregion

    #region 队伍从击杀获取的得分 ScoreKill
    private int scoreKill;
    /// <summary>
    /// 队伍从击杀获取的得分
    /// </summary>
    public int ScoreKill
    {
        get => scoreKill;
        set => SetProperty(ref scoreKill, value);
    }
    #endregion

    #region 队伍图片 TeamImg
    private string teamImg;
    /// <summary>
    /// 队伍图片
    /// </summary>
    public string TeamImg
    {
        get => teamImg;
        set => SetProperty(ref teamImg, value);
    }
    #endregion

    #region 队伍名称 TeamName
    private string teamName;
    /// <summary>
    /// 队伍名称
    /// </summary>
    public string TeamName
    {
        get => teamName;
        set => SetProperty(ref teamName, value);
    }
    #endregion

    #region 队伍突击兵玩家数量 AssaultKitCount
    private int assaultKitCount;
    /// <summary>
    /// 队伍突击兵玩家数量
    /// </summary>
    public int AssaultKitCount
    {
        get => assaultKitCount;
        set => SetProperty(ref assaultKitCount, value);
    }
    #endregion

    #region 队伍医疗兵玩家数量 MedicKitCount
    private int medicKitCount;
    /// <summary>
    /// 队伍医疗兵玩家数量
    /// </summary>
    public int MedicKitCount
    {
        get => medicKitCount;
        set => SetProperty(ref medicKitCount, value);
    }
    #endregion

    #region 队伍支援兵玩家数量 SupportKitCount
    private int supportKitCount;
    /// <summary>
    /// 队伍支援兵玩家数量
    /// </summary>
    public int SupportKitCount
    {
        get => supportKitCount;
        set => SetProperty(ref supportKitCount, value);
    }
    #endregion

    #region 队伍侦查兵玩家数量 ScoutKitCount
    private int scoutKitCount;
    /// <summary>
    /// 队伍侦查兵玩家数量
    /// </summary>
    public int ScoutKitCount
    {
        get => scoutKitCount;
        set => SetProperty(ref scoutKitCount, value);
    }
    #endregion

    #region 队伍已部署玩家数量 PlayerCount
    private int playerCount;
    /// <summary>
    /// 队伍已部署玩家数量
    /// </summary>
    public int PlayerCount
    {
        get => playerCount;
        set => SetProperty(ref playerCount, value);
    }
    #endregion

    #region 队伍玩家数量 MaxPlayerCount
    private int maxPlayerCount;
    /// <summary>
    /// 队伍玩家数量
    /// </summary>
    public int MaxPlayerCount
    {
        get => maxPlayerCount;
        set => SetProperty(ref maxPlayerCount, value);
    }
    #endregion

    #region 队伍150等级玩家数量 Rank150PlayerCount
    private int rank150PlayerCount;
    /// <summary>
    /// 队伍150等级玩家数量
    /// </summary>
    public int Rank150PlayerCount
    {
        get => rank150PlayerCount;
        set => SetProperty(ref rank150PlayerCount, value);
    }
    #endregion

    #region 队伍总击杀数 AllKillCount
    private int allKillCount;
    /// <summary>
    /// 队伍总击杀数
    /// </summary>
    public int AllKillCount
    {
        get => allKillCount;
        set => SetProperty(ref allKillCount, value);
    }
    #endregion

    #region 队伍总死亡数 AllDeadCount
    private int allDeadCount;
    /// <summary>
    /// 队伍总死亡数
    /// </summary>
    public int AllDeadCount
    {
        get => allDeadCount;
        set => SetProperty(ref allDeadCount, value);
    }
    #endregion
}

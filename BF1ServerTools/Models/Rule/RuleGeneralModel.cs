using CommunityToolkit.Mvvm.ComponentModel;

namespace BF1ServerTools.Models;

public class RuleGeneralModel : ObservableObject
{
    #region 最大击杀 MaxKill
    private int maxKill;
    /// <summary>
    /// 最大击杀
    /// </summary>
    public int MaxKill
    {
        get => maxKill;
        set => SetProperty(ref maxKill, value);
    }
    #endregion

    #region 计算KD标志 FlagKD
    private int flagKD;
    /// <summary>
    /// 计算KD标志
    /// </summary>
    public int FlagKD
    {
        get => flagKD;
        set => SetProperty(ref flagKD, value);
    }
    #endregion

    #region 最大KD MaxKD
    private float maxKD;
    /// <summary>
    /// 最大KD
    /// </summary>
    public float MaxKD
    {
        get => maxKD;
        set => SetProperty(ref maxKD, value);
    }
    #endregion

    #region 计算KPM标志 FlagKPM
    private int flagKPM;
    /// <summary>
    /// 计算KPM标志
    /// </summary>
    public int FlagKPM
    {
        get => flagKPM;
        set => SetProperty(ref flagKPM, value);
    }
    #endregion

    #region 最大KPM MaxKPM
    private float maxKPM;
    /// <summary>
    /// 最大KPM
    /// </summary>
    public float MaxKPM
    {
        get => maxKPM;
        set => SetProperty(ref maxKPM, value);
    }
    #endregion

    #region 最低等级 MinRank
    private int minRank;
    /// <summary>
    /// 最低等级
    /// </summary>
    public int MinRank
    {
        get => minRank;
        set => SetProperty(ref minRank, value);
    }
    #endregion

    #region 最低等级 MaxRank
    private int maxRank;
    /// <summary>
    /// 最低等级
    /// </summary>
    public int MaxRank
    {
        get => maxRank;
        set => SetProperty(ref maxRank, value);
    }
    #endregion
}

using BF1ServerTools.Data;
using BF1ServerTools.Models;

namespace BF1ServerTools.Views.Rule;

/// <summary>
/// GeneralView.xaml 的交互逻辑
/// </summary>
public partial class GeneralView : UserControl
{
    /// <summary>
    /// 绑定UI 队伍1规则集
    /// </summary>
    public RuleGeneralModel RuleGeneral1Model { get; set; } = new();
    /// <summary>
    /// 绑定UI 队伍2规则集
    /// </summary>
    public RuleGeneralModel RuleGeneral2Model { get; set; } = new();

    ////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 获取队伍1当局规则数据委托
    /// </summary>
    public static Func<GeneralData> FuncGetTeam1GeneralData;
    /// <summary>
    /// 获取队伍2当局规则数据委托
    /// </summary>
    public static Func<GeneralData> FuncGetTeam2GeneralData;

    /// <summary>
    /// 更新队伍1当局规则数据委托
    /// </summary>
    public static Action<GeneralData> ActionUpdateTeam1GeneralData;
    /// <summary>
    ///  更新队伍2当局规则数据委托
    /// </summary>
    public static Action<GeneralData> ActionUpdateTeam2GeneralData;

    public GeneralView()
    {
        InitializeComponent();

        FuncGetTeam1GeneralData = GetTeam1GeneralData;
        FuncGetTeam2GeneralData = GetTeam2GeneralData;

        ActionUpdateTeam1GeneralData = UpdateTeam1GeneralData;
        ActionUpdateTeam2GeneralData = UpdateTeam2GeneralData;

        RuleView.UpdateCurrentRuleEvent += RuleView_UpdateCurrentRuleEvent;
    }

    private void RuleView_UpdateCurrentRuleEvent()
    {
        UpdateCurrentRule();
    }

    private void UpdateCurrentRule()
    {
        Globals.ServerRule_Team1.MaxKill = RuleGeneral1Model.MaxKill;
        Globals.ServerRule_Team1.FlagKD = RuleGeneral1Model.FlagKD;
        Globals.ServerRule_Team1.MaxKD = RuleGeneral1Model.MaxKD;
        Globals.ServerRule_Team1.FlagKPM = RuleGeneral1Model.FlagKPM;
        Globals.ServerRule_Team1.MaxKPM = RuleGeneral1Model.MaxKPM;
        Globals.ServerRule_Team1.MinRank = RuleGeneral1Model.MinRank;
        Globals.ServerRule_Team1.MaxRank = RuleGeneral1Model.MaxRank;

        Globals.ServerRule_Team2.MaxKill = RuleGeneral2Model.MaxKill;
        Globals.ServerRule_Team2.FlagKD = RuleGeneral2Model.FlagKD;
        Globals.ServerRule_Team2.MaxKD = RuleGeneral2Model.MaxKD;
        Globals.ServerRule_Team2.FlagKPM = RuleGeneral2Model.FlagKPM;
        Globals.ServerRule_Team2.MaxKPM = RuleGeneral2Model.MaxKPM;
        Globals.ServerRule_Team2.MinRank = RuleGeneral2Model.MinRank;
        Globals.ServerRule_Team2.MaxRank = RuleGeneral2Model.MaxRank;
    }

    /// <summary>
    /// 获取队伍1当局规则数据
    /// </summary>
    /// <returns></returns>
    private GeneralData GetTeam1GeneralData()
    {
        return new GeneralData
        {
            MaxKill = RuleGeneral1Model.MaxKill,
            FlagKD = RuleGeneral1Model.FlagKD,
            MaxKD = RuleGeneral1Model.MaxKD,
            FlagKPM = RuleGeneral1Model.FlagKPM,
            MaxKPM = RuleGeneral1Model.MaxKPM,
            MinRank = RuleGeneral1Model.MinRank,
            MaxRank = RuleGeneral1Model.MaxRank
        };
    }

    /// <summary>
    /// 获取队伍2当局规则数据
    /// </summary>
    /// <returns></returns>
    private GeneralData GetTeam2GeneralData()
    {
        return new GeneralData
        {
            MaxKill = RuleGeneral2Model.MaxKill,
            FlagKD = RuleGeneral2Model.FlagKD,
            MaxKD = RuleGeneral2Model.MaxKD,
            FlagKPM = RuleGeneral2Model.FlagKPM,
            MaxKPM = RuleGeneral2Model.MaxKPM,
            MinRank = RuleGeneral2Model.MinRank,
            MaxRank = RuleGeneral2Model.MaxRank
        };
    }

    /// <summary>
    /// 更新队伍1当局规则数据
    /// </summary>
    /// <returns></returns>
    private void UpdateTeam1GeneralData(GeneralData generalData)
    {
        RuleGeneral1Model.MaxKill = generalData.MaxKill;
        RuleGeneral1Model.FlagKD = generalData.FlagKD;
        RuleGeneral1Model.MaxKD = generalData.MaxKD;
        RuleGeneral1Model.FlagKPM = generalData.FlagKPM;
        RuleGeneral1Model.MaxKPM = generalData.MaxKPM;
        RuleGeneral1Model.MinRank = generalData.MinRank;
        RuleGeneral1Model.MaxRank = generalData.MaxRank;
    }

    /// <summary>
    /// 更新队伍2当局规则数据
    /// </summary>
    /// <returns></returns>
    private void UpdateTeam2GeneralData(GeneralData generalData)
    {
        RuleGeneral2Model.MaxKill = generalData.MaxKill;
        RuleGeneral2Model.FlagKD = generalData.FlagKD;
        RuleGeneral2Model.MaxKD = generalData.MaxKD;
        RuleGeneral2Model.FlagKPM = generalData.FlagKPM;
        RuleGeneral2Model.MaxKPM = generalData.MaxKPM;
        RuleGeneral2Model.MinRank = generalData.MinRank;
        RuleGeneral2Model.MaxRank = generalData.MaxRank;
    }
}

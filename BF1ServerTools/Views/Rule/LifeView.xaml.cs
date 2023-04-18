using BF1ServerTools.Data;
using BF1ServerTools.Models;

namespace BF1ServerTools.Views.Rule;

/// <summary>
/// LifeView.xaml 的交互逻辑
/// </summary>
public partial class LifeView : UserControl
{
    /// <summary>
    /// 绑定UI 队伍1规则集
    /// </summary>
    public RuleLifeModel RuleLife1Model { get; set; } = new();
    /// <summary>
    /// 绑定UI 队伍2规则集
    /// </summary>
    public RuleLifeModel RuleLife2Model { get; set; } = new();

    ////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 获取队伍1生涯规则数据委托
    /// </summary>
    public static Func<LifeData> FuncGetTeam1LifeData;
    /// <summary>
    /// 获取队伍2生涯规则数据委托
    /// </summary>
    public static Func<LifeData> FuncGetTeam2LifeData;

    /// <summary>
    /// 设置队伍1生涯规则数据委托
    /// </summary>
    public static Action<LifeData> ActionSetTeam1LifeData;
    /// <summary>
    /// 设置队伍2生涯规则数据委托
    /// </summary>
    public static Action<LifeData> ActionSetTeam2LifeData;

    ////////////////////////////////////////////////////////////////////

    public LifeView()
    {
        InitializeComponent();

        FuncGetTeam1LifeData = GetTeam1LifeData;
        FuncGetTeam2LifeData = GetTeam2LifeData;

        ActionSetTeam1LifeData = SetTeam1LifeData;
        ActionSetTeam2LifeData = SetTeam2LifeData;

        RuleView.ApplyCurrentRuleEvent += RuleView_ApplyCurrentRuleEvent;
    }

    private void RuleView_ApplyCurrentRuleEvent()
    {
        ApplyCurrentRule();
    }

    private void ApplyCurrentRule()
    {
        Globals.ServerRule_Team1.LifeMaxKD = RuleLife1Model.LifeMaxKD;
        Globals.ServerRule_Team1.LifeMaxKPM = RuleLife1Model.LifeMaxKPM;
        Globals.ServerRule_Team1.LifeMaxWeaponStar = RuleLife1Model.LifeMaxWeaponStar;
        Globals.ServerRule_Team1.LifeMaxVehicleStar = RuleLife1Model.LifeMaxVehicleStar;

        Globals.ServerRule_Team2.LifeMaxKD = RuleLife2Model.LifeMaxKD;
        Globals.ServerRule_Team2.LifeMaxKPM = RuleLife2Model.LifeMaxKPM;
        Globals.ServerRule_Team2.LifeMaxWeaponStar = RuleLife2Model.LifeMaxWeaponStar;
        Globals.ServerRule_Team2.LifeMaxVehicleStar = RuleLife2Model.LifeMaxVehicleStar;
    }

    /// <summary>
    /// 获取队伍1生涯规则数据
    /// </summary>
    /// <returns></returns>
    private LifeData GetTeam1LifeData()
    {
        return new LifeData
        {
            LifeMaxKD = RuleLife1Model.LifeMaxKD,
            LifeMaxKPM = RuleLife1Model.LifeMaxKPM,
            LifeMaxWeaponStar = RuleLife1Model.LifeMaxWeaponStar,
            LifeMaxVehicleStar = RuleLife1Model.LifeMaxVehicleStar
        };
    }

    /// <summary>
    /// 获取队伍2生涯规则数据
    /// </summary>
    /// <returns></returns>
    private LifeData GetTeam2LifeData()
    {
        return new LifeData
        {
            LifeMaxKD = RuleLife2Model.LifeMaxKD,
            LifeMaxKPM = RuleLife2Model.LifeMaxKPM,
            LifeMaxWeaponStar = RuleLife2Model.LifeMaxWeaponStar,
            LifeMaxVehicleStar = RuleLife2Model.LifeMaxVehicleStar
        };
    }

    /// <summary>
    /// 设置队伍1生涯规则数据
    /// </summary>
    /// <returns></returns>
    private void SetTeam1LifeData(LifeData LifeData)
    {
        RuleLife1Model.LifeMaxKD = LifeData.LifeMaxKD;
        RuleLife1Model.LifeMaxKPM = LifeData.LifeMaxKPM;
        RuleLife1Model.LifeMaxWeaponStar = LifeData.LifeMaxWeaponStar;
        RuleLife1Model.LifeMaxVehicleStar = LifeData.LifeMaxVehicleStar;
    }

    /// <summary>
    /// 设置队伍2生涯规则数据
    /// </summary>
    /// <returns></returns>
    private void SetTeam2LifeData(LifeData LifeData)
    {
        RuleLife2Model.LifeMaxKD = LifeData.LifeMaxKD;
        RuleLife2Model.LifeMaxKPM = LifeData.LifeMaxKPM;
        RuleLife2Model.LifeMaxWeaponStar = LifeData.LifeMaxWeaponStar;
        RuleLife2Model.LifeMaxVehicleStar = LifeData.LifeMaxVehicleStar;
    }
}

using BF1ServerTools.Data;
using BF1ServerTools.Services;

namespace BF1ServerTools.Views.Rule;

/// <summary>
/// QueryView.xaml 的交互逻辑
/// </summary>
public partial class QueryView : UserControl
{
    /// <summary>
    /// 绑定UI 规则信息
    /// </summary>
    public ObservableCollection<RuleInfo> DataGrid_RuleInfos { get; set; } = new();

    public QueryView()
    {
        InitializeComponent();

        RuleView.QueryCurrentRuleEvent += RuleView_QueryCurrentRuleEvent;
    }

    private void RuleView_QueryCurrentRuleEvent()
    {
        ClearRuleInfo();

        AddRuleInfo("【当局规则】", "最高击杀限制", $"{Globals.ServerRule_Team1.MaxKill}", $"{Globals.ServerRule_Team2.MaxKill}");

        AddRuleInfo("【当局规则】", "计算KD的最低击杀数", $"{Globals.ServerRule_Team1.FlagKD}", $"{Globals.ServerRule_Team2.FlagKD}");
        AddRuleInfo("【当局规则】", "最高KD限制", $"{Globals.ServerRule_Team1.MaxKD}", $"{Globals.ServerRule_Team2.MaxKD}");

        AddRuleInfo("【当局规则】", "计算KPM的最低击杀数", $"{Globals.ServerRule_Team1.FlagKPM}", $"{Globals.ServerRule_Team2.FlagKPM}");
        AddRuleInfo("【当局规则】", "最高KPM限制", $"{Globals.ServerRule_Team1.MaxKPM}", $"{Globals.ServerRule_Team2.MaxKPM}");

        AddRuleInfo("【当局规则】", "最低等级限制", $"{Globals.ServerRule_Team1.MinRank}", $"{Globals.ServerRule_Team2.MinRank}");
        AddRuleInfo("【当局规则】", "最高等级限制", $"{Globals.ServerRule_Team1.MaxRank}", $"{Globals.ServerRule_Team2.MaxRank}");

        ////////////////////////////////

        AddRuleInfo();

        AddRuleInfo("【生涯规则】", "最高生涯KD限制", $"{Globals.ServerRule_Team1.LifeMaxKD}", $"{Globals.ServerRule_Team2.LifeMaxKD}");
        AddRuleInfo("【生涯规则】", "最高生涯KPM限制", $"{Globals.ServerRule_Team1.LifeMaxKPM}", $"{Globals.ServerRule_Team2.LifeMaxKPM}");

        AddRuleInfo("【生涯规则】", "最高生涯武器星数限制", $"{Globals.ServerRule_Team1.LifeMaxWeaponStar}", $"{Globals.ServerRule_Team2.LifeMaxWeaponStar}");
        AddRuleInfo("【生涯规则】", "最高生涯载具星数限制", $"{Globals.ServerRule_Team1.LifeMaxVehicleStar}", $"{Globals.ServerRule_Team2.LifeMaxVehicleStar}");

        ////////////////////////////////

        AddRuleInfo();

        int team1 = Globals.CustomWeapons_Team1.Count;
        int team2 = Globals.CustomWeapons_Team2.Count;
        for (int i = 0; i < Math.Max(team1, team2); i++)
        {
            if (i < team1 && i < team2)
            {
                // 共有禁用武器
                AddRuleInfo("【禁用武器】", $"武器名称 {i + 1}", $"{ClientUtil.GetWeaponChsName(Globals.CustomWeapons_Team1[i])}", $"{ClientUtil.GetWeaponChsName(Globals.CustomWeapons_Team2[i])}");
            }
            else if (i < team1)
            {
                // 队伍1禁用武器
                AddRuleInfo("【禁用武器】", $"武器名称 {i + 1}", $"{ClientUtil.GetWeaponChsName(Globals.CustomWeapons_Team1[i])}", string.Empty);
            }
            else if (i < team2)
            {
                // 队伍2禁用武器
                AddRuleInfo("【禁用武器】", $"武器名称 {i + 1}", string.Empty, $"{ClientUtil.GetWeaponChsName(Globals.CustomWeapons_Team2[i])}");
            }
        }

        ////////////////////////////////

        AddRuleInfo();


    }

    /// <summary>
    /// 添加规则信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="Name"></param>
    /// <param name="t1Value"></param>
    /// <param name="t2Value"></param>
    private void AddRuleInfo(string type = "", string Name = "", string t1Value = "", string t2Value = "")
    {
        DataGrid_RuleInfos.Add(new()
        {
            Type = type,
            Name = Name,
            T1Value = t1Value,
            T2Value = t2Value
        });
    }

    /// <summary>
    /// 清空规则信息
    /// </summary>
    private void ClearRuleInfo()
    {
        DataGrid_RuleInfos.Clear();
    }
}

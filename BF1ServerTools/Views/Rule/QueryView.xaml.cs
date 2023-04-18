using BF1ServerTools.Data;
using BF1ServerTools.Helpers;
using BF1ServerTools.Services;

namespace BF1ServerTools.Views.Rule;

/// <summary>
/// QueryView.xaml 的交互逻辑
/// </summary>
public partial class QueryView : UserControl
{
    /// <summary>
    /// 绑定UI 规则日志信息
    /// </summary>
    public ObservableCollection<RuleLog> DataGrid_RuleLogs { get; set; } = new();

    public QueryView()
    {
        InitializeComponent();

        RuleView.QueryCurrentRuleEvent += RuleView_QueryCurrentRuleEvent;
    }

    private void RuleView_QueryCurrentRuleEvent()
    {
        ClearRuleLog();
        AddHeader("【当局规则】");

        AddRuleLog("【当局规则】", "最高击杀限制", $"{Globals.ServerRule_Team1.MaxKill}", $"{Globals.ServerRule_Team2.MaxKill}");

        AddRuleLog("【当局规则】", "计算KD的最低击杀数", $"{Globals.ServerRule_Team1.FlagKD}", $"{Globals.ServerRule_Team2.FlagKD}");
        AddRuleLog("【当局规则】", "最高KD限制", $"{Globals.ServerRule_Team1.MaxKD}", $"{Globals.ServerRule_Team2.MaxKD}");

        AddRuleLog("【当局规则】", "计算KPM的最低击杀数", $"{Globals.ServerRule_Team1.FlagKPM}", $"{Globals.ServerRule_Team2.FlagKPM}");
        AddRuleLog("【当局规则】", "最高KPM限制", $"{Globals.ServerRule_Team1.MaxKPM}", $"{Globals.ServerRule_Team2.MaxKPM}");

        AddRuleLog("【当局规则】", "最低等级限制", $"{Globals.ServerRule_Team1.MinRank}", $"{Globals.ServerRule_Team2.MinRank}");
        AddRuleLog("【当局规则】", "最高等级限制", $"{Globals.ServerRule_Team1.MaxRank}", $"{Globals.ServerRule_Team2.MaxRank}");

        ////////////////////////////////

        AddRuleLog();
        AddHeader("【生涯规则】");

        AddRuleLog("【生涯规则】", "最高生涯KD限制", $"{Globals.ServerRule_Team1.LifeMaxKD}", $"{Globals.ServerRule_Team2.LifeMaxKD}");
        AddRuleLog("【生涯规则】", "最高生涯KPM限制", $"{Globals.ServerRule_Team1.LifeMaxKPM}", $"{Globals.ServerRule_Team2.LifeMaxKPM}");

        AddRuleLog("【生涯规则】", "最高生涯武器星数限制", $"{Globals.ServerRule_Team1.LifeMaxWeaponStar}", $"{Globals.ServerRule_Team2.LifeMaxWeaponStar}");
        AddRuleLog("【生涯规则】", "最高生涯载具星数限制", $"{Globals.ServerRule_Team1.LifeMaxVehicleStar}", $"{Globals.ServerRule_Team2.LifeMaxVehicleStar}");

        ////////////////////////////////

        AddRuleLog();
        AddHeader("【禁用武器】");

        int team1 = Globals.CustomWeapons_Team1.Count;
        int team2 = Globals.CustomWeapons_Team2.Count;
        for (int i = 0; i < Math.Max(team1, team2); i++)
        {
            if (i < team1 && i < team2)
            {
                // 共有禁用武器
                AddRuleLog("【禁用武器】", $"武器名称 {i + 1}", $"{ClientUtil.GetWeaponChsName(Globals.CustomWeapons_Team1[i])}", $"{ClientUtil.GetWeaponChsName(Globals.CustomWeapons_Team2[i])}");
            }
            else if (i < team1)
            {
                // 队伍1禁用武器
                AddRuleLog("【禁用武器】", $"武器名称 {i + 1}", $"{ClientUtil.GetWeaponChsName(Globals.CustomWeapons_Team1[i])}", string.Empty);
            }
            else if (i < team2)
            {
                // 队伍2禁用武器
                AddRuleLog("【禁用武器】", $"武器名称 {i + 1}", string.Empty, $"{ClientUtil.GetWeaponChsName(Globals.CustomWeapons_Team2[i])}");
            }
        }

        ////////////////////////////////

        AddRuleLog();
        AddHeader("【白名单特权】");

        if (Globals.WhiteKill)
            AddRuleLog("【白名单特权】", "免疫击杀限制", "✔");
        if (Globals.WhiteKD)
            AddRuleLog("【白名单特权】", "免疫KD限制", "✔");
        if (Globals.WhiteKPM)
            AddRuleLog("【白名单特权】", "免疫KPM限制", "✔");
        if (Globals.WhiteRank)
            AddRuleLog("【白名单特权】", "免疫等级限制", "✔");
        if (Globals.WhiteWeapon)
            AddRuleLog("【白名单特权】", "免疫武器限制", "✔");

        if (Globals.WhiteLifeKD)
            AddRuleLog("【白名单特权】", "免疫生涯KD限制", "✔");
        if (Globals.WhiteLifeKPM)
            AddRuleLog("【白名单特权】", "免疫生涯KPM限制", "✔");
        if (Globals.WhiteLifeWeaponStar)
            AddRuleLog("【白名单特权】", "免疫生涯武器星数限制", "✔");
        if (Globals.WhiteLifeVehicleStar)
            AddRuleLog("【白名单特权】", "免疫生涯载具星数限制", "✔");

        ////////////////////////////////

        AddRuleLog();
        AddHeader("【白名单列表】");

        int index = 1;
        foreach (var item in Globals.CustomWhites_Name)
        {
            AddRuleLog("【白名单列表】", $"玩家ID {index++}", $"{item}");
        }

        ////////////////////////////////

        AddRuleLog();
        AddHeader("【黑名单列表】");

        index = 1;
        foreach (var item in Globals.CustomBlacks_Name)
        {
            AddRuleLog("【黑名单列表】", $"玩家ID {index++}", $"{item}");
        }

        ////////////////////////////////

        NotifierHelper.Show(NotifierType.Success, "查询当前规则成功");
    }

    ////////////////////////////////////////////////////////////////

    /// <summary>
    /// 增加标题
    /// </summary>
    /// <param name="header"></param>
    private void AddHeader(string header)
    {
        AddRuleLog(string.Empty, header);
    }

    /// <summary>
    /// 添加规则日志信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="Name"></param>
    /// <param name="t1Value"></param>
    /// <param name="t2Value"></param>
    private void AddRuleLog(string type = "", string Name = "", string t1Value = "", string t2Value = "")
    {
        DataGrid_RuleLogs.Add(new()
        {
            Type = type,
            Name = Name,
            T1Value = t1Value,
            T2Value = t2Value
        });
    }

    /// <summary>
    /// 清空规则日志信息
    /// </summary>
    private void ClearRuleLog()
    {
        DataGrid_RuleLogs.Clear();
    }
}

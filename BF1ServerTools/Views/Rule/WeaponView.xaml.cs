using BF1ServerTools.Models;
using BF1ServerTools.Services;

namespace BF1ServerTools.Views.Rule;

/// <summary>
/// WeaponView.xaml 的交互逻辑
/// </summary>
public partial class WeaponView : UserControl
{
    /// <summary>
    /// 绑定UI 武器数据
    /// </summary>
    public ObservableCollection<RuleWeaponModel> DataGrid_RuleWeaponModels { get; set; } = new();

    ////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 获取队伍1武器规则数据委托
    /// </summary>
    public static Func<List<string>> FuncGetTeam1WeaponData;
    /// <summary>
    /// 获取队伍2武器规则数据委托
    /// </summary>
    public static Func<List<string>> FuncGetTeam2WeaponData;

    /// <summary>
    /// 设置队伍1武器规则数据委托
    /// </summary>
    public static Action<List<string>> ActionSetTeam1WeaponData;
    /// <summary>
    /// 设置队伍2武器规则数据委托
    /// </summary>
    public static Action<List<string>> ActionSetTeam2WeaponData;

    ////////////////////////////////////////////////////////////////////

    public WeaponView()
    {
        InitializeComponent();

        FuncGetTeam1WeaponData = GetTeam1WeaponData;
        FuncGetTeam2WeaponData = GetTeam2WeaponData;

        ActionSetTeam1WeaponData = SetTeam1WeaponData;
        ActionSetTeam2WeaponData = SetTeam2WeaponData;

        RuleView.ApplyCurrentRuleEvent += RuleView_ApplyCurrentRuleEvent;

        // 添加武器数据列表
        foreach (var item in WeaponDB.AllWeaponInfo)
        {
            DataGrid_RuleWeaponModels.Add(new()
            {
                Kind = item.Kind,
                Name = item.Chinese,
                English = item.English,
                Image = ClientUtil.GetWeaponImagePath(item.English),
                Team1 = false,
                Team2 = false
            });
        }
    }

    private void RuleView_ApplyCurrentRuleEvent()
    {
        // 清空限制武器列表
        Globals.CustomWeapons_Team1.Clear();
        Globals.CustomWeapons_Team2.Clear();

        // 添加自定义限制武器
        foreach (var item in DataGrid_RuleWeaponModels)
        {
            if (item.Team1)
                Globals.CustomWeapons_Team1.Add(item.English);

            if (item.Team2)
                Globals.CustomWeapons_Team2.Add(item.English);
        }
    }

    /// <summary>
    /// 获取队伍1武器规则数据
    /// </summary>
    /// <returns></returns>
    private List<string> GetTeam1WeaponData()
    {
        var list = new List<string>();

        foreach (var item in DataGrid_RuleWeaponModels)
        {
            if (item.Team1)
            {
                list.Add(item.English);
            }
        }

        return list;
    }

    /// <summary>
    /// 获取队伍2武器规则数据
    /// </summary>
    /// <returns></returns>
    private List<string> GetTeam2WeaponData()
    {
        var list = new List<string>();

        foreach (var item in DataGrid_RuleWeaponModels)
        {
            if (item.Team2)
            {
                list.Add(item.English);
            }
        }

        return list;
    }

    /// <summary>
    /// 设置队伍1武器规则数据
    /// </summary>
    /// <param name="weaponList"></param>
    private void SetTeam1WeaponData(List<string> weaponList)
    {
        foreach (var item in DataGrid_RuleWeaponModels)
        {
            if (weaponList.Contains(item.English))
                item.Team1 = true;
            else
                item.Team1 = false;
        }
    }

    /// <summary>
    /// 设置队伍2武器规则数据
    /// </summary>
    /// <param name="weaponList"></param>
    private void SetTeam2WeaponData(List<string> weaponList)
    {
        foreach (var item in DataGrid_RuleWeaponModels)
        {
            if (weaponList.Contains(item.English))
                item.Team2 = true;
            else
                item.Team2 = false;
        }
    }
}

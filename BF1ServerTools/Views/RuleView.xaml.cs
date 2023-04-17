using BF1ServerTools.Configs;
using BF1ServerTools.Helpers;
using BF1ServerTools.Views.Rule;

namespace BF1ServerTools.Views;

/// <summary>
/// RuleView.xaml 的交互逻辑
/// </summary>
public partial class RuleView : UserControl
{
    /// <summary>
    /// 更新当前规则事件
    /// </summary>
    public static event Action UpdateCurrentRuleEvent;
    /// <summary>
    /// 查询当前规则事件
    /// </summary>
    public static event Action QueryCurrentRuleEvent;

    ////////////////////////////////////////////////////////////////////

    /// <summary>
    ///配置文件路径
    /// </summary>
    private readonly string File_Rule_Config = Path.Combine(FileHelper.Dir_Config, "RuleConfig.json");

    /// <summary>
    /// Rule配置文件，以json格式保存到本地
    /// </summary>
    private RuleConfig RuleConfig = new();

    /// <summary>
    /// 绑定UI 配置文件名称动态集合
    /// </summary>
    public ObservableCollection<string> ConfigNames { get; set; } = new();

    public RuleView()
    {
        InitializeComponent();
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;

        #region 配置文件
        // 如果配置文件不存在就创建（第一次创建）
        if (!File.Exists(File_Rule_Config))
        {
            RuleConfig.SelectedIndex = 0;
            RuleConfig.RuleInfos = new();
            // 初始化10个配置文件槽
            for (int i = 0; i < 10; i++)
            {
                RuleConfig.RuleInfos.Add(new()
                {
                    RuleName = $"自定义规则 {i}",
                    WhiteLifeKD = true,
                    WhiteLifeKPM = true,
                    WhiteLifeWeaponStar = true,
                    WhiteLifeVehicleStar = true,
                    WhiteKill = true,
                    WhiteKD = true,
                    WhiteKPM = true,
                    WhiteRank = true,
                    WhiteWeapon = true,
                    Team1Rule = new(),
                    Team2Rule = new(),
                    Team1Weapon = new(),
                    Team2Weapon = new(),
                    BlackList = new(),
                    WhiteList = new()
                });
            }
            // 保存配置文件
            SaveConfig();
        }

        // 如果配置文件存在就读取
        if (File.Exists(File_Rule_Config))
        {
            using var streamReader = new StreamReader(File_Rule_Config);
            RuleConfig = JsonHelper.JsonDese<RuleConfig>(streamReader.ReadToEnd());
            streamReader.Close();

            // 读取配置文件名称
            foreach (var item in RuleConfig.RuleInfos)
                ConfigNames.Add(item.RuleName);
            // 读取选中配置文件索引
            ComboBox_ConfigNames.SelectedIndex = RuleConfig.SelectedIndex;
        }
        #endregion
    }

    /// <summary>
    /// 主窗口关闭事件
    /// </summary>
    private void MainWindow_WindowClosingEvent()
    {
        SaveConfig();
    }

    /// <summary>
    /// 保存配置文件
    /// </summary>
    private void SaveConfig()
    {
        // 更新当前授权信息
        var index = ComboBox_ConfigNames.SelectedIndex;
        if (index != -1)
        {
            RuleConfig.SelectedIndex = index;

            var rule = RuleConfig.RuleInfos[index];

            var team1General = GeneralView.FuncGetTeam1GeneralData();
            var team2General = GeneralView.FuncGetTeam2GeneralData();

            rule.Team1Rule.MaxKill = team1General.MaxKill;
            rule.Team1Rule.FlagKD = team1General.FlagKD;
            rule.Team1Rule.MaxKD = team1General.MaxKD;
            rule.Team1Rule.FlagKPM = team1General.FlagKPM;
            rule.Team1Rule.MaxKPM = team1General.MaxKPM;
            rule.Team1Rule.MinRank = team1General.MinRank;
            rule.Team1Rule.MaxRank = team1General.MaxRank;

            rule.Team2Rule.MaxKill = team2General.MaxKill;
            rule.Team2Rule.FlagKD = team2General.FlagKD;
            rule.Team2Rule.MaxKD = team2General.MaxKD;
            rule.Team2Rule.FlagKPM = team2General.FlagKPM;
            rule.Team2Rule.MaxKPM = team2General.MaxKPM;
            rule.Team2Rule.MinRank = team2General.MinRank;
            rule.Team2Rule.MaxRank = team2General.MaxRank;

            var team1Life = LifeView.FuncGetTeam1LifeData();
            var team2Life = LifeView.FuncGetTeam2LifeData();

            rule.Team1Rule.LifeMaxKD = team1Life.LifeMaxKD;
            rule.Team1Rule.LifeMaxKPM = team1Life.LifeMaxKPM;
            rule.Team1Rule.LifeMaxWeaponStar = team1Life.LifeMaxWeaponStar;
            rule.Team1Rule.LifeMaxVehicleStar = team1Life.LifeMaxVehicleStar;

            rule.Team2Rule.LifeMaxKD = team2Life.LifeMaxKD;
            rule.Team2Rule.LifeMaxKPM = team2Life.LifeMaxKPM;
            rule.Team2Rule.LifeMaxWeaponStar = team2Life.LifeMaxWeaponStar;
            rule.Team2Rule.LifeMaxVehicleStar = team2Life.LifeMaxVehicleStar;
        }

        File.WriteAllText(File_Rule_Config, JsonHelper.JsonSeri(RuleConfig));
    }

    /// <summary>
    /// 查询当前规则
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_QueryCurrentRule_Click(object sender, RoutedEventArgs e)
    {
        // 切换到第一个页面
        TabControl_RuleView.SelectedIndex = 0;
        // 查询当前规则
        QueryCurrentRuleEvent?.Invoke();
    }

    /// <summary>
    /// 应用并查询当前规则
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_ApplyAndQueryCurrentRule_Click(object sender, RoutedEventArgs e)
    {
        // 切换到第一个页面
        TabControl_RuleView.SelectedIndex = 0;
        // 应用当前规则
        UpdateCurrentRuleEvent?.Invoke();
        // 查询当前规则
        QueryCurrentRuleEvent?.Invoke();
    }

    /// <summary>
    /// ComboBox选中项变更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ComboBox_ConfigNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var index = ComboBox_ConfigNames.SelectedIndex;
        if (index == -1)
            return;

        var rule = RuleConfig.RuleInfos[index];

        GeneralView.ActionUpdateTeam1GeneralData(new()
        {
            MaxKill = rule.Team1Rule.MaxKill,
            FlagKD = rule.Team1Rule.FlagKD,
            MaxKD = rule.Team1Rule.MaxKD,
            FlagKPM = rule.Team1Rule.FlagKPM,
            MaxKPM = rule.Team1Rule.MaxKPM,
            MinRank = rule.Team1Rule.MinRank,
            MaxRank = rule.Team1Rule.MaxRank
        });
        GeneralView.ActionUpdateTeam2GeneralData(new()
        {
            MaxKill = rule.Team2Rule.MaxKill,
            FlagKD = rule.Team2Rule.FlagKD,
            MaxKD = rule.Team2Rule.MaxKD,
            FlagKPM = rule.Team2Rule.FlagKPM,
            MaxKPM = rule.Team2Rule.MaxKPM,
            MinRank = rule.Team2Rule.MinRank,
            MaxRank = rule.Team2Rule.MaxRank
        });

        LifeView.ActionUpdateTeam1LifeData(new()
        {
            LifeMaxKD = rule.Team1Rule.LifeMaxKD,
            LifeMaxKPM = rule.Team1Rule.LifeMaxKPM,
            LifeMaxWeaponStar = rule.Team1Rule.LifeMaxWeaponStar,
            LifeMaxVehicleStar = rule.Team1Rule.LifeMaxVehicleStar
        });
        LifeView.ActionUpdateTeam2LifeData(new()
        {
            LifeMaxKD = rule.Team2Rule.LifeMaxKD,
            LifeMaxKPM = rule.Team2Rule.LifeMaxKPM,
            LifeMaxWeaponStar = rule.Team2Rule.LifeMaxWeaponStar,
            LifeMaxVehicleStar = rule.Team2Rule.LifeMaxVehicleStar
        });

        SaveConfig();
    }

    /// <summary>
    /// 保存配置文件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_SaveConfig_Click(object sender, RoutedEventArgs e)
    {
        SaveConfig();
        NotifierHelper.Show(NotifierType.Success, "保存配置文件成功");
    }

    /// <summary>
    /// 当前配置文件重命名
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_ReNameCurrentConfig_Click(object sender, RoutedEventArgs e)
    {
        var name = TextBox_CurrentConfigName.Text.Trim();
        if (string.IsNullOrEmpty(name))
        {
            NotifierHelper.Show(NotifierType.Warning, "配置文件名称不能为空");
            return;
        }

        var index = ComboBox_ConfigNames.SelectedIndex;
        if (index == -1)
        {
            NotifierHelper.Show(NotifierType.Warning, "请选择正确的配置文件");
            return;
        }

        ConfigNames[index] = name;
        RuleConfig.RuleInfos[index].RuleName = name;

        ComboBox_ConfigNames.SelectedIndex = index;
        NotifierHelper.Show(NotifierType.Success, "当前配置文件重命名成功");
    }
}

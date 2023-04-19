using BF1ServerTools.Data;
using BF1ServerTools.Models;
using BF1ServerTools.Helpers;
using BF1ServerTools.Services;

namespace BF1ServerTools.Windows;

/// <summary>
/// QueryRecordWindow.xaml 的交互逻辑
/// </summary>
public partial class QueryRecordWindow
{
    /// <summary>
    /// 数据模型绑定
    /// </summary>
    public QueryModel QueryModel { get; set; } = new();

    /// <summary>
    /// 玩家综合数据
    /// </summary>
    public ObservableCollection<string> ListBox_PlayerDatas { get; set; } = new();
    /// <summary>
    /// 玩家武器数据
    /// </summary>
    public ObservableCollection<WeaponStat> ListBox_WeaponStats { get; set; } = new();
    /// <summary>
    /// 玩家载具数据
    /// </summary>
    public ObservableCollection<VehicleStat> ListBox_VehicleStats { get; set; } = new();

    /////////////////////////////////////////////////////

    public string PlayerName { get; }
    public long PersonaId { get; }
    public int Rank { get; }

    public QueryRecordWindow(string playerName, long personaId, int rank)
    {
        InitializeComponent();
        this.DataContext = this;

        PlayerName = playerName;
        PersonaId = personaId;
        Rank = rank;
    }

    private void Window_QueryRecord_Loaded(object sender, RoutedEventArgs e)
    {
        Title = $"{this.Title} > 玩家ID : {PlayerName} > 数字ID : {PersonaId}";

        QueryPlayerRecord();
    }

    private void Window_QueryRecord_Closing(object sender, CancelEventArgs e)
    {

    }

    /// <summary>
    /// 分段查询玩家数据
    /// </summary>
    private async void QueryPlayerRecord()
    {
        var result = await Task.Run(() =>
        {
            return GameUtil.FindPlayerLifeCache(PersonaId);
        });

        if (result != null)
        {
            DetailedStats(result.BaseStats);

            GetWeapons(result.WeaponStats);
            GetVehicles(result.VehicleStats);
        }
        else
        {
            NotifierHelper.Show(NotifierType.Error, $"生涯缓存列表未找到玩家 {PlayerName} 数据，操作取消");
        }
    }

    /// <summary>
    /// 获取玩家详情数据
    /// </summary>
    /// <param name="baseStats"></param>
    private void DetailedStats(BaseStat baseStats)
    {
        Task.Run(() =>
        {
            AddPlayerInfo($"KD : {baseStats.kd:0.00}");
            AddPlayerInfo($"KPM : {baseStats.kpm:0.00}");
            AddPlayerInfo($"SPM : {baseStats.spm:0.00}");

            AddPlayerInfo($"命中率 : {baseStats.accuracyRatio * 100:0.00}%");
            AddPlayerInfo($"爆头率 : {baseStats.headshotsVKills:0.00}%");
            AddPlayerInfo($"爆头数 : {baseStats.headShots}");

            AddPlayerInfo($"最高连续击杀数 : {baseStats.highestKillStreak}");
            AddPlayerInfo($"最远爆头距离 : {baseStats.longestHeadShot}");
            AddPlayerInfo($"最佳兵种 : {baseStats.favoriteClass}");

            AddPlayerInfo();

            AddPlayerInfo($"击杀 : {baseStats.kills}");
            AddPlayerInfo($"死亡 : {baseStats.deaths}");
            AddPlayerInfo($"协助击杀数 : {baseStats.killAssists}");

            AddPlayerInfo($"仇敌击杀数 : {baseStats.avengerKills}");
            AddPlayerInfo($"救星击杀数 : {baseStats.saviorKills}");
            AddPlayerInfo($"急救数 : {baseStats.revives}");
            AddPlayerInfo($"治疗分 : {baseStats.heals}");
            AddPlayerInfo($"修理分 : {baseStats.repairs}");

            AddPlayerInfo();

            AddPlayerInfo($"胜利场数 : {baseStats.wins}");
            AddPlayerInfo($"战败场数 : {baseStats.losses}");
            AddPlayerInfo($"胜率 : {baseStats.winPercent:0.00}%");
            AddPlayerInfo($"技巧值 : {baseStats.skill}");
            AddPlayerInfo($"游戏总场数 : {baseStats.roundsPlayed}");
            AddPlayerInfo($"取得狗牌数 : {baseStats.dogtagsTaken}");

            AddPlayerInfo($"小隊分数 : {baseStats.squadScore}");
            AddPlayerInfo($"奖励分数 : {baseStats.awardScore}");
            AddPlayerInfo($"加成分数 : {baseStats.bonusScore}");
        });
    }

    /// <summary>
    /// 获取玩家武器数据
    /// </summary>
    /// <param name="weaponStats"></param>
    private void GetWeapons(List<WeaponStat> weaponStats)
    {
        Task.Run(() =>
        {
            foreach (var item in weaponStats)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Background, () =>
                {
                    ListBox_WeaponStats.Add(item);
                });
            }
        });
    }

    /// <summary>
    /// 获取玩家载具数据
    /// </summary>
    /// <param name="vehicleStats"></param>
    private void GetVehicles(List<VehicleStat> vehicleStats)
    {
        Task.Run(() =>
        {
            foreach (var item in vehicleStats)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Background, () =>
                {
                    ListBox_VehicleStats.Add(item);
                });
            }
        });
    }

    private void AddPlayerInfo(string str = "")
    {
        this.Dispatcher.Invoke(() =>
        {
            ListBox_PlayerDatas.Add(str);
        });
    }
}

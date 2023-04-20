using BF1ServerTools.Data;
using BF1ServerTools.Utils;
using BF1ServerTools.Services;

namespace BF1ServerTools.Views.Monit;

/// <summary>
/// CacheView.xaml 的交互逻辑
/// </summary>
public partial class CacheView : UserControl
{
    /// <summary>
    /// 绑定UI动态数据集合
    /// </summary>
    public ObservableCollection<QueryCache> DataGrid_QueryCaches { get; set; } = new();

    public CacheView()
    {
        InitializeComponent();
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataGrid_Cache.SelectedItem is QueryCache item)
            MenuItem_SelectedInfo.Header = $"[缓存]  {item.Name}";
        else
            MenuItem_SelectedInfo.Header = "[缓存]  当前未选中";
    }

    private void MenuItem_Cache_RefushCacheList_Click(object sender, RoutedEventArgs e)
    {
        RefreshCacheLifeData();
    }

    private void MenuItem_Cache_CopyPlayerName_Click(object sender, RoutedEventArgs e)
    {
        if (DataGrid_Cache.SelectedItem is QueryCache item)
            ViewUtil.Copy2Clipboard(item.Name);
        else
            ViewUtil.UnSelectedNotifier("缓存");
    }

    private void MenuItem_Cache_CopyPlayerPersonaId_Click(object sender, RoutedEventArgs e)
    {
        if (DataGrid_Cache.SelectedItem is QueryCache item)
            ViewUtil.Copy2Clipboard(item.PersonaId.ToString());
        else
            ViewUtil.UnSelectedNotifier("缓存");
    }

    private void MenuItem_Cache_QueryPlayerRecord_Click(object sender, RoutedEventArgs e)
    {
        if (DataGrid_Cache.SelectedItem is QueryCache item)
            ViewUtil.QueryPlayerRecord(item.Name, item.PersonaId, 0);
        else
            ViewUtil.UnSelectedNotifier("缓存");
    }

    /// <summary>
    /// 刷新已缓存玩家生涯数据
    /// </summary>
    private void RefreshCacheLifeData()
    {
        DataGrid_QueryCaches.Clear();

        for (int i = 0; i < Globals.PlayerLifeCaches.Count; i++)
        {
            var item = Globals.PlayerLifeCaches[i];

            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, () =>
            {
                DataGrid_QueryCaches.Add(new()
                {
                    Index = DataGrid_QueryCaches.Count + 1,
                    Name = item.Name,
                    PersonaId = item.PersonaId,
                    KD = item.KD,
                    KPM = item.KPM,
                    Time = item.Time,
                    WeaponCount = item.WeaponStats.Count,
                    VehicleCount = item.VehicleStats.Count,
                    CountDown = $"{MiscUtil.DiffMinutes(item.CreateTime, DateTime.Now):0.00} 分钟"
                });
            });
        }
    }
}

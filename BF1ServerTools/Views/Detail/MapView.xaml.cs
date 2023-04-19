using BF1ServerTools.API;
using BF1ServerTools.Data;
using BF1ServerTools.Helpers;
using BF1ServerTools.Windows;
using BF1ServerTools.Services;

namespace BF1ServerTools.Views.Detail;

/// <summary>
/// MapView.xaml 的交互逻辑
/// </summary>
public partial class MapView : UserControl
{
    public MapView()
    {
        InitializeComponent();

        ServerService.UpdateServerMapListEvent += ServerService_UpdateServerMapListEvent;
    }

    private void ServerService_UpdateServerMapListEvent(List<MapInfo> mapList)
    {
        this.Dispatcher.Invoke(ListBox_Map.Items.Clear);

        foreach (var map in mapList)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, () =>
            {
                ListBox_Map.Items.Add(map);
            });
        }
    }

    private async void ListBox_Map_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ListBox_Map.SelectedIndex == -1)
            return;

        if (!AuthUtil.CheckPlayerAuth())
        {
            // 使ListBox能够响应重复点击
            ListBox_Map.SelectedIndex = -1;
            return;
        }

        if (string.IsNullOrEmpty(Globals.PersistedGameId))
        {
            NotifierHelper.Show(NotifierType.Warning, "PersistedGameId为空，请重新获取服务器详细信息");
            // 使ListBox能够响应重复点击
            ListBox_Map.SelectedIndex = -1;
            return;
        }

        if (ListBox_Map.SelectedItem is MapInfo item)
        {
            var mapInfo = item.MapMode + " - " + item.MapName;
            var changeMapWindow = new ChangeMapWindow(mapInfo, item.MapImage)
            {
                Owner = MainWindow.MainWindowInstance
            };
            if (changeMapWindow.ShowDialog() == true)
            {
                NotifierHelper.Show(NotifierType.Information, $"正在更换服务器 {Globals.GameId} 地图为 {item.MapName} 中...");

                var result = await BF1API.RSPChooseLevel(Globals.SessionId, Globals.PersistedGameId, item.Index);
                if (result.IsSuccess)
                    NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  更换服务器 {Globals.GameId} 地图为 {item.MapName} 成功");
                else
                    NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  更换服务器 {Globals.GameId} 地图为 {item.MapName} 失败\n{result.Content}");
            }
        }

        // 使ListBox能够响应重复点击
        ListBox_Map.SelectedIndex = -1;
    }
}

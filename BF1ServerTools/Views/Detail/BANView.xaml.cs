using BF1ServerTools.API;
using BF1ServerTools.Data;
using BF1ServerTools.Helpers;
using BF1ServerTools.Windows;
using BF1ServerTools.Services;

namespace BF1ServerTools.Views.Detail;

/// <summary>
/// BANView.xaml 的交互逻辑
/// </summary>
public partial class BANView : UserControl
{
    public BANView()
    {
        InitializeComponent();

        ServerService.UpdateServerBANListEvent += ServerService_UpdateServerBANListEvent;
    }

    private void ServerService_UpdateServerBANListEvent(List<RSPInfo> banList)
    {
        this.Dispatcher.Invoke(ListBox_BAN.Items.Clear);

        foreach (var ban in banList)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, () =>
            {
                ListBox_BAN.Items.Add(ban);
            });
        }
    }

    private void ListBox_BAN_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ListBox_BAN.SelectedItem is RSPInfo item)
            MenuItem_SelectedInfo.Header = $"[BAN]  {item.DisplayName}";
        else
            MenuItem_SelectedInfo.Header = "[BAN]  当前未选中";
    }

    private void MenuItem_BAN_AddNewPlayer_Click(object sender, RoutedEventArgs e)
    {
        if (!PlayerUtil.CheckPlayerAuth2())
            return;

        var addPlayerWindow = new AddPlayerWindow("BAN")
        {
            Owner = MainWindow.MainWindowInstance
        };
        addPlayerWindow.ShowDialog();
    }

    private async void MenuItem_BAN_RemoveSelectedPlayer_Click(object sender, RoutedEventArgs e)
    {
        if (!PlayerUtil.CheckPlayerAuth2())
            return;

        if (ListBox_BAN.SelectedItem is RSPInfo item)
        {
            NotifierHelper.Show(NotifierType.Information, $"正在移除服务器BAN {item.DisplayName} 中...");

            var result = await BF1API.RemoveServerBan(Globals.SessionId, Globals.ServerId, item.PersonaId);
            if (result.IsSuccess)
                NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  移除服务器BAN {item.DisplayName} 成功");
            else
                NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  移除服务器BAN {item.DisplayName} 失败\n{result.Content}");
        }
    }

    private void MenuItem_BAN_CopyPlayerName_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_BAN.SelectedItem is RSPInfo item)
            ViewUtil.Copy2Clipboard(item.DisplayName);
        else
            ViewUtil.UnSelectedNotifier("BAN");
    }

    private void MenuItem_BAN_CopyPlayerPersonaId_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_BAN.SelectedItem is RSPInfo item)
            ViewUtil.Copy2Clipboard(item.PersonaId.ToString());
        else
            ViewUtil.UnSelectedNotifier("BAN");
    }

    private void MenuItem_BAN_QueryPlayerRecord_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_BAN.SelectedItem is RSPInfo item)
            ViewUtil.QueryPlayerRecord(item.DisplayName, item.PersonaId, -9);
        else
            ViewUtil.UnSelectedNotifier("BAN");
    }
}

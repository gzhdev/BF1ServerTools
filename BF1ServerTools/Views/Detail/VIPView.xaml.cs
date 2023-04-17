using BF1ServerTools.API;
using BF1ServerTools.Data;
using BF1ServerTools.Helpers;
using BF1ServerTools.Windows;
using BF1ServerTools.Services;

namespace BF1ServerTools.Views.Detail;

/// <summary>
/// VIPView.xaml 的交互逻辑
/// </summary>
public partial class VIPView : UserControl
{
    public VIPView()
    {
        InitializeComponent();

        ServerService.UpdateServerVIPListEvent += ServerService_UpdateServerVIPListEvent;
    }

    private void ServerService_UpdateServerVIPListEvent(List<RSPInfo> vipList)
    {
        this.Dispatcher.Invoke(ListBox_VIP.Items.Clear);

        foreach (var vip in vipList)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, () =>
            {
                ListBox_VIP.Items.Add(vip);
            });
        }
    }

    private void ListBox_VIP_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ListBox_VIP.SelectedItem is RSPInfo item)
            MenuItem_SelectedInfo.Header = $"[VIP]  {item.DisplayName}";
        else
            MenuItem_SelectedInfo.Header = "[VIP]  当前未选中";
    }

    private void MenuItem_VIP_AddNewPlayer_Click(object sender, RoutedEventArgs e)
    {
        if (!PlayerUtil.CheckPlayerAuth2())
            return;

        var addPlayerWindow = new AddPlayerWindow("VIP")
        {
            Owner = MainWindow.MainWindowInstance
        };
        addPlayerWindow.ShowDialog();
    }

    private async void MenuItem_VIP_RemoveSelectedPlayer_Click(object sender, RoutedEventArgs e)
    {
        if (!PlayerUtil.CheckPlayerAuth2())
            return;

        if (ListBox_VIP.SelectedItem is RSPInfo item)
        {
            NotifierHelper.Show(NotifierType.Information, $"正在移除服务器VIP {item.DisplayName} 中...");

            var result = await BF1API.RemoveServerVip(Globals.SessionId, Globals.ServerId, item.PersonaId);
            if (result.IsSuccess)
                NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  移除服务器VIP {item.DisplayName} 成功");
            else
                NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  移除服务器VIP {item.DisplayName} 失败\n{result.Content}");
        }
    }

    private void MenuItem_VIP_CopyPlayerName_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_VIP.SelectedItem is RSPInfo item)
            ViewUtil.Copy2Clipboard(item.DisplayName);
        else
            ViewUtil.UnSelectedNotifier("VIP");
    }

    private void MenuItem_VIP_CopyPlayerPersonaId_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_VIP.SelectedItem is RSPInfo item)
            ViewUtil.Copy2Clipboard(item.PersonaId.ToString());
        else
            ViewUtil.UnSelectedNotifier("VIP");
    }

    private void MenuItem_VIP_QueryPlayerRecord_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_VIP.SelectedItem is RSPInfo item)
            ViewUtil.QueryPlayerRecord(item.DisplayName, item.PersonaId, -9);
        else
            ViewUtil.UnSelectedNotifier("VIP");
    }
}

using BF1ServerTools.API;
using BF1ServerTools.Data;
using BF1ServerTools.Helpers;
using BF1ServerTools.Windows;
using BF1ServerTools.Services;

namespace BF1ServerTools.Views.Detail;

/// <summary>
/// AdminView.xaml 的交互逻辑
/// </summary>
public partial class AdminView : UserControl
{
    public AdminView()
    {
        InitializeComponent();

        ServerService.UpdateServerAdminListEvent += ServerService_UpdateServerAdminListEvent;
    }

    private void ServerService_UpdateServerAdminListEvent(List<RSPInfo> adminList)
    {
        this.Dispatcher.Invoke(ListBox_Admin.Items.Clear);

        foreach (var admin in adminList)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, () =>
            {
                ListBox_Admin.Items.Add(admin);
            });
        }
    }

    private void ListBox_Admin_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ListBox_Admin.SelectedItem is RSPInfo item)
            MenuItem_SelectedInfo.Header = $"[Admin]  {item.DisplayName}";
        else
            MenuItem_SelectedInfo.Header = "[Admin]  当前未选中";
    }

    private void MenuItem_Admin_AddNewPlayer_Click(object sender, RoutedEventArgs e)
    {
        if (!PlayerUtil.CheckPlayerAuth2())
            return;

        var addPlayerWindow = new AddPlayerWindow("Admin")
        {
            Owner = MainWindow.MainWindowInstance
        };
        addPlayerWindow.ShowDialog();
    }

    private async void MenuItem_Admin_RemoveSelectedPlayer_Click(object sender, RoutedEventArgs e)
    {
        if (!PlayerUtil.CheckPlayerAuth2())
            return;

        if (ListBox_Admin.SelectedItem is RSPInfo item)
        {
            NotifierHelper.Show(NotifierType.Information, $"正在移除服务器Admin {item.DisplayName} 中...");

            var result = await BF1API.RemoveServerAdmin(Globals.SessionId, Globals.ServerId, item.PersonaId);
            if (result.IsSuccess)
                NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  移除服务器Admin {item.DisplayName} 成功");
            else
                NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  移除服务器Admin {item.DisplayName} 失败\n{result.Content}");
        }
    }

    private void MenuItem_Admin_CopyPlayerName_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_Admin.SelectedItem is RSPInfo item)
            ViewUtil.Copy2Clipboard(item.DisplayName);
        else
            ViewUtil.UnSelectedNotifier("Admin");
    }

    private void MenuItem_Admin_CopyPlayerPersonaId_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_Admin.SelectedItem is RSPInfo item)
            ViewUtil.Copy2Clipboard(item.PersonaId.ToString());
        else
            ViewUtil.UnSelectedNotifier("Admin");
    }

    private void MenuItem_Admin_QueryPlayerRecord_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_Admin.SelectedItem is RSPInfo item)
            ViewUtil.QueryPlayerRecord(item.DisplayName, item.PersonaId, -9);
        else
            ViewUtil.UnSelectedNotifier("Admin");
    }
}

using BF1ServerTools.API;
using BF1ServerTools.Data;
using BF1ServerTools.Helpers;
using BF1ServerTools.Models;
using BF1ServerTools.Services;
using System.Xml.Linq;

namespace BF1ServerTools.Views;

/// <summary>
/// DetailView.xaml 的交互逻辑
/// </summary>
public partial class DetailView : UserControl
{
    /// <summary>
    /// 数据模型绑定
    /// </summary>
    public DetailModel DetailModel { get; set; } = new();

    public DetailView()
    {
        InitializeComponent();
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;

        ServerService.UpdateServerDetailDataEvent += ServerService_UpdateServerDetailDataEvent;
    }

    private void MainWindow_WindowClosingEvent()
    {

    }

    private void ServerService_UpdateServerDetailDataEvent(DetailData detailData)
    {
        DetailModel.Name = detailData.Name;
        DetailModel.Description = detailData.Description;
        DetailModel.GameId = detailData.GameId;
        DetailModel.Guid = detailData.Guid;
        DetailModel.ServerId = detailData.ServerId;
        DetailModel.Bookmark = detailData.Bookmark;
        DetailModel.OwnerName = detailData.OwnerName;
        DetailModel.OwnerPersonaId = detailData.OwnerPersonaId;
        DetailModel.OwnerImage = detailData.OwnerImage;
    }

    /// <summary>
    /// 刷新当前服务器详情
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_RefreshFullServerDetails_Click(object sender, RoutedEventArgs e)
    {
        if (!AuthUtil.CheckPlayerSesId())
            return;

        NotifierHelper.Show(NotifierType.Information, "正在刷新当前服务器详情中...");

        ServerService.ReUpdateServerDetils();

        await Task.Run(async () =>
        {
            int count = 5;
            while (count-- > 0)
            {
                this.Dispatcher.Invoke(() =>
                {
                    Button_RefreshFullServerDetails.IsEnabled = false;
                    Button_RefreshFullServerDetails.Content = $"刷新当前服务器详情 {count}s";
                });

                await Task.Delay(1000);
            }

            this.Dispatcher.Invoke(() =>
            {
                Button_RefreshFullServerDetails.IsEnabled = true;
                Button_RefreshFullServerDetails.Content = "刷新当前服务器详情";
            });
        });

        NotifierHelper.Show(NotifierType.Notification, "刷新当前服务器详情操作结束");
    }

    /// <summary>
    /// 离开服务器
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_LeaveCurrentGame_Click(object sender, RoutedEventArgs e)
    {
        if (!AuthUtil.CheckPlayerSesId1())
            return;

        NotifierHelper.Show(NotifierType.Information, $"正在离开服务器 {Globals.GameId} 中...");

        var result = await BF1API.LeaveGame(Globals.SessionId1, Globals.GameId);
        if (result.IsSuccess)
            NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  离开服务器 {Globals.GameId} 成功");
        else
            NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  离开服务器 {Globals.GameId} 失败\n{result.Content}");
    }
}

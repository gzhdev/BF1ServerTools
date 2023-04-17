using BF1ServerTools.API;
using BF1ServerTools.Helpers;

namespace BF1ServerTools.Windows;

/// <summary>
/// CustomKickWindow.xaml 的交互逻辑
/// </summary>
public partial class CustomKickWindow
{
    public string PlayerName { get; }
    public long PersonaId { get; }

    public CustomKickWindow(string playerName, long personaId)
    {
        InitializeComponent();
        this.DataContext = this;

        PlayerName = playerName;
        PersonaId = personaId;
    }

    private void Window_CustomKick_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void Window_CustomKick_Closing(object sender, CancelEventArgs e)
    {

    }

    private async void Button_KickPlayer_Click(object sender, RoutedEventArgs e)
    {
        this.Hide();

        string reason = string.Empty;
        if (RadioButton_Reson0.IsChecked == true)
        {
            reason = TextBox_CustomReason.Text.Trim();
            if (!string.IsNullOrEmpty(reason))
                reason = ChsHelper.ToTraditional(reason);
        }
        else if (RadioButton_Reson1.IsChecked == true)
        {
            reason = "FairFight: Banned Code #RSuhf1";
        }
        else if (RadioButton_Reson2.IsChecked == true)
        {
            reason = "您已被 FairFight 踢出。";
        }
        else if (RadioButton_Reson3.IsChecked == true)
        {
            reason = "未知錯誤。錯誤代碼：1";
        }
        else if (RadioButton_Reson4.IsChecked == true)
        {
            reason = "您與遊戲連線已中斷。";
        }
        else if (RadioButton_Reson5.IsChecked == true)
        {
            reason = "該遊戲已不存在。";
        }
        else if (RadioButton_Reson6.IsChecked == true)
        {
            reason = "ADMINPRIORITY";
        }

        NotifierHelper.Show(NotifierType.Information, $"正在踢出玩家 {PlayerName} 中...");

        var result = await BF1API.RSPKickPlayer(Globals.SessionId, Globals.GameId, PersonaId, reason);
        if (result.IsSuccess)
            NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  踢出玩家 {PlayerName} 成功");
        else
            NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  踢出玩家 {PlayerName} 失败\n{result.Content}");

        this.Close();
    }
}

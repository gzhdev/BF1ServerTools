using BF1ServerTools.API;
using BF1ServerTools.Helpers;

namespace BF1ServerTools.Windows;

/// <summary>
/// AddPlayerWindow.xaml 的交互逻辑
/// </summary>
public partial class AddPlayerWindow
{
    public string AddType { get; private set; }

    public AddPlayerWindow(string addType)
    {
        InitializeComponent();
        this.DataContext = this;

        AddType = addType;
    }

    private void Window_AddPlayer_Loaded(object sender, RoutedEventArgs e)
    {
        Title = $"添加新玩家到 {AddType} 列表";
        Button_AddNewPlayer.Content = $"添加新玩家到 {AddType} 列表";
    }

    private void Window_AddPlayer_Closing(object sender, CancelEventArgs e)
    {

    }

    private async void Button_AddNewPlayer_Click(object sender, RoutedEventArgs e)
    {
        this.Hide();

        var playerName = TextBox_NewPlayerName.Text.Trim();

        NotifierHelper.Show(NotifierType.Information, $"正在添加服务器{AddType} {playerName} 中...");

        RespContent result;
        switch (AddType)
        {
            case "Admin":
                result = await BF1API.AddServerAdmin(Globals.SessionId, Globals.ServerId, playerName);
                break;
            case "VIP":
                result = await BF1API.AddServerVip(Globals.SessionId, Globals.ServerId, playerName);
                break;
            case "BAN":
                result = await BF1API.AddServerBan(Globals.SessionId, Globals.ServerId, playerName);
                break;
            default:
                return;
        }

        if (result.IsSuccess)
            NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  添加服务器{AddType} {playerName} 成功");
        else
            NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  添加服务器{AddType} {playerName} 失败\n{result.Content}");

        this.Close();
    }
}

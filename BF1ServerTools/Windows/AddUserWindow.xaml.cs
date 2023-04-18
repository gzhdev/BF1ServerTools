namespace BF1ServerTools.Windows;

/// <summary>
/// AddUserWindow.xaml 的交互逻辑
/// </summary>
public partial class AddUserWindow
{
    public string MainTitle { get; private set; }

    public Action<string> ActionGetPlayerName;

    public AddUserWindow(string mainTitle)
    {
        InitializeComponent();
        this.DataContext = this;

        MainTitle = mainTitle;
    }

    private void Window_AddUser_Loaded(object sender, RoutedEventArgs e)
    {
        Title = $"添加新玩家到 {MainTitle} 列表";
        Button_AddNewPlayer.Content = $"添加新玩家到 {MainTitle} 列表";
    }

    private void Window_AddUser_Closing(object sender, CancelEventArgs e)
    {

    }

    private void Button_AddNewPlayer_Click(object sender, RoutedEventArgs e)
    {
        this.Hide();

        var playerName = TextBox_NewPlayerName.Text.Trim();

        ActionGetPlayerName?.Invoke(playerName);

        this.Close();
    }
}

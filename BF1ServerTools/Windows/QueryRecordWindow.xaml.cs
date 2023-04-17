namespace BF1ServerTools.Windows;

/// <summary>
/// QueryRecordWindow.xaml 的交互逻辑
/// </summary>
public partial class QueryRecordWindow
{
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
    }

    private void Window_QueryRecord_Closing(object sender, CancelEventArgs e)
    {

    }
}

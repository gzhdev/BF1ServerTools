namespace BF1ServerTools.Views;

/// <summary>
/// RuleView.xaml 的交互逻辑
/// </summary>
public partial class RuleView : UserControl
{
    public RuleView()
    {
        InitializeComponent();
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;
    }

    /// <summary>
    /// 主窗口关闭事件
    /// </summary>
    private void MainWindow_WindowClosingEvent()
    {

    }
}

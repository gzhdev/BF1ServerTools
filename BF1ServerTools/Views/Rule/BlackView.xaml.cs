using BF1ServerTools.Data;
using BF1ServerTools.Windows;
using BF1ServerTools.Helpers;
using BF1ServerTools.Services;

namespace BF1ServerTools.Views.Rule;

/// <summary>
/// BlackView.xaml 的交互逻辑
/// </summary>
public partial class BlackView : UserControl
{
    /// <summary>
    /// 绑定UI 黑名单数据
    /// </summary>
    public ObservableCollection<WhiteInfo> ListBox_BlackInfos { get; set; } = new();

    ////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 获取黑名单数据委托
    /// </summary>
    public static Func<List<string>> FuncGetBlackData;

    /// <summary>
    /// 设置黑名单数据委托
    /// </summary>
    public static Action<List<string>> ActionSetBlackData;

    ////////////////////////////////////////////////////////////////////

    public BlackView()
    {
        InitializeComponent();

        FuncGetBlackData = GetBlackData;
        ActionSetBlackData = SetBlackData;

        RuleView.ApplyCurrentRuleEvent += RuleView_ApplyCurrentRuleEvent;
    }

    private void RuleView_ApplyCurrentRuleEvent()
    {
        // 清空黑名单列表
        Globals.CustomBlacks_Name.Clear();

        // 添加自定义黑名单列表
        foreach (var item in ListBox_BlackInfos)
        {
            Globals.CustomBlacks_Name.Add(item.Name);
        }
    }

    /// <summary>
    /// 获取黑名单数据
    /// </summary>
    /// <returns></returns>
    private List<string> GetBlackData()
    {
        var list = new List<string>();

        foreach (var item in ListBox_BlackInfos)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
                continue;

            list.Add(item.Name);
        }

        return list;
    }

    /// <summary>
    /// 设置黑名单数据
    /// </summary>
    /// <param name="BlackList"></param>
    private void SetBlackData(List<string> BlackList)
    {
        ListBox_BlackInfos.Clear();

        foreach (var name in BlackList)
        {
            if (string.IsNullOrWhiteSpace(name))
                continue;

            ListBox_BlackInfos.Add(new()
            {
                Avatar = Globals.Default_Avatar,
                Name = name
            });
        }
    }

    //////////////////////////////////////////////////////

    private void ListBox_Black_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ListBox_Black.SelectedItem is WhiteInfo item)
            MenuItem_SelectedInfo.Header = $"[黑名单]  {item.Name}";
        else
            MenuItem_SelectedInfo.Header = "[黑名单]  当前未选中";
    }

    private void MenuItem_Black_AddNewPlayer_Click(object sender, RoutedEventArgs e)
    {
        var addUserWindow = new AddUserWindow("黑名单")
        {
            Owner = MainWindow.MainWindowInstance,
            ActionGetPlayerName = (name) =>
            {
                ListBox_BlackInfos.Add(new()
                {
                    Avatar = Globals.Default_Avatar,
                    Name = name
                });
            }
        };
        addUserWindow.ShowDialog();
    }

    private void MenuItem_Black_RemoveSelectedPlayer_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_Black.SelectedItem is WhiteInfo item)
        {
            ListBox_BlackInfos.Remove(item);
            NotifierHelper.Show(NotifierType.Success, $"从黑名单列表移除玩家 {item.Name} 成功");
        }
        else
        {
            ViewUtil.UnSelectedNotifier("黑名单");
        }
    }

    private void MenuItem_Black_CopyPlayerName_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_Black.SelectedItem is WhiteInfo item)
            ViewUtil.Copy2Clipboard(item.Name);
        else
            ViewUtil.UnSelectedNotifier("黑名单");
    }

    private void MenuItem_Black_ImportList_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var fileDialog = new OpenFileDialog
            {
                Title = "批量导入黑名单列表",
                RestoreDirectory = true,
                Multiselect = false,
                Filter = "文本文档|*.txt"
            };

            if (fileDialog.ShowDialog() == true)
            {
                ListBox_BlackInfos.Clear();
                foreach (var name in File.ReadAllLines(fileDialog.FileName))
                {
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        ListBox_BlackInfos.Add(new()
                        {
                            Avatar = Globals.Default_Avatar,
                            Name = name
                        });
                    }
                }

                NotifierHelper.Show(NotifierType.Success, "批量导入txt文件到黑名单列表成功");
            }
        }
        catch (Exception ex)
        {
            NotifierHelper.ShowException(ex);
        }
    }

    private void MenuItem_Black_ExportList_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_BlackInfos.Count == 0)
        {
            NotifierHelper.Show(NotifierType.Warning, "黑名单列表为空，导出操作取消");
            return;
        }

        try
        {
            var fileDialog = new SaveFileDialog
            {
                Title = "批量导出黑名单列表",
                RestoreDirectory = true,
                Filter = "文本文档|*.txt",
                FileName = "批量导出黑名单列表.txt"
            };

            if (fileDialog.ShowDialog() == true)
            {
                var nameList = new List<string>();
                ListBox_BlackInfos.ToList().ForEach(x =>
                {
                    nameList.Add(x.Name);
                });

                File.WriteAllText(fileDialog.FileName, string.Join(Environment.NewLine, nameList));

                NotifierHelper.Show(NotifierType.Success, "批量导出黑名单列表到txt文件成功");
            }
        }
        catch (Exception ex)
        {
            NotifierHelper.ShowException(ex);
        }
    }

    private void MenuItem_Black_TrimList_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_BlackInfos.Count == 0)
        {
            NotifierHelper.Show(NotifierType.Warning, "黑名单列表为空，整理操作取消");
            return;
        }

        var tempStr = new List<string>();

        // 提取玩家名称列表
        foreach (var item in ListBox_BlackInfos.ToList())
            tempStr.Add(item.Name);

        // 清空原列表
        ListBox_BlackInfos.Clear();

        // 填充原列表
        foreach (var name in tempStr.Distinct().ToList().Order())
        {
            ListBox_BlackInfos.Add(new()
            {
                Avatar = Globals.Default_Avatar,
                Name = name
            });
        }

        NotifierHelper.Show(NotifierType.Success, "黑名单列表整理操作成功");
    }

    private void MenuItem_Black_ClearList_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("你确认要清空黑名单列表吗？此操作不可恢复", "清空黑名单列表",
            MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
        {
            ListBox_BlackInfos.Clear();
            NotifierHelper.Show(NotifierType.Success, "清空黑名单列表成功");
        }
    }
}

using BF1ServerTools.Data;
using BF1ServerTools.Windows;
using BF1ServerTools.Helpers;
using BF1ServerTools.Services;

namespace BF1ServerTools.Views.Rule;

/// <summary>
/// WhiteView.xaml 的交互逻辑
/// </summary>
public partial class WhiteView : UserControl
{
    /// <summary>
    /// 绑定UI 白名单数据
    /// </summary>
    public ObservableCollection<WhiteInfo> ListBox_WhiteInfos { get; set; } = new();

    /// <summary>
    /// 默认头像
    /// </summary>
    private const string Default_Avatar = "\\Assets\\Images\\Other\\Avatar.jpg";

    ////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 获取白名单数据委托
    /// </summary>
    public static Func<List<string>> FuncGetWhiteData;

    /// <summary>
    /// 设置白名单数据委托
    /// </summary>
    public static Action<List<string>> ActionSetWhiteData;

    ////////////////////////////////////////////////////////////////////

    public WhiteView()
    {
        InitializeComponent();

        FuncGetWhiteData = GetWhiteData;
        ActionSetWhiteData = SetWhiteData;

        RuleView.ApplyCurrentRuleEvent += RuleView_ApplyCurrentRuleEvent;
    }

    private void RuleView_ApplyCurrentRuleEvent()
    {

    }

    private List<string> GetWhiteData()
    {
        var list = new List<string>();

        foreach (var item in ListBox_WhiteInfos)
        {
            if (string.IsNullOrWhiteSpace(item.Name))
                continue;

            list.Add(item.Name);
        }

        return list;
    }

    private void SetWhiteData(List<string> whiteList)
    {
        ListBox_WhiteInfos.Clear();

        foreach (var name in whiteList)
        {
            if (string.IsNullOrWhiteSpace(name))
                continue;

            ListBox_WhiteInfos.Add(new()
            {
                Avatar = Default_Avatar,
                Name = name
            });
        }
    }

    private void ListBox_White_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ListBox_White.SelectedItem is WhiteInfo item)
            MenuItem_SelectedInfo.Header = $"[白名单]  {item.Name}";
        else
            MenuItem_SelectedInfo.Header = "[白名单]  当前未选中";
    }

    private void MenuItem_White_AddNewPlayer_Click(object sender, RoutedEventArgs e)
    {
        var addUserWindow = new AddUserWindow("白名单")
        {
            Owner = MainWindow.MainWindowInstance,
            ActionGetPlayerName = (name) =>
            {
                ListBox_WhiteInfos.Add(new()
                {
                    Avatar = Default_Avatar,
                    Name = name
                });
            }
        };
        addUserWindow.ShowDialog();
    }

    private void MenuItem_White_RemoveSelectedPlayer_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_White.SelectedItem is WhiteInfo item)
        {
            ListBox_WhiteInfos.Remove(item);
            NotifierHelper.Show(NotifierType.Success, $"从白名单列表移除玩家 {item.Name} 成功");
        }
        else
        {
            ViewUtil.UnSelectedNotifier("白名单");
        }
    }

    private void MenuItem_White_CopyPlayerName_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_White.SelectedItem is WhiteInfo item)
            ViewUtil.Copy2Clipboard(item.Name);
        else
            ViewUtil.UnSelectedNotifier("白名单");
    }

    private void MenuItem_White_ImportList_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var fileDialog = new OpenFileDialog
            {
                Title = "批量导入白名单列表",
                RestoreDirectory = true,
                Multiselect = false,
                Filter = "文本文档|*.txt"
            };

            if (fileDialog.ShowDialog() == true)
            {
                ListBox_WhiteInfos.Clear();
                foreach (var name in File.ReadAllLines(fileDialog.FileName))
                {
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        ListBox_WhiteInfos.Add(new()
                        {
                            Avatar = Default_Avatar,
                            Name = name
                        });
                    }
                }

                NotifierHelper.Show(NotifierType.Success, "批量导入txt文件到白名单列表成功");
            }
        }
        catch (Exception ex)
        {
            NotifierHelper.ShowException(ex);
        }
    }

    private void MenuItem_White_ExportList_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_WhiteInfos.Count == 0)
        {
            NotifierHelper.Show(NotifierType.Warning, "白名单列表为空，导出操作取消");
            return;
        }

        try
        {
            var fileDialog = new SaveFileDialog
            {
                Title = "批量导出白名单列表",
                RestoreDirectory = true,
                Filter = "文本文档|*.txt",
                FileName = "批量导出白名单列表.txt"
            };

            if (fileDialog.ShowDialog() == true)
            {
                var nameList = new List<string>();
                ListBox_WhiteInfos.ToList().ForEach(x =>
                {
                    nameList.Add(x.Name);
                });

                File.WriteAllText(fileDialog.FileName, string.Join(Environment.NewLine, nameList));

                NotifierHelper.Show(NotifierType.Success, "批量导出白名单列表到txt文件成功");
            }
        }
        catch (Exception ex)
        {
            NotifierHelper.ShowException(ex);
        }
    }

    private void MenuItem_White_TrimList_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_WhiteInfos.Count == 0)
        {
            NotifierHelper.Show(NotifierType.Warning, "白名单列表为空，整理操作取消");
            return;
        }

        var tempStr = new List<string>();
        // 提取玩家名称列表
        foreach (var item in ListBox_WhiteInfos.ToList())
            tempStr.Add(item.Name);
        // 清空原列表
        ListBox_WhiteInfos.Clear();
        // 填充原列表
        foreach (var name in tempStr.Distinct().ToList().Order())
        {
            ListBox_WhiteInfos.Add(new()
            {
                Avatar = Default_Avatar,
                Name = name
            });
        }

        NotifierHelper.Show(NotifierType.Success, "白名单列表整理操作成功");
    }

    private void MenuItem_White_ClearList_Click(object sender, RoutedEventArgs e)
    {
        if (MessageBox.Show("你确认要清空白名单列表吗？此操作不可恢复", "清空白名单列表",
            MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
        {
            ListBox_WhiteInfos.Clear();
            NotifierHelper.Show(NotifierType.Success, "清空白名单列表成功");
        }
    }
}

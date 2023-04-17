using BF1ServerTools.SDK;
using BF1ServerTools.Data;
using BF1ServerTools.Utils;
using BF1ServerTools.Helpers;
using BF1ServerTools.Models;
using BF1ServerTools.Services;

using CommunityToolkit.Mvvm.Input;

namespace BF1ServerTools;

/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow
{
    /// <summary>
    /// 导航字典
    /// </summary>
    private readonly Dictionary<string, UserControl> NavDictionary = new();

    /// <summary>
    /// 主窗口关闭事件
    /// </summary>
    public static event Action WindowClosingEvent;

    /// <summary>
    /// 向外暴露主窗口实例
    /// </summary>
    public static Window MainWindowInstance { get; private set; }

    /////////////////////////////////////////

    /// <summary>
    /// 数据模型绑定
    /// </summary>
    public MainModel MainModel { get; set; } = new();

    /// <summary>
    /// 声明一个变量，用于存储软件开始运行的时间
    /// </summary>
    private DateTime Origin_DateTime;

    /////////////////////////////////////////

    public MainWindow()
    {
        InitializeComponent();

        CreateView();
    }

    private void Window_Main_Loaded(object sender, RoutedEventArgs e)
    {
        MainWindowInstance = this;

        Navigate("HomeView");

        ////////////////////////////////////////////

        // 客户端程序版本号
        MainModel.VersionInfo = CoreUtil.VersionInfo;
        MainModel.AppRunTime = "loading...";

        MainModel.DisplayName1 = "loading...";
        MainModel.PersonaId1 = 0;

        MainModel.DisplayName2 = "loading...";
        MainModel.PersonaId2 = 0;

        // 获取当前时间，存储到对于变量中
        Origin_DateTime = DateTime.Now;

        ////////////////////////////////////////////

        MainService.UpdateMainDataEvent += MainService_UpdateMainDataEvent;
    }

    private void Window_Main_Closing(object sender, CancelEventArgs e)
    {
        WindowClosingEvent?.Invoke();
        LoggerHelper.Info("调用主窗口关闭事件成功");

        ProcessHelper.CloseThirdProcess();
        LoggerHelper.Info("关闭第三方进程成功");

        ServiceApp.Shutdown();
        LoggerHelper.Info("服务模块停止成功");

        Chat.FreeMemory();
        LoggerHelper.Info("释放中文聊天指针内存成功");

        Memory.UnInitialize();
        LoggerHelper.Info("释放内存模块进程句柄成功");

        Application.Current.Shutdown();
        LoggerHelper.Info("程序关闭\n\n");
    }

    /// <summary>
    /// 创建子页面
    /// </summary>
    private void CreateView()
    {
        foreach (var item in MiscUtil.GetControls(StackPanel_NavMenu).Cast<RadioButton>())
        {
            var viewName = item.CommandParameter.ToString();

            if (!NavDictionary.ContainsKey(viewName))
            {
                var type = Type.GetType($"BF1ServerTools.Views.{viewName}");
                if (type == null)
                    continue;

                NavDictionary.Add(viewName, Activator.CreateInstance(type) as UserControl);
            }
        }
    }

    /// <summary>
    /// View页面导航
    /// </summary>
    /// <param name="viewName"></param>
    [RelayCommand]
    private void Navigate(string viewName)
    {
        if (!NavDictionary.ContainsKey(viewName))
            return;

        if (ContentControl_NavRegion.Content != NavDictionary[viewName])
            ContentControl_NavRegion.Content = NavDictionary[viewName];
    }

    private void MainService_UpdateMainDataEvent(MainData mainData)
    {
        // 获取软件运行时间
        MainModel.AppRunTime = MiscUtil.ExecDateDiff(Origin_DateTime, DateTime.Now);

        // 是否使用模式1
        MainModel.IsUseMode1 = Globals.IsUseMode1;

        // 模式1玩家信息
        MainModel.DisplayName1 = Globals.DisplayName1;
        MainModel.PersonaId1 = Globals.PersonaId1;
        // 模式2玩家信息
        MainModel.DisplayName2 = Globals.DisplayName2;
        MainModel.PersonaId2 = Globals.PersonaId2;

        if (!ProcessHelper.IsBf1Run())
        {
            this.Dispatcher.Invoke(this.Close);
            return;
        }
    }
}

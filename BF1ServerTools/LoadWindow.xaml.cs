using BF1ServerTools.API;
using BF1ServerTools.SDK;
using BF1ServerTools.Utils;
using BF1ServerTools.Models;
using BF1ServerTools.Helpers;
using BF1ServerTools.Services;

using CommunityToolkit.Mvvm.Input;

namespace BF1ServerTools;

/// <summary>
/// LoadWindow.xaml 的交互逻辑
/// </summary>
public partial class LoadWindow
{
    /// <summary>
    /// 数据模型绑定
    /// </summary>
    public LoadModel LoadModel { get; set; } = new();

    public LoadWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Load窗口加载完成事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Load_Loaded(object sender, RoutedEventArgs e)
    {
        Task.Run(async () =>
        {
            try
            {
                LoadModel.LoadState = "正在初始化工具中...";

                LoggerHelper.Info("开始初始化程序...");
                LoggerHelper.Info($"当前程序版本号 {CoreUtil.VersionInfo}");
                LoggerHelper.Info($"当前程序最后编译时间 {CoreUtil.BuildDate}");

                // 客户端程序版本号
                LoadModel.VersionInfo = CoreUtil.VersionInfo;
                // 最后编译时间
                LoadModel.BuildDate = CoreUtil.BuildDate;

                // 关闭第三方进程
                ProcessHelper.CloseThirdProcess();

                /////////////////////////////////////////////////////////////////////

                LoadModel.LoadState = "正在初始化配置文件...";
                LoggerHelper.Info("正在初始化配置文件...");

                // 创建配置目录
                FileHelper.CreateDirectory(FileHelper.Dir_Cache);
                FileHelper.CreateDirectory(FileHelper.Dir_Config);
                FileHelper.CreateDirectory(FileHelper.Dir_Data);
                FileHelper.CreateDirectory(FileHelper.Dir_Log);
                FileHelper.CreateDirectory(FileHelper.Dir_Robot);

                FileHelper.CreateDirectory(FileHelper.Dir_Log_NLog);
                FileHelper.CreateDirectory(FileHelper.Dir_Log_Crash);

                LoadModel.LoadState = "正在检测战地1是否运行...";
                // 检测战地1是否运行
                if (!ProcessHelper.IsBf1Run())
                {
                    LoadModel.LoadState = "未发现《战地1》游戏进程！程序即将关闭";
                    LoggerHelper.Error("未发现战地1进程");

                    await Task.Delay(2000);
                    this.Dispatcher.Invoke(() =>
                    {
                        Application.Current.Shutdown();
                    });
                    return;
                }

                LoadModel.LoadState = "正在初始化战地1内存模块...";
                // 初始化战地1内存模块
                if (!Memory.Initialize())
                {
                    LoadModel.LoadState = $"战地1内存模块初始化失败！程序即将关闭";
                    LoggerHelper.Error("战地1内存模块初始化失败");

                    await Task.Delay(2000);
                    this.Dispatcher.Invoke(() =>
                    {
                        Application.Current.Shutdown();
                    });
                    return;
                }

                //LoadModel.LoadState = "正在初始化SQLite数据库...";
                //// 初始化SQLite数据库
                //if (!SQLiteApp.Initialize())
                //{
                //    LoadModel.LoadState = "SQLite数据库初始化失败！程序即将关闭";
                //    LoggerHelper.Error("SQLite数据库初始化失败");

                //    await Task.Delay(2000);
                //    this.Dispatcher.Invoke(() =>
                //    {
                //        Application.Current.Shutdown();
                //    });
                //    return;
                //}

                LoadModel.LoadState = "正在初始化战地1API模块...";
                // 初始化战地1API模块
                var ipAddress = IniHelper.ReadValue("WebProxy", "IPAddress");
                var port = IniHelper.ReadValue("WebProxy", "Port");

                // 根据配置文件决定是否使用代理功能
                if (!string.IsNullOrWhiteSpace(ipAddress) &&
                    !string.IsNullOrWhiteSpace(port) &&
                    IPAddress.TryParse(ipAddress, out IPAddress ipAddressValue) &&
                    int.TryParse(port, out int portValue))
                {
                    Globals.IPAddress = ipAddressValue;
                    Globals.Port = portValue;

                    BF1API.Initialize(ipAddressValue, portValue);
                    EA1API.Initialize(ipAddressValue, portValue);
                    EA2API.Initialize(ipAddressValue, portValue);

                    HttpHelper.Initialize(ipAddressValue, portValue);
                }
                else
                {
                    BF1API.Initialize();
                    EA1API.Initialize();
                    EA2API.Initialize();

                    HttpHelper.Initialize();
                }

                /////////////////////////////////////////////////////////////////////

                LoadModel.LoadState = "正在准备最后工作...";

                // 释放资源文件
                FileHelper.ExtractResFile(FileHelper.Res_Robot_Config, FileHelper.File_Robot_Config);
                FileHelper.ExtractResFile(FileHelper.Res_Robot_GoCqHttp, FileHelper.File_Robot_GoCqHttp, true);

                // 申请聊天内存空间
                Chat.AllocateMemory();
                LoggerHelper.Info($"中文聊天指针分配成功 0x{Chat.AllocateMemAddress:x}");

                // 初始化简繁字库
                ChsHelper.PreHeat();
                LoggerHelper.Info("简繁翻译库初始化成功");

                // 初始化战地1服务模块
                ServiceApp.Initialize();
                LoggerHelper.Info("战地1服务模块初始化成功");

                /////////////////////////////////////////////////////////////////////

                this.Dispatcher.Invoke(() =>
                {
                    var mainWindow = new MainWindow();
                    // 转移主程序控制权
                    Application.Current.MainWindow = mainWindow;
                    // 显示主窗口
                    mainWindow.Show();

                    // 关闭初始化窗口
                    this.Close();
                });
            }
            catch (Exception ex)
            {
                LoadModel.LoadState = $"初始化错误，发生了未知异常！\n\n{ex.Message}";
                LoadModel.IsInitError = true;
                LoggerHelper.Error("初始化错误，发生了未知异常", ex);
            }
        });
    }

    private void Window_Load_Closing(object sender, CancelEventArgs e)
    {

    }

    [RelayCommand]
    private void ButtonClick(string cmdName)
    {
        switch (cmdName)
        {
            case "OpenDefaultDir":
                ProcessHelper.OpenLink(FileHelper.Dir_Default);
                break;
            case "ExitMainAPP":
                Application.Current.Shutdown();
                break;
        }
    }
}

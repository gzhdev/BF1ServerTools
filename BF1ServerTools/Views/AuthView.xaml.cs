using BF1ServerTools.API;
using BF1ServerTools.SDK;
using BF1ServerTools.Utils;
using BF1ServerTools.Helpers;
using BF1ServerTools.Models;
using BF1ServerTools.Configs;
using BF1ServerTools.Windows;
using BF1ServerTools.Services;

using CommunityToolkit.Mvvm.Messaging;

namespace BF1ServerTools.Views;

/// <summary>
/// AuthView.xaml 的交互逻辑
/// </summary>
public partial class AuthView : UserControl
{
    /// <summary>
    /// 数据模型绑定
    /// </summary>
    public AuthModel AuthModel { get; set; } = new();

    /// <summary>
    /// 配置文件名称动态集合
    /// </summary>
    public ObservableCollection<string> ConfigNames { get; set; } = new();

    ////////////////////////////////////////////////////////////

    /// <summary>
    /// 配置文件，以json格式保存到本地
    /// </summary>
    private readonly AuthConfig AuthConfig = new();

    /// <summary>
    /// 配置文件路径
    /// </summary>
    private readonly string File_Auth_Config = Path.Combine(FileHelper.Dir_Config, "AuthConfig.json");

    public AuthView()
    {
        InitializeComponent();
        MainWindow.WindowClosingEvent += MainWindow_WindowClosingEvent;

        #region 配置文件
        // 如果配置文件不存在就创建（第一次创建）
        if (!File.Exists(File_Auth_Config))
        {
            AuthConfig.IsUseMode1 = true;
            AuthConfig.SelectedIndex = 0;
            AuthConfig.AuthInfos = new();
            // 初始化10个配置文件槽
            for (int i = 0; i < 10; i++)
            {
                AuthConfig.AuthInfos.Add(new()
                {
                    Avatar2 = string.Empty,
                    DisplayName2 = $"配置槽名称 {i + 1}",
                    PersonaId2 = 0,
                    Sid = string.Empty,
                    Remid = string.Empty,
                    AccessToken = string.Empty,
                    SessionId2 = string.Empty
                });
            }
            // 保存配置文件
            SaveConfig();
        }

        // 如果配置文件存在就读取
        if (File.Exists(File_Auth_Config))
        {
            using var streamReader = new StreamReader(File_Auth_Config);
            AuthConfig = JsonHelper.JsonDeserialize<AuthConfig>(streamReader.ReadToEnd());
            streamReader.Close();

            // 读取配置文件名称
            foreach (var item in AuthConfig.AuthInfos)
                ConfigNames.Add(item.DisplayName2);

            // 读取选中配置文件索引
            ComboBox_ConfigNames.SelectedIndex = AuthConfig.SelectedIndex;
        }
        #endregion

        ////////////////////////////////////////////////

        RadioButton_NoHttpProxy.IsChecked = !Globals.IsUseProxy;
        RadioButton_UseHttpProxy.IsChecked = Globals.IsUseProxy;

        TextBox_IPAddress.Text = Globals.IPAddress == default ? string.Empty : Globals.IPAddress.ToString();
        TextBox_Port.Text = Globals.Port == default ? string.Empty : Globals.Port.ToString();

        ////////////////////////////////////////////////

        // 用于接收WebView2传回的数据
        WeakReferenceMessenger.Default.Register<string, string>(this, "SendRemidSid", (s, e) =>
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                AuthModel.Avatar2 = Globals.Avatar2;
                AuthModel.DisplayName2 = Globals.DisplayName2;
                AuthModel.PersonaId2 = Globals.PersonaId2;

                AuthModel.Sid = Globals.Sid;
                AuthModel.Remid = Globals.Remid;
                AuthModel.AccessToken = Globals.AccessToken;
                AuthModel.SessionId2 = Globals.SessionId2;

                SaveConfig();
            });
        });

        ////////////////////////////////////////////

        AuthService.UpdateMode1SessionId();
        AuthService.UpdateMode2SessionId();

        ////////////////////////////////////////////

        AuthService.UpdateMode2DataEvent += AuthService_UpdateMode2DataEvent;
    }

    /// <summary>
    /// 主窗口关闭事件
    /// </summary>
    private void MainWindow_WindowClosingEvent()
    {
        SaveConfig();
    }

    /// <summary>
    /// 保存配置文件
    /// </summary>
    private void SaveConfig()
    {
        var ipAddress = TextBox_IPAddress.Text.Trim();
        var port = TextBox_Port.Text.Trim();

        ConfigHelper.WriteBool("WebProxy", "IsUseProxy", RadioButton_UseHttpProxy.IsChecked == true);

        if (!string.IsNullOrWhiteSpace(ipAddress) &&
            !string.IsNullOrWhiteSpace(port) &&
            IPAddress.TryParse(ipAddress, out IPAddress ipAddressValue) &&
            int.TryParse(port, out int portValue))
        {
            ConfigHelper.WriteString("WebProxy", "IPAddress", ipAddressValue.ToString());
            ConfigHelper.WriteInt("WebProxy", "Port", portValue);
        }

        ////////////////////////////////////////////////

        // 更新当前授权信息
        var index = ComboBox_ConfigNames.SelectedIndex;
        if (index != -1)
        {
            AuthConfig.IsUseMode1 = Globals.IsUseMode1;
            AuthConfig.SelectedIndex = index;

            var auth = AuthConfig.AuthInfos[index];

            auth.Avatar2 = Globals.Avatar2;
            auth.DisplayName2 = Globals.DisplayName2;
            auth.PersonaId2 = Globals.PersonaId2;
            auth.Sid = Globals.Sid;
            auth.Remid = Globals.Remid;
            auth.AccessToken = Globals.AccessToken;
            auth.SessionId2 = Globals.SessionId2;
        }
        // 写入到Json文件
        File.WriteAllText(File_Auth_Config, JsonHelper.JsonSerialize(AuthConfig));
    }

    /// <summary>
    /// 通知更新模式2数据事件
    /// </summary>
    private void AuthService_UpdateMode2DataEvent()
    {
        AuthModel.AccessToken = Globals.AccessToken;

        AuthModel.Avatar2 = Globals.Avatar2;
        AuthModel.DisplayName2 = Globals.DisplayName2;
        AuthModel.PersonaId2 = Globals.PersonaId2;

        AuthModel.Sid = Globals.Sid;
        AuthModel.Remid = Globals.Remid;
        AuthModel.SessionId2 = Globals.SessionId2;
    }

    /// <summary>
    /// ComboBox选中项变更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ComboBox_ConfigNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var index = ComboBox_ConfigNames.SelectedIndex;
        if (index == -1)
            return;

        Globals.IsUseMode1 = AuthConfig.IsUseMode1;

        if (Globals.IsUseMode1)
            RadioButton_Mode1.IsChecked = true;
        else
            RadioButton_Mode2.IsChecked = true;

        ///////////////////////////////////////////

        var auth = AuthConfig.AuthInfos[index];

        AuthModel.Avatar2 = auth.Avatar2;
        AuthModel.DisplayName2 = auth.DisplayName2;
        AuthModel.PersonaId2 = auth.PersonaId2;
        AuthModel.Sid = auth.Sid;
        AuthModel.Remid = auth.Remid;
        AuthModel.AccessToken = auth.AccessToken;
        AuthModel.SessionId2 = auth.SessionId2;

        Globals.Avatar2 = auth.Avatar2;
        Globals.DisplayName2 = auth.DisplayName2;
        Globals.PersonaId2 = auth.PersonaId2;
        Globals.Sid = auth.Sid;
        Globals.Remid = auth.Remid;
        Globals.AccessToken = auth.AccessToken;
        Globals.SessionId2 = auth.SessionId2;

        SaveConfig();
    }

    /// <summary>
    /// 通用超链接点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Hyperlink_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Hyperlink link)
            ProcessHelper.OpenLink(link.NavigateUri.AbsoluteUri);
    }

    /// <summary>
    /// 选择工作模式
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RadioButton_Mode12_Click(object sender, RoutedEventArgs e)
    {
        Globals.IsUseMode1 = RadioButton_Mode1.IsChecked == true;
    }

    /// <summary>
    /// 获取玩家Cookies数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_GetPlayerCookies_Click(object sender, RoutedEventArgs e)
    {
        if (!CoreUtil.CheckWebView2Env())
        {
            NotifierHelper.Show(NotifierType.Warning, "未检测到WebView2对应依赖，请安装对应依赖");
            return;
        }

        var webView2Window = new WebView2Window()
        {
            Owner = MainWindow.MainWindowInstance
        };
        webView2Window.ShowDialog();
    }

    /// <summary>
    /// 刷新玩家Auth信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_RefreshAuthInfo_Click(object sender, RoutedEventArgs e)
    {
        if (Globals.IsUseMode1)
        {
            NotifierHelper.Show(NotifierType.Information, "正在内存扫描中，请稍后...");

            var sessionId = await Scan.GetGatewaySession();
            if (sessionId != string.Empty)
            {
                Globals.SessionId1 = sessionId;
                NotifierHelper.Show(NotifierType.Success, $"内存扫描SessionId成功 {Globals.SessionId1}");
            }
            else
            {
                NotifierHelper.Show(NotifierType.Error, "内存扫描SessionId失败");
            }
        }
        else
        {
            if (string.IsNullOrEmpty(Globals.Remid) || string.IsNullOrEmpty(Globals.Sid))
            {
                NotifierHelper.Show(NotifierType.Warning, "玩家Remid或Sid为空，请先获取玩家Cookies");
                return;
            }

            NotifierHelper.Show(NotifierType.Information, "正在刷新中，请稍后...");

            var respAuth = await EA1API.GetAuthCode(Globals.Remid, Globals.Sid);
            if (respAuth.IsSuccess)
            {
                if (!string.IsNullOrEmpty(respAuth.Remid))
                    Globals.Remid = respAuth.Remid;
                if (!string.IsNullOrEmpty(respAuth.Sid))
                    Globals.Sid = respAuth.Sid;

                var result = await EA2API.GetAccessToken(Globals.Remid, Globals.Sid);
                if (result.IsSuccess)
                {
                    var jNode = JsonNode.Parse(result.Content);
                    Globals.AccessToken = jNode["access_token"].GetValue<string>();
                    AuthModel.AccessToken = Globals.AccessToken;
                    NotifierHelper.Show(NotifierType.Success, "刷新玩家access_token成功");
                }

                result = await BF1API.GetEnvIdViaAuthCode(respAuth.Code);
                if (result.IsSuccess)
                {
                    var envIdViaAuthCode = JsonHelper.JsonDeserialize<EnvIdViaAuthCode>(result.Content);
                    Globals.SessionId2 = envIdViaAuthCode.result.sessionId;
                    Globals.PersonaId2 = long.Parse(envIdViaAuthCode.result.personaId);

                    result = await BF1API.GetPersonasByIds(Globals.SessionId2, Globals.PersonaId);
                    if (result.IsSuccess)
                    {
                        var jNode = JsonNode.Parse(result.Content);
                        var personas = jNode["result"]![$"{Globals.PersonaId}"];
                        if (personas != null)
                        {
                            Globals.Avatar2 = personas!["avatar"].GetValue<string>();
                            Globals.DisplayName2 = personas!["displayName"].GetValue<string>();

                            AuthModel.Avatar2 = Globals.Avatar2;
                            AuthModel.DisplayName2 = Globals.DisplayName2;
                            AuthModel.PersonaId2 = Globals.PersonaId2;

                            AuthModel.Sid = Globals.Sid;
                            AuthModel.Remid = Globals.Remid;
                            AuthModel.SessionId2 = Globals.SessionId2;

                            NotifierHelper.Show(NotifierType.Success, "刷新玩家Cookies数据成功");
                        }
                    }
                    else
                    {
                        NotifierHelper.Show(NotifierType.Error, $"刷新失败\n{result.Content}");
                    }
                }
                else
                {
                    NotifierHelper.Show(NotifierType.Error, $"刷新失败\n{result.Content}");
                }
            }
            else
            {
                NotifierHelper.Show(NotifierType.Error, "刷新失败，玩家Remid或Sid可能已过期");
            }
        }
    }

    /// <summary>
    /// 验证玩家SessionId有效性
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_VerifySessionId_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(Globals.SessionId))
        {
            NotifierHelper.Show(NotifierType.Warning, "玩家SessionId为空，请先获取玩家SessionId");
            return;
        }

        TextBlock_SessionIdState.Text = "正在验证中，请稍后...";
        Border_SessionIdState.Background = Brushes.Gray;
        NotifierHelper.Show(NotifierType.Information, "正在验证中，请稍后...");

        _ = BF1API.SetAPILocale(Globals.SessionId);

        var result = await BF1API.GetWelcomeMessage(Globals.SessionId);
        if (result.IsSuccess)
        {
            var welcomeMsg = JsonHelper.JsonDeserialize<WelcomeMsg>(result.Content);
            var firstMessage = ChsHelper.ToSimplified(welcomeMsg.result.firstMessage);

            TextBlock_SessionIdState.Text = firstMessage;
            Border_SessionIdState.Background = Brushes.Green;
            NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  验证成功\n{firstMessage}");
        }
        else
        {
            TextBlock_SessionIdState.Text = "验证失败";
            Border_SessionIdState.Background = Brushes.OrangeRed;
            NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  验证失败\n{result.Content}");
        }
    }

    /// <summary>
    /// 保存配置文件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_SaveConfig_Click(object sender, RoutedEventArgs e)
    {
        SaveConfig();

        var index = ComboBox_ConfigNames.SelectedIndex;
        if (index != -1)
        {
            ConfigNames[index] = Globals.DisplayName2;
            ComboBox_ConfigNames.SelectedIndex = index;
        }

        NotifierHelper.Show(NotifierType.Success, "保存配置文件成功");

#if DEBUG
        GC.Collect();
        GC.WaitForPendingFinalizers();
#endif
    }

    /// <summary>
    /// 打开配置文件夹
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_OpenConfigFolder_Click(object sender, RoutedEventArgs e)
    {
        ProcessHelper.OpenLink(FileHelper.Dir_Config);
    }
}

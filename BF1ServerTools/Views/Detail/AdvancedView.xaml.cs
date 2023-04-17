using BF1ServerTools.API;
using BF1ServerTools.Helpers;
using BF1ServerTools.Services;

namespace BF1ServerTools.Views.Detail;

/// <summary>
/// AdvancedView.xaml 的交互逻辑
/// </summary>
public partial class AdvancedView : UserControl
{
    /// <summary>
    /// 服务器设置详情Json数据模型
    /// </summary>
    private ServerDetails _serverDetails;
    /// <summary>
    /// 是否成功获取服务器设置详情
    /// </summary>
    private bool _isGetServerDetailsOK = false;

    public AdvancedView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 获取服务器信息（修改前需要重新获取）
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_GetServerAdvancedInfo_Click(object sender, RoutedEventArgs e)
    {
        if (!PlayerUtil.CheckPlayerAuth2())
            return;

        NotifierHelper.Show(NotifierType.Information, $"正在获取服务器 {Globals.ServerId} 数据中...");

        var result = await BF1API.GetServerDetails(Globals.SessionId, Globals.ServerId);
        if (result.IsSuccess)
        {
            _serverDetails = JsonHelper.JsonDese<ServerDetails>(result.Content);

            TextBox_ServerName.Text = _serverDetails.result.serverSettings.name;
            TextBox_ServerDescription.Text = _serverDetails.result.serverSettings.description;

            _isGetServerDetailsOK = true;

            NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  获取服务器 {Globals.ServerId} 数据成功");
        }
        else
        {
            NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  获取服务器 {Globals.ServerId} 数据失败\n{result.Content}");
        }
    }

    /// <summary>
    /// 更新服务器信息（需要服主权限才能修改）
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Button_UpdateServerAdvancedInfo_Click(object sender, RoutedEventArgs e)
    {
        if (!_isGetServerDetailsOK)
        {
            NotifierHelper.Show(NotifierType.Warning, "请先获取服务器信息后，再执行本操作");
            return;
        }

        var serverName = TextBox_ServerName.Text.Trim();
        var serverDescription = TextBox_ServerDescription.Text.Trim();
        serverDescription = ChsHelper.ToTraditional(serverDescription);

        if (!PlayerUtil.CheckPlayerAuth2())
            return;

        if (string.IsNullOrEmpty(serverName))
        {
            NotifierHelper.Show(NotifierType.Warning, "服务器名称不能为空");
            return;
        }

        NotifierHelper.Show(NotifierType.Information, $"正在更新服务器 {Globals.ServerId} 数据中...");

        UpdateServer reqBody = new()
        {
            jsonrpc = "2.0",
            method = "RSP.updateServer"
        };

        var tempParams = new UpdateServer.Params
        {
            deviceIdMap = new UpdateServer.Params.DeviceIdMap()
            {
                machash = Guid.NewGuid().ToString()
            },
            game = "tunguska",
            serverId = Globals.ServerId.ToString(),
            bannerSettings = new UpdateServer.Params.BannerSettings()
            {
                bannerUrl = "",
                clearBanner = true
            }
        };

        var tempMapRotation = new UpdateServer.Params.MapRotation();
        var temp = _serverDetails.result.mapRotations[0];
        var tempMaps = new List<UpdateServer.Params.MapRotation.MapsItem>();
        foreach (var item in temp.maps)
        {
            tempMaps.Add(new UpdateServer.Params.MapRotation.MapsItem()
            {
                gameMode = item.gameMode,
                mapName = item.mapName
            });
        }
        tempMapRotation.maps = tempMaps;
        tempMapRotation.rotationType = temp.rotationType;
        tempMapRotation.mod = temp.mod;
        tempMapRotation.name = temp.name;
        tempMapRotation.description = temp.description;
        tempMapRotation.id = "100";

        tempParams.mapRotation = tempMapRotation;

        tempParams.serverSettings = new UpdateServer.Params.ServerSettings()
        {
            name = serverName,
            description = serverDescription,

            message = _serverDetails.result.serverSettings.message,
            password = _serverDetails.result.serverSettings.password,
            bannerUrl = _serverDetails.result.serverSettings.bannerUrl,
            mapRotationId = _serverDetails.result.serverSettings.mapRotationId,
            customGameSettings = _serverDetails.result.serverSettings.customGameSettings
        };

        reqBody.@params = tempParams;
        reqBody.id = Guid.NewGuid().ToString();

        var result = await BF1API.UpdateServer(Globals.SessionId, reqBody);
        if (result.IsSuccess)
            NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  更新服务器 {Globals.ServerId} 数据成功");
        else
            NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  更新服务器 {Globals.ServerId} 数据失败\n{result.Content}");

        _isGetServerDetailsOK = false;
    }

    /// <summary>
    /// 转换服务器描述文本为简体中文
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_ToSimplified_Click(object sender, RoutedEventArgs e)
    {
        var serverDescription = TextBox_ServerDescription.Text.Trim();

        if (string.IsNullOrEmpty(serverDescription))
        {
            NotifierHelper.Show(NotifierType.Warning, "服务器描述不能为空");
            return;
        }

        TextBox_ServerDescription.Text = ChsHelper.ToSimplified(serverDescription);
        NotifierHelper.Show(NotifierType.Success, "转换服务器描述文本为简体中文成功");
    }

    /// <summary>
    /// 转换服务器描述文本为繁体中文
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_ToTraditional_Click(object sender, RoutedEventArgs e)
    {
        var serverDescription = TextBox_ServerDescription.Text.Trim();

        if (string.IsNullOrEmpty(serverDescription))
        {
            NotifierHelper.Show(NotifierType.Warning, "服务器描述不能为空");
            return;
        }

        TextBox_ServerDescription.Text = ChsHelper.ToTraditional(serverDescription);
        NotifierHelper.Show(NotifierType.Success, "转换服务器描述文本为繁体中文成功");
    }
}

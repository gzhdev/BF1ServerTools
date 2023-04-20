using BF1ServerTools.API;
using BF1ServerTools.Utils;
using BF1ServerTools.Helpers;
using BF1ServerTools.Windows;

namespace BF1ServerTools.Services;

public static class ViewUtil
{
    /// <summary>
    /// 通用未选中警告提示
    /// </summary>
    /// <param name="teamInfo"></param>
    public static void UnSelectedNotifier(string teamInfo)
    {
        NotifierHelper.Show(NotifierType.Warning, $"[{teamInfo}]  当前未选中任何玩家，操作取消");
    }

    /// <summary>
    /// 复制数据到剪切板
    /// </summary>
    /// <param name="text"></param>
    public static void Copy2Clipboard(string text)
    {
        CoreUtil.SetText(text);
        NotifierHelper.Show(NotifierType.Success, $"复制 {text} 到剪切板成功");
    }

    /// <summary>
    /// 踢出玩家（官方理由）
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="personaId"></param>
    /// <param name="reason"></param>
    public static async void KickPlayer(string playerName, long personaId, string reason)
    {
        // 检查权限
        if (!AuthUtil.CheckPlayerAuth())
            return;

        NotifierHelper.Show(NotifierType.Information, $"正在踢出玩家 {playerName} 中...");

        var result = await BF1API.RSPKickPlayer(Globals.SessionId, Globals.GameId, personaId, reason);
        if (result.IsSuccess)
            NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  踢出玩家 {playerName} 成功");
        else
            NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  踢出玩家 {playerName} 失败\n{result.Content}");
    }

    /// <summary>
    /// 踢出玩家（自定义理由）
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="personaId"></param>
    public static void KickPlayerCustom(string playerName, long personaId)
    {
        // 检查权限
        if (!AuthUtil.CheckPlayerAuth())
            return;

        var customKickWindow = new CustomKickWindow(playerName, personaId)
        {
            Owner = MainWindow.MainWindowInstance
        };
        customKickWindow.ShowDialog();
    }

    /// <summary>
    /// 查询玩家战绩
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="personaId"></param>
    /// <param name="rank"></param>
    public static void QueryPlayerRecord(string playerName, long personaId, int rank)
    {
        // 检查权限
        if (!AuthUtil.CheckPlayerSesId())
            return;

        var queryRecordWindow = new QueryRecordWindow(playerName, personaId, rank);
        queryRecordWindow.Show();
    }
}

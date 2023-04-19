using BF1ServerTools.API;
using BF1ServerTools.Data;
using BF1ServerTools.Helpers;

namespace BF1ServerTools.Services;

public static class ServerService
{
    public static event Action<DetailData> UpdateServerDetailDataEvent;

    public static event Action<List<MapInfo>> UpdateServerMapListEvent;
    public static event Action<List<RSPInfo>> UpdateServerAdminListEvent;
    public static event Action<List<RSPInfo>> UpdateServerVIPListEvent;
    public static event Action<List<RSPInfo>> UpdateServerBANListEvent;

    ///////////////////////////////////////////////////////

    private static readonly DetailData DetailData = new();

    private static readonly List<MapInfo> ServerInfo_MapList = new();
    private static readonly List<RSPInfo> ServerInfo_AdminList = new();
    private static readonly List<RSPInfo> ServerInfo_VIPList = new();
    private static readonly List<RSPInfo> ServerInfo_BANList = new();

    ///////////////////////////////////////////////////////

    /// <summary>
    /// 更新服务器详情线程
    /// </summary>
    public static async void UpdateServerDetilsThread()
    {
        bool isClear = true;

        while (true)
        {
            if (ServiceApp.IsDispose)
                return;

            if (Globals.GameId != 0)
            {
                /////////////////// 代表进入服务器 ///////////////////

                // 正常情况下管理员列表至少有一位玩家
                // 排除官方服务器
                // 排除没有服主信息的服务器（bug）
                if (Globals.ServerAdmins_PID.Count == 0 &&
                    DetailData.Name != "OFFICIAL" &&
                    DetailData.OwnerName != "NULL")
                {
                    await UpdateServerDetils();
                }
                else
                {
                    // 已经正常拿到服务器信息
                    isClear = false;
                }
            }
            else
            {
                /////////////////// 代表离开服务器 ///////////////////

                if (!isClear)
                {
                    isClear = true;

                    // 开始清理数据（清理数据只执行一次）
                    ClearData1();
                    ClearData2();

                    ////////////////////// 通知事件 //////////////////////

                    UpdateData1();
                    UpdateData2();
                }
            }

            Thread.Sleep(1000);
        }
    }

    /// <summary>
    /// 重新更新服务器详情
    /// </summary>
    public static void ReUpdateServerDetils()
    {
        ClearData1();
    }

    /// <summary>
    /// 更新服务器详情
    /// </summary>
    private static async Task UpdateServerDetils()
    {
        // 开始联网获取服务器信息
        if (await GetFullServerDetails())
        {
            ////////////////////// 通知事件 //////////////////////

            UpdateData1();
            UpdateData2();
        }
    }

    /// <summary>
    /// 获取当前服务器详情数据
    /// </summary>
    /// <returns></returns>
    private static async Task<bool> GetFullServerDetails()
    {
        if (string.IsNullOrEmpty(Globals.SessionId))
            return false;

        ////////////////////// 数据重置 //////////////////////

        ClearData1();

        DetailData.Name = "获取中...";
        DetailData.Description = "获取中...";
        DetailData.GameId = "获取中...";
        DetailData.Guid = "获取中...";
        DetailData.ServerId = "获取中...";
        DetailData.Bookmark = "获取中...";
        DetailData.OwnerName = "获取中...";
        DetailData.OwnerPersonaId = "获取中...";
        DetailData.OwnerImage = string.Empty;

        ////////////////////// 通知事件 //////////////////////

        UpdateData2();

        ////////////////////// 服务器数据 //////////////////////

        var result = await BF1API.GetFullServerDetails(Globals.SessionId, Globals.GameId);
        if (result.IsSuccess)
        {
            var fullServerDetails = JsonHelper.JsonDeserialize<FullServerDetails>(result.Content);
            if (fullServerDetails.result.serverInfo.serverType == "OFFICIAL")
            {
                DetailData.Name = "OFFICIAL";
                DetailData.Description = "当前进入的是官方服务器，操作取消";
                DetailData.GameId = string.Empty;
                DetailData.Guid = string.Empty;
                DetailData.ServerId = string.Empty;
                DetailData.Bookmark = string.Empty;
                DetailData.OwnerName = string.Empty;
                DetailData.OwnerPersonaId = string.Empty;
                DetailData.OwnerImage = string.Empty;

                LoggerHelper.Warn("当前进入的是官方服务器，操作取消");

                return false;
            }

            Globals.ServerId = int.Parse(fullServerDetails.result.rspInfo.server.serverId);
            Globals.PersistedGameId = fullServerDetails.result.rspInfo.server.persistedGameId;

            DetailData.Name = fullServerDetails.result.serverInfo.name;
            DetailData.Description = fullServerDetails.result.serverInfo.description;

            DetailData.GameId = Globals.GameId.ToString();
            DetailData.Guid = Globals.PersistedGameId;
            DetailData.ServerId = Globals.ServerId.ToString();

            int index = 0;
            if (fullServerDetails.result.rspInfo.owner != null)
            {
                DetailData.OwnerName = fullServerDetails.result.rspInfo.owner.displayName;
                DetailData.OwnerPersonaId = fullServerDetails.result.rspInfo.owner.personaId;
                DetailData.OwnerImage = fullServerDetails.result.rspInfo.owner.avatar;

                // 服主
                ServerInfo_AdminList.Add(new()
                {
                    Index = index++,
                    Avatar = fullServerDetails.result.rspInfo.owner.avatar,
                    DisplayName = fullServerDetails.result.rspInfo.owner.displayName,
                    PersonaId = long.Parse(fullServerDetails.result.rspInfo.owner.personaId)
                });
                Globals.ServerAdmins_PID.Add(long.Parse(fullServerDetails.result.rspInfo.owner.personaId));
            }
            else
            {
                DetailData.OwnerName = "NULL";
                DetailData.OwnerPersonaId = "NULL";

                LoggerHelper.Warn("检测到Bug服务器，工具可能会出现异常");
            }

            DetailData.Bookmark = $"★ {fullServerDetails.result.serverInfo.serverBookmarkCount}";

            // 地图列表
            index = 0;
            foreach (var item in fullServerDetails.result.serverInfo.rotation)
            {
                ServerInfo_MapList.Add(new()
                {
                    Index = index++,
                    MapImage = ClientUtil.GetTempImagePath(item.mapImage, "map"),
                    MapName = ChsHelper.ToSimplified(item.mapPrettyName),
                    MapMode = ChsHelper.ToSimplified(item.modePrettyName)
                });
            }

            // 管理员列表
            index = 1;
            foreach (var item in fullServerDetails.result.rspInfo.adminList)
            {
                ServerInfo_AdminList.Add(new()
                {
                    Index = index++,
                    Avatar = item.avatar,
                    DisplayName = item.displayName,
                    PersonaId = long.Parse(item.personaId)
                });

                Globals.ServerAdmins_PID.Add(long.Parse(item.personaId));
            }

            // VIP列表
            index = 1;
            foreach (var item in fullServerDetails.result.rspInfo.vipList)
            {
                ServerInfo_VIPList.Add(new()
                {
                    Index = index++,
                    Avatar = item.avatar,
                    DisplayName = item.displayName,
                    PersonaId = long.Parse(item.personaId)
                });

                Globals.ServerVIPs_PID.Add(long.Parse(item.personaId));
            }

            // BAN列表
            index = 1;
            foreach (var item in fullServerDetails.result.rspInfo.bannedList)
            {
                ServerInfo_BANList.Add(new()
                {
                    Index = index++,
                    Avatar = item.avatar,
                    DisplayName = item.displayName,
                    PersonaId = long.Parse(item.personaId)
                });
            }

            return true;
        }
        else
        {
            ClearData2();

            return false;
        }
    }

    /// <summary>
    /// 清理数据1
    /// </summary>
    private static void ClearData1()
    {
        Globals.ServerAdmins_PID.Clear();
        Globals.ServerVIPs_PID.Clear();

        Globals.ServerId = 0;
        Globals.PersistedGameId = string.Empty;

        ServerInfo_MapList.Clear();
        ServerInfo_AdminList.Clear();
        ServerInfo_VIPList.Clear();
        ServerInfo_BANList.Clear();
    }

    /// <summary>
    /// 清理数据2
    /// </summary>
    private static void ClearData2()
    {
        DetailData.Name = string.Empty;
        DetailData.Description = string.Empty;
        DetailData.GameId = string.Empty;
        DetailData.Guid = string.Empty;
        DetailData.ServerId = string.Empty;
        DetailData.Bookmark = string.Empty;
        DetailData.OwnerName = string.Empty;
        DetailData.OwnerPersonaId = string.Empty;
        DetailData.OwnerImage = string.Empty;
    }

    /// <summary>
    /// 更新数据1
    /// </summary>
    private static void UpdateData1()
    {
        UpdateServerMapListEvent?.Invoke(ServerInfo_MapList);
        UpdateServerAdminListEvent?.Invoke(ServerInfo_AdminList);
        UpdateServerVIPListEvent?.Invoke(ServerInfo_VIPList);
        UpdateServerBANListEvent?.Invoke(ServerInfo_BANList);
    }

    /// <summary>
    /// 更新数据2
    /// </summary>
    private static void UpdateData2()
    {
        UpdateServerDetailDataEvent?.Invoke(DetailData);
    }
}

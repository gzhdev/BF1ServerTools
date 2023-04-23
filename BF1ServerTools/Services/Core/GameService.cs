using BF1ServerTools.SDK;
using BF1ServerTools.Data;

namespace BF1ServerTools.Services;

public static class GameService
{
    public static event Action<ServerData> UpdateServerDataEvent;

    public static event Action<TeamData> UpdateTeam1DataEvent;
    public static event Action<TeamData> UpdateTeam2DataEvent;

    public static event Action<List<PlayerData>> UpdatePlayerListTeam01Event;
    public static event Action<List<PlayerData>> UpdatePlayerListTeam02Event;

    public static event Action<List<PlayerData>> UpdatePlayerListTeam1Event;
    public static event Action<List<PlayerData>> UpdatePlayerListTeam2Event;

    ///////////////////////////////////////////////////////

    private static readonly ServerData ServerData = new();

    private static readonly TeamData Team1Data = new();
    private static readonly TeamData Team2Data = new();

    private static readonly List<PlayerData> PlayerList_Team01 = new();
    private static readonly List<PlayerData> PlayerList_Team02 = new();

    private static readonly List<PlayerData> PlayerList_Team1 = new();
    private static readonly List<PlayerData> PlayerList_Team2 = new();

    ///////////////////////////////////////////////////////

    /// <summary>
    /// 更新战地1游戏信息线程
    /// </summary>
    public static void UpdateGameInfoThread()
    {
        while (true)
        {
            if (ServiceApp.IsDispose)
                return;

            ////////////////////// 数据重置 //////////////////////

            PlayerList_Team01.Clear();
            PlayerList_Team02.Clear();

            PlayerList_Team1.Clear();
            PlayerList_Team2.Clear();

            ////////////////////// 服务器数据 //////////////////////

            GetServerData(ServerData, Team1Data, Team2Data);

            ////////////////////// 全局数据 //////////////////////

            // 如果玩家没有进入服务器，要进行一些数据清理
            if (ServerData.MapName == "大厅菜单")
            {
                Globals.GameId = 0;

                Globals.ServerAdmins_PID.Clear();
                Globals.ServerVIPs_PID.Clear();

                Globals.AutoKickBreakRulePlayer = false;
            }
            else
            {
                Globals.GameId = ServerData.GameId;
            }

            ////////////////////// 玩家数据 //////////////////////

            foreach (var item in Player.GetPlayerList())
            {
                // 用于小队排序
                if (item.SquadId == 0)
                    item.SquadId = 99;

                item.SquadName = ClientUtil.GetSquadNameById(item.SquadId);

                item.KD = PlayerUtil.GetPlayerKD(item.Kill, item.Dead);
                item.KPM = PlayerUtil.GetPlayerKPMBySecond(item.Kill, ServerData.Time);

                item.LifeKD = GameUtil.GetLifeKD(item.PersonaId);
                item.LifeKPM = GameUtil.GetLifeKPM(item.PersonaId);
                item.LifeTime = GameUtil.GetLifeTime(item.PersonaId);

                item.IsAdmin = GameUtil.IsServerAdmin(item.PersonaId);
                item.IsVIP = GameUtil.IsServerVIP(item.PersonaId);
                item.IsWhite = GameUtil.IsServerWhite(item.Name);

                item.KitImg = ClientUtil.GetPlayerKitImage(item.Kit);
                item.KitName = ClientUtil.GetPlayerKitName(item.Kit);

                // 主武器生涯星数
                item.LifeStar = GameUtil.GetLifeStar(item.PersonaId, item.WeaponS0);

                // 显示中文武器名称
                item.WeaponS0 = ClientUtil.GetWeaponChsName(item.WeaponS0);
                item.WeaponS1 = ClientUtil.GetWeaponChsName(item.WeaponS1);
                item.WeaponS2 = ClientUtil.GetWeaponChsName(item.WeaponS2);
                item.WeaponS3 = ClientUtil.GetWeaponChsName(item.WeaponS3);
                item.WeaponS4 = ClientUtil.GetWeaponChsName(item.WeaponS4);
                item.WeaponS5 = ClientUtil.GetWeaponChsName(item.WeaponS5);
                item.WeaponS6 = ClientUtil.GetWeaponChsName(item.WeaponS6);
                item.WeaponS7 = ClientUtil.GetWeaponChsName(item.WeaponS7);

                switch (item.TeamId)
                {
                    case 0:
                        if (GameUtil.IsSpectator(item.Spectator))
                            // 观战
                            PlayerList_Team01.Add(item);
                        else if (GameUtil.IsInGame())
                            // 载入中（排除在大厅情况）
                            PlayerList_Team02.Add(item);
                        break;
                    case 1:
                        PlayerList_Team1.Add(item);
                        break;
                    case 2:
                        PlayerList_Team2.Add(item);
                        break;
                }
            }

            // 队伍1数据统计
            StatisticTeamInfo(PlayerList_Team1, Team1Data);
            // 队伍2数据统计
            StatisticTeamInfo(PlayerList_Team2, Team2Data);

            // 统计队伍1玩家数量
            Team1Data.MaxPlayerCount = PlayerList_Team1.Count;
            // 统计队伍2玩家数量
            Team2Data.MaxPlayerCount = PlayerList_Team2.Count;

            // 统计当前服务器玩家数量
            ServerData.AllPlayerCount = Team1Data.MaxPlayerCount + Team2Data.MaxPlayerCount;

            ////////////////////// 通知事件 //////////////////////

            UpdateServerDataEvent?.Invoke(ServerData);

            UpdateTeam1DataEvent?.Invoke(Team1Data);
            UpdateTeam2DataEvent?.Invoke(Team2Data);

            UpdatePlayerListTeam01Event?.Invoke(PlayerList_Team01);
            UpdatePlayerListTeam02Event?.Invoke(PlayerList_Team02);

            UpdatePlayerListTeam1Event?.Invoke(PlayerList_Team1);
            UpdatePlayerListTeam2Event?.Invoke(PlayerList_Team2);

            Thread.Sleep(1000);
        }
    }

    /// <summary>
    /// 获取服务器数据
    /// </summary>
    /// <param name="serverData"></param>
    /// <param name="team1Data"></param>
    /// <param name="team2Data"></param>
    public static void GetServerData(ServerData serverData, TeamData team1Data, TeamData team2Data)
    {
        team1Data.Reset();
        team2Data.Reset();

        // 服务器名称
        serverData.Name = Server.GetServerName();
        // 修正名称
        serverData.Name = string.IsNullOrEmpty(serverData.Name) ? "未知" : serverData.Name;

        // 服务器数字Id
        serverData.GameId = Server.GetGameId();

        // 服务器地图名称
        serverData.MapName = Server.GetMapName();

        team1Data.TeamImg = ClientUtil.GetTeam1Image(serverData.MapName);
        team1Data.TeamName = ClientUtil.GetTeam1ChsName(serverData.MapName);

        team2Data.TeamImg = ClientUtil.GetTeam2Image(serverData.MapName);
        team2Data.TeamName = ClientUtil.GetTeam2ChsName(serverData.MapName);

        // 服务器地图预览图
        serverData.MapImg = ClientUtil.GetMapImage(serverData.MapName);

        // 服务器地图中文名称
        serverData.MapName = ClientUtil.GetMapChsName(serverData.MapName);
        // 修正名称
        serverData.MapName = string.IsNullOrEmpty(serverData.MapName) ? "未知" : serverData.MapName;

        // 服务器游戏模式
        serverData.GameMode = Server.GetGameMode();
        // 服务器游戏模式中文名称
        serverData.GameMode = ClientUtil.GetGameMode(serverData.GameMode);
        // 修正名称
        serverData.GameMode = serverData.MapName == "未知" || serverData.MapName == "大厅菜单" ? "未知" : serverData.GameMode;

        // 服务器时间
        serverData.Time = Server.GetServerTime();
        // 服务器时间 - 字符串
        serverData.GameTime = GameUtil.GetMMSSStrBySecond(serverData.Time);

        // 最大比分
        team1Data.MaxScore = Server.GetServerMaxScore();
        team2Data.MaxScore = Server.GetServerMaxScore();

        // 队伍1分数
        team1Data.AllScore = Server.GetTeamScore(1);
        team1Data.ScoreKill = Server.GetTeamKillScore(1);
        team1Data.ScoreFlag = Server.GetTeamFlagScore(1);
        // 队伍2分数
        team2Data.AllScore = Server.GetTeamScore(2);
        team2Data.ScoreKill = Server.GetTeamKillScore(2);
        team2Data.ScoreFlag = Server.GetTeamFlagScore(2);
    }

    /// <summary>
    /// 统计队伍相关数据
    /// </summary>
    /// <param name="playerData"></param>
    /// <param name="teamData"></param>
    private static void StatisticTeamInfo(List<PlayerData> playerData, TeamData teamData)
    {
        foreach (var item in playerData)
        {
            // 统计当前服务器存活玩家数量
            if (item.Kit != string.Empty)
                teamData.PlayerCount++;

            // 统计当前服务器150级玩家数量
            if (item.Rank == 150)
                teamData.Rank150PlayerCount++;

            // 统计突击兵数量
            if (item.Kit == "ID_M_ASSAULT")
                teamData.AssaultKitCount++;

            // 统计医疗兵数量
            if (item.Kit == "ID_M_MEDIC")
                teamData.MedicKitCount++;

            // 统计支援兵数量
            if (item.Kit == "ID_M_SUPPORT")
                teamData.SupportKitCount++;

            // 统计侦查兵数量
            if (item.Kit == "ID_M_SCOUT")
                teamData.ScoutKitCount++;

            // 总击杀数统计
            teamData.AllKillCount += item.Kill;
            // 总死亡数统计
            teamData.AllDeadCount += item.Dead;
        }
    }
}

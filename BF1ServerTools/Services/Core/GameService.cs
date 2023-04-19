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

            Team1Data.Reset();
            Team2Data.Reset();

            ////////////////////// 服务器数据 //////////////////////

            // 服务器名称
            ServerData.Name = Server.GetServerName();
            // 修正名称
            ServerData.Name = string.IsNullOrEmpty(ServerData.Name) ? "未知" : ServerData.Name;

            // 服务器数字Id
            ServerData.GameId = Server.GetGameId();

            // 服务器地图名称
            ServerData.MapName = Server.GetMapName();

            Team1Data.TeamImg = ClientUtil.GetTeam1Image(ServerData.MapName);
            Team1Data.TeamName = ClientUtil.GetTeamChsName(ServerData.MapName, 1);

            Team2Data.TeamImg = ClientUtil.GetTeam2Image(ServerData.MapName);
            Team2Data.TeamName = ClientUtil.GetTeamChsName(ServerData.MapName, 2);

            // 服务器地图预览图
            ServerData.MapImg = ClientUtil.GetMapPreImage(ServerData.MapName);

            // 服务器地图中文名称
            ServerData.MapName = ClientUtil.GetMapChsName(ServerData.MapName);
            // 修正名称
            ServerData.MapName = string.IsNullOrEmpty(ServerData.MapName) ? "未知" : ServerData.MapName;

            // 服务器游戏模式
            ServerData.GameMode = Server.GetGameMode();
            // 服务器游戏模式中文名称
            ServerData.GameMode = ClientUtil.GetGameMode(ServerData.GameMode);
            // 修正名称
            ServerData.GameMode = ServerData.MapName == "未知" || ServerData.MapName == "大厅菜单" ? "未知" : ServerData.GameMode;

            // 服务器时间
            ServerData.Time = Server.GetServerTime();
            // 服务器时间 - 字符串
            ServerData.GameTime = GameUtil.GetMMSSStrBySecond(ServerData.Time);

            // 最大比分
            Team1Data.MaxScore = Server.GetServerMaxScore();
            Team2Data.MaxScore = Server.GetServerMaxScore();

            // 队伍1分数
            Team1Data.AllScore = Server.GetTeamScore(1);
            Team1Data.ScoreKill = Server.GetTeamKillScore(1);
            Team1Data.ScoreFlag = Server.GetTeamFlagScore(1);
            // 队伍2分数
            Team2Data.AllScore = Server.GetTeamScore(2);
            Team2Data.ScoreKill = Server.GetTeamKillScore(2);
            Team2Data.ScoreFlag = Server.GetTeamFlagScore(2);

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
                        if (item.Spectator == 0x01)
                            PlayerList_Team01.Add(item);
                        else if (ServerData.GameId != 0)
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

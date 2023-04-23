using BF1ServerTools.SDK;
using BF1ServerTools.Data;
using BF1ServerTools.SQLite;

namespace BF1ServerTools.Services;

public static class MonitService
{
    public static event Action UpdateBreakPlayerEvent;

    public static event Action<ChangeTeamSheet> ChangeTeamSheetEvent;

    ///////////////////////////////////////////////////////

    private static readonly ServerData ServerData = new();

    private static readonly TeamData Team1Data = new();
    private static readonly TeamData Team2Data = new();

    private static List<PlayerData> PlayerList_Team1 = new();
    private static List<PlayerData> PlayerList_Team2 = new();

    private static List<PlayerData> PlayerList_Team1_Swap = new();
    private static List<PlayerData> PlayerList_Team2_Swap = new();

    ///////////////////////////////////////////////////////

    /// <summary>
    /// 更新当前服务器违规玩家信息线程
    /// </summary>
    public static void UpdateBreakPlayerThread()
    {
        while (true)
        {
            if (ServiceApp.IsDispose)
                return;

            /////////////////////////////////////////////

            PlayerList_Team1.Clear();
            PlayerList_Team2.Clear();

            Globals.PlayerBreakRuleInfos.Clear();

            /////////////////////////////////////////////

            GameService.GetServerData(ServerData, Team1Data, Team2Data);

            /////////////////////////////////////////////

            var second = Server.GetServerTime();

            foreach (var item in Player.GetPlayerList())
            {
                item.KD = PlayerUtil.GetPlayerKD(item.Kill, item.Dead);
                item.KPM = PlayerUtil.GetPlayerKPMBySecond(item.Kill, second);

                item.LifeKD = GameUtil.GetLifeKD(item.PersonaId);
                item.LifeKPM = GameUtil.GetLifeKPM(item.PersonaId);

                item.IsAdmin = GameUtil.IsServerAdmin(item.PersonaId);
                item.IsWhite = GameUtil.IsServerWhite(item.Name);

                // 黑名单
                for (int i = 0; i < Globals.CustomBlacks_Name.Count; i++)
                {
                    if (item.Name == Globals.CustomBlacks_Name[i])
                    {
                        AddBreakRulePlayerInfo(item, BreakType.Black, "Server Black List");
                    }
                }

                switch (item.TeamId)
                {
                    case 0:
                        // 检查队伍0违规玩家
                        if (GameUtil.IsSpectator(item.Spectator))
                            CheckTeam0BreakRuleInfo(item);
                        break;
                    case 1:
                        PlayerList_Team1.Add(item);
                        // 检查队伍1违规玩家
                        CheckTeam12BreakRuleInfo(item, Globals.ServerRule_Team1, Globals.CustomWeapons_Team1);
                        CheckTeam12BreakRuleInfo(item, Globals.ServerRule_Team1);
                        break;
                    case 2:
                        PlayerList_Team2.Add(item);
                        // 检查队伍2违规玩家
                        CheckTeam12BreakRuleInfo(item, Globals.ServerRule_Team2, Globals.CustomWeapons_Team2);
                        CheckTeam12BreakRuleInfo(item, Globals.ServerRule_Team1);
                        break;
                }
            }

            // 填充默认规则
            foreach (var item in Globals.PlayerBreakRuleInfos)
            {
                item.Reason = item.BreakInfos[0].Reason;
            }

            // 检测换边玩家
            CheckChangeTeam();

            /////////////////////////////////////////////

            UpdateBreakPlayerEvent?.Invoke();

            /////////////////////////////////////////////

            Thread.Sleep(1000);
        }
    }

    /// <summary>
    /// 增加违规玩家信息
    /// </summary>
    /// <param name="playerData"></param>
    /// <param name="breakType"></param>
    /// <param name="reason"></param>
    private static void AddBreakRulePlayerInfo(PlayerData playerData, BreakType breakType, string reason)
    {
        // 查询这个玩家在违规列表是否已经存在
        var index = Globals.PlayerBreakRuleInfos.FindIndex(val => val.PersonaId == playerData.PersonaId);
        if (index == -1)
        {
            // 如果不存在就添加到列表
            Globals.PlayerBreakRuleInfos.Add(new()
            {
                Rank = playerData.Rank,
                Name = playerData.Name,
                PersonaId = playerData.PersonaId,
                IsAdmin = playerData.IsAdmin,
                IsWhite = playerData.IsWhite,
                BreakInfos = new()
                {
                    new()
                    {
                        BreakType = breakType,
                        Reason = reason
                    }
                }
            });
        }
        else
        {
            // 如果存在就仅增加违规信息
            Globals.PlayerBreakRuleInfos[index].BreakInfos.Add(new()
            {
                BreakType = breakType,
                Reason = reason
            });
        }
    }

    /// <summary>
    /// 检查队伍0 限制观战 违规信息
    /// </summary>
    /// <param name="playerData"></param>
    private static void CheckTeam0BreakRuleInfo(PlayerData playerData)
    {
        // 限制观战
        if (Globals.IsAutoKickSpectator)
        {
            playerData.Rank = -1;
            AddBreakRulePlayerInfo(playerData, BreakType.Spectator, "Server BAN Spectator");
        }
    }

    /// <summary>
    /// 检查队伍12 通用规则 违规信息
    /// </summary>
    /// <param name="playerData"></param>
    /// <param name="serverRule"></param>
    /// <param name="customWeapons"></param>
    private static void CheckTeam12BreakRuleInfo(PlayerData playerData, ServerRule serverRule, List<string> customWeapons)
    {
        // 限制玩家击杀
        if (playerData.Kill > serverRule.MaxKill &&
            serverRule.MaxKill != 0)
        {
            AddBreakRulePlayerInfo(playerData, BreakType.Kill, $"Kill Limit {serverRule.MaxKill:0}");
        }

        // 计算玩家KD最低击杀数
        if (playerData.Kill > serverRule.FlagKD &&
            serverRule.FlagKD != 0)
        {
            // 限制玩家KD
            if (playerData.KD > serverRule.MaxKD &&
                serverRule.MaxKD != 0.00f)
            {
                AddBreakRulePlayerInfo(playerData, BreakType.KD, $"KD Limit {serverRule.MaxKD:0.00}");
            }
        }

        // 计算玩家KPM比条件
        if (playerData.Kill > serverRule.FlagKPM &&
            serverRule.FlagKPM != 0)
        {
            // 限制玩家KPM
            if (playerData.KPM > serverRule.MaxKPM &&
                serverRule.MaxKPM != 0.00f)
            {
                AddBreakRulePlayerInfo(playerData, BreakType.KPM, $"KPM Limit {serverRule.MaxKPM:0.00}");
            }
        }

        // 限制玩家最低等级
        if (playerData.Rank < serverRule.MinRank &&
            serverRule.MinRank != 0 &&
            playerData.Rank != 0)
        {
            AddBreakRulePlayerInfo(playerData, BreakType.Rank, $"Min Rank Limit {serverRule.MinRank:0}");
        }

        // 限制玩家最高等级
        if (playerData.Rank > serverRule.MaxRank &&
            serverRule.MaxRank != 0 &&
            playerData.Rank != 0)
        {
            AddBreakRulePlayerInfo(playerData, BreakType.Rank, $"Max Rank Limit {serverRule.MaxRank:0}");
        }

        // 从武器规则里遍历限制武器名称
        for (int i = 0; i < customWeapons.Count; i++)
        {
            var item = customWeapons[i];

            // K 弹
            if (item == "_KBullet")
            {
                if (playerData.WeaponS2.Contains("_KBullet") ||
                    playerData.WeaponS5.Contains("_KBullet"))
                {
                    AddBreakRulePlayerInfo(playerData, BreakType.Weapon, "Weapon Limit K Bullet");
                }
            }

            // 步枪手榴弹（破片）
            if (item == "_RGL_Frag")
            {
                if (playerData.WeaponS2.Contains("_RGL_Frag") ||
                    playerData.WeaponS5.Contains("_RGL_Frag"))
                {
                    AddBreakRulePlayerInfo(playerData, BreakType.Weapon, "Weapon Limit RGL Frag");
                }
            }

            // 步枪手榴弹（烟雾）
            if (item == "_RGL_Smoke")
            {
                if (playerData.WeaponS2.Contains("_RGL_Smoke") ||
                    playerData.WeaponS5.Contains("_RGL_Smoke"))
                {
                    AddBreakRulePlayerInfo(playerData, BreakType.Weapon, "Weapon Limit RGL Smoke");
                }
            }

            // 步枪手榴弹（高爆）
            if (item == "_RGL_HE")
            {
                if (playerData.WeaponS2.Contains("_RGL_HE") ||
                    playerData.WeaponS5.Contains("_RGL_HE"))
                {
                    AddBreakRulePlayerInfo(playerData, BreakType.Weapon, "Weapon Limit RGL HE");
                }
            }

            // 其他违规武器
            if (playerData.WeaponS0 == item ||
                playerData.WeaponS1 == item ||
                playerData.WeaponS2 == item ||
                playerData.WeaponS3 == item ||
                playerData.WeaponS4 == item ||
                playerData.WeaponS5 == item ||
                playerData.WeaponS6 == item ||
                playerData.WeaponS7 == item)
            {
                AddBreakRulePlayerInfo(playerData, BreakType.Weapon, $"Weapon Limit {ClientUtil.GetWeaponShortTxt(item)}");
            }
        }
    }

    /// <summary>
    /// 检查队伍12 生涯规则 违规信息
    /// </summary>
    /// <param name="playerData"></param>
    /// <param name="serverRule"></param>
    private static void CheckTeam12BreakRuleInfo(PlayerData playerData, ServerRule serverRule)
    {
        // 先查找玩家生涯缓存
        var lifeCache = GameUtil.FindPlayerLifeCache(playerData.PersonaId);
        if (lifeCache != null)
        {
            // 限制玩家生涯KD
            if (serverRule.LifeMaxKD != 0 &&
                lifeCache.KD > serverRule.LifeMaxKD)
            {
                AddBreakRulePlayerInfo(playerData, BreakType.LifeKD, $"Life KD Limit {serverRule.LifeMaxKD:0.00}");
            }

            // 限制玩家生涯KPM
            if (serverRule.LifeMaxKPM != 0 &&
                lifeCache.KPM > serverRule.LifeMaxKPM)
            {
                AddBreakRulePlayerInfo(playerData, BreakType.LifeKPM, $"Life KPM Limit {serverRule.LifeMaxKPM:0.00}");
            }

            // 限制玩家武器最高星数
            if (serverRule.LifeMaxWeaponStar != 0)
            {
                // 武器Guid列表
                var guidList = new List<string>
                {
                    ClientUtil.GetWeaponGuid(playerData.WeaponS0),
                    ClientUtil.GetWeaponGuid(playerData.WeaponS1),
                    ClientUtil.GetWeaponGuid(playerData.WeaponS2),
                    ClientUtil.GetWeaponGuid(playerData.WeaponS3),
                    ClientUtil.GetWeaponGuid(playerData.WeaponS4),
                    ClientUtil.GetWeaponGuid(playerData.WeaponS5),
                    ClientUtil.GetWeaponGuid(playerData.WeaponS6),
                    ClientUtil.GetWeaponGuid(playerData.WeaponS7)
                };

                foreach (var guid in guidList)
                {
                    if (string.IsNullOrWhiteSpace(guid))
                        continue;

                    var weapon = lifeCache.WeaponStats.Find(x => x.guid == guid);
                    if (weapon != null && weapon.star > serverRule.LifeMaxWeaponStar)
                    {
                        AddBreakRulePlayerInfo(playerData, BreakType.LifeWeaponStar, $"Life Weapon Star Limit {serverRule.LifeMaxWeaponStar:0}");
                    }
                }
            }

            // 限制玩家载具最高星数
            if (serverRule.LifeMaxVehicleStar != 0)
            {
                // 只判断主武器槽
                var guid = ClientUtil.GetWeaponGuid(playerData.WeaponS0);

                if (!string.IsNullOrWhiteSpace(guid))
                {
                    var vehicel = lifeCache.VehicleStats.Find(x => x.guid == guid);
                    if (vehicel != null && vehicel.star > serverRule.LifeMaxVehicleStar)
                    {
                        AddBreakRulePlayerInfo(playerData, BreakType.LifeVehicleStar, $"Life Vehicle Star Limit {serverRule.LifeMaxVehicleStar:0}");
                    }
                }
            }
        }
    }

    /// <summary>
    /// 检测换边玩家
    /// </summary>
    private static void CheckChangeTeam()
    {
        // 如果玩家没有进入服务器，不检测换边情况
        if (!GameUtil.IsInGame())
            return;

        // 如果双方玩家人数都为0，不检测换边情况
        if (PlayerList_Team1.Count == 0 && PlayerList_Team2.Count == 0)
        {
            if (PlayerList_Team1_Swap.Count != 0)
                PlayerList_Team1_Swap.Clear();
            if (PlayerList_Team2_Swap.Count != 0)
                PlayerList_Team2_Swap.Clear();

            return;
        }

        // 第一次初始化
        if (PlayerList_Team1_Swap.Count == 0 && PlayerList_Team2_Swap.Count == 0)
        {
            PlayerList_Team1_Swap = CopyPlayerDataList(PlayerList_Team1);
            PlayerList_Team2_Swap = CopyPlayerDataList(PlayerList_Team2);

            return;
        }

        // 检测换边
        CheckTeamChangeTeam(PlayerList_Team2, PlayerList_Team1_Swap, 1);
        CheckTeamChangeTeam(PlayerList_Team1, PlayerList_Team2_Swap, 2);

        // 更新临时数据
        PlayerList_Team1_Swap = CopyPlayerDataList(PlayerList_Team1);
        PlayerList_Team2_Swap = CopyPlayerDataList(PlayerList_Team2);
    }

    /// <summary>
    /// 检测队伍换边情况，并通知
    /// </summary>
    /// <param name="playerDataList"></param>
    /// <param name="playerDataListSwap"></param>
    /// <param name="teamId"></param>
    private static void CheckTeamChangeTeam(List<PlayerData> playerDataList, List<PlayerData> playerDataListSwap, int teamId)
    {
        string changeFlag;
        if (teamId == 1)
            changeFlag = ">>>";
        else if (teamId == 2)
            changeFlag = "<<<";
        else
            changeFlag = string.Empty;

        // 临时保存的队伍玩家列表
        foreach (var item in playerDataListSwap)
        {
            // 查询这个玩家是否在目前的队伍中
            var result = playerDataList.Find(var => var.PersonaId == item.PersonaId);
            if (result != null)
            {
                // 队伍换边日志
                var changeTeamSheet = new ChangeTeamSheet()
                {
                    ServerName = ServerData.Name,
                    GameId = ServerData.GameId,
                    GameTime = ServerData.GameTime,

                    Rank = item.Rank,
                    Name = item.Name,
                    PersonaId = item.PersonaId,

                    GameMode = ServerData.GameMode,
                    MapName = ServerData.MapName,

                    Team1Name = Team1Data.TeamName,
                    Team2Name = Team2Data.TeamName,
                    TeamScore = $"{Team1Data.AllScore} - {Team2Data.AllScore}",

                    State = $"{Team1Data.TeamName} {changeFlag} {Team2Data.TeamName}",

                    CreateTime = DateTime.Now
                };
                // 通知事件
                ChangeTeamSheetEvent?.Invoke(changeTeamSheet);
            }
        }
    }

    /// <summary>
    /// 玩家列表深复制
    /// </summary>
    /// <param name="originalList"></param>
    /// <returns></returns>
    private static List<PlayerData> CopyPlayerDataList(List<PlayerData> originalList)
    {
        var list = new List<PlayerData>();
        foreach (var item in originalList)
        {
            var data = new PlayerData()
            {
                Rank = item.Rank,
                Name = item.Name,
                PersonaId = item.PersonaId
            };
            list.Add(data);
        }
        return list;
    }
}

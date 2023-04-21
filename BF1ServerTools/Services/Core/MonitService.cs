using BF1ServerTools.Data;
using BF1ServerTools.SDK;

namespace BF1ServerTools.Services;

public static class MonitService
{
    /// <summary>
    /// 更新当前服务器违规玩家信息线程
    /// </summary>
    public static void UpdateBreakPlayerThread()
    {
        while (true)
        {
            if (ServiceApp.IsDispose)
                return;

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
            }

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
        var index = Globals.BreakRuleInfo_PlayerList.FindIndex(val => val.PersonaId == playerData.PersonaId);
        if (index == -1)
        {
            // 如果不存在就添加到列表
            Globals.BreakRuleInfo_PlayerList.Add(new()
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
            Globals.BreakRuleInfo_PlayerList[index].BreakInfos.Add(new()
            {
                BreakType = breakType,
                Reason = reason
            });
        }
    }
}

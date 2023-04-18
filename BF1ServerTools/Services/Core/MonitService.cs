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

            var time = GameUtil.SecondsToMinute(Server.GetServerTime());

            foreach (var item in Player.GetPlayerList())
            {
                item.KD = GameUtil.GetPlayerKD(item.Kill, item.Dead);
                item.KPM = GameUtil.GetPlayerKPM(item.Kill, time);

                //item.LifeKD = GameUtil.GetLifeKD(item.PersonaId);
                //item.LifeKPM = GameUtil.GetLifeKPM(item.PersonaId);

                item.IsAdmin = GameUtil.IsAdminVIP(item.PersonaId, Globals.ServerAdmins_PID);
                item.IsWhite = GameUtil.IsWhite(item.Name, Globals.CustomWhites_Name);


            }

            Thread.Sleep(1000);
        }
    }
}

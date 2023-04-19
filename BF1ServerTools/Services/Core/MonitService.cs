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


            }

            Thread.Sleep(1000);
        }
    }
}

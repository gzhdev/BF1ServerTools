using BF1ServerTools.SDK;
using BF1ServerTools.Data;

namespace BF1ServerTools.Services;

public static class MainService
{
    public static event Action<MainData> UpdateMainDataEvent;

    ///////////////////////////////////////////////////////

    private static readonly MainData MainData = new();

    public static void UpdateMainDataThread()
    {
        while (true)
        {
            if (ServiceApp.IsDispose)
                return;

            var localData = Player.GetPlayerLocal();
            Globals.DisplayName1 = localData.DisplayName;
            Globals.PersonaId1 = localData.PersonaId;

            ////////////////////// 通知事件 //////////////////////

            UpdateMainDataEvent?.Invoke(MainData);

            Thread.Sleep(1000);
        }
    }
}

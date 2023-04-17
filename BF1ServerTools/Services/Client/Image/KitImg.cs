namespace BF1ServerTools.Services;

public static class KitImg
{
    public static Dictionary<string, string> KitDict = new();
    public static Dictionary<string, string> Kit2Dict = new();

    static KitImg()
    {
        InitDict();
    }

    private static void InitDict()
    {
        KitDict.Add("Scout.png", @"\Assets\Caches\Classes\Scout.png");
        KitDict.Add("Support.png", @"\Assets\Caches\Classes\Support.png");
        KitDict.Add("Assault.png", @"\Assets\Caches\Classes\Assault.png");
        KitDict.Add("Medic.png", @"\Assets\Caches\Classes\Medic.png");
        KitDict.Add("Cavalry.png", @"\Assets\Caches\Classes\Cavalry.png");
        KitDict.Add("Tanker.png", @"\Assets\Caches\Classes\Tanker.png");
        KitDict.Add("Pilot.png", @"\Assets\Caches\Classes\Pilot.png");

        Kit2Dict.Add("Scout.png", @"\Assets\Caches\Classes2\Scout.png");
        Kit2Dict.Add("Support.png", @"\Assets\Caches\Classes2\Support.png");
        Kit2Dict.Add("Assault.png", @"\Assets\Caches\Classes2\Assault.png");
        Kit2Dict.Add("Medic.png", @"\Assets\Caches\Classes2\Medic.png");
        Kit2Dict.Add("Cavalry.png", @"\Assets\Caches\Classes2\Cavalry.png");
        Kit2Dict.Add("Tanker.png", @"\Assets\Caches\Classes2\Tanker.png");
        Kit2Dict.Add("Pilot.png", @"\Assets\Caches\Classes2\Pilot.png");
    }
}

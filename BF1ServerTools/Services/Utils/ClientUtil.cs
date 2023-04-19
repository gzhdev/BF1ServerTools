namespace BF1ServerTools.Services;

public static class ClientUtil
{
    /// <summary>
    /// 获取地图对应中文名称
    /// </summary>
    /// <param name="englishName"></param>
    /// <returns></returns>
    public static string GetMapChsName(string englishName)
    {
        var result = MapDB.AllMapInfo.Find(var => var.English == englishName);
        if (result != null)
            return result.Chinese;

        return string.Empty;
    }

    /// <summary>
    /// 获取地图对应预览图
    /// </summary>
    /// <param name="englishName"></param>
    /// <returns></returns>
    public static string GetMapImage(string englishName)
    {
        var result = MapDB.AllMapInfo.Find(var => var.English == englishName);
        if (result != null)
            return result.GetMapImage();

        return string.Empty;
    }

    /// <summary>
    /// 获取武器简短名称，用于踢人理由
    /// </summary>
    /// <param name="englishName"></param>
    /// <returns></returns>
    public static string GetWeaponShortTxt(string englishName)
    {
        var result = WeaponDB.AllWeaponInfo.Find(var => var.English == englishName);
        if (result != null)
            return result.ShortName;

        return string.Empty;
    }

    /// <summary>
    /// 获取武器Guid
    /// </summary>
    /// <param name="englishName"></param>
    /// <returns></returns>
    public static string GetWeaponGuid(string englishName)
    {
        var result = WeaponDB.AllWeaponInfo.Find(var => var.English == englishName);
        if (result != null)
            return result.Guid;

        return string.Empty;
    }

    /// <summary>
    /// 获取武器对应中文名称
    /// </summary>
    /// <param name="englishName"></param>
    /// <returns></returns>
    public static string GetWeaponChsName(string englishName)
    {
        if (string.IsNullOrWhiteSpace(englishName))
            return string.Empty;

        if (englishName.Contains("_KBullet"))
            return "K 弹";

        if (englishName.Contains("_RGL_Frag"))
            return "步枪手榴弹（破片）";

        if (englishName.Contains("_RGL_Smoke"))
            return "步枪手榴弹（烟雾）";

        if (englishName.Contains("_RGL_HE"))
            return "步枪手榴弹（高爆）";

        var result = WeaponDB.AllWeaponInfo.Find(var => var.English == englishName);
        if (result != null)
            return result.Chinese;

        return string.Empty;
    }

    /// <summary>
    /// 获取武器对应本地图片路径
    /// </summary>
    /// <param name="englishName"></param>
    /// <returns></returns>
    public static string GetWeaponImagePath(string englishName)
    {
        if (string.IsNullOrWhiteSpace(englishName))
            return string.Empty;

        var tempName = string.Empty;

        if (englishName.Contains("_KBullet"))
            tempName = "_KBullet";
        else if (englishName.Contains("_RGL_Frag"))
            tempName = "_RGL_Frag";
        else if (englishName.Contains("_RGL_Smoke"))
            tempName = "_RGL_Smoke";
        else if (englishName.Contains("_RGL_HE"))
            tempName = "_RGL_HE";

        // 如果 tempName 不为空，则使用 tempName 匹配，否则继续使用 englishName 匹配
        var result = WeaponDB.AllWeaponInfo.Find(var => var.English == (tempName != string.Empty ? tempName : englishName));
        if (result != null)
            return result.GetWeaponImage();

        return string.Empty;
    }

    /// <summary>
    /// 获取武器图片路径
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetWebWeaponImage(string url)
    {
        var extension = Path.GetFileName(url);

        return $"\\Assets\\Caches\\Weapons\\{extension}";
    }

    /// <summary>
    /// 获取地图图片路径
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetWebMapImage(string url)
    {
        var extension = Path.GetFileName(url);

        return $"\\Assets\\Caches\\Maps\\{extension}";
    }

    /// <summary>
    /// 获取小队的中文名称
    /// </summary>
    /// <param name="squadId"></param>
    /// <returns></returns>
    public static string GetSquadNameById(int squadId)
    {
        return squadId switch
        {
            0 or 99 => "无",
            1 => "苹果",
            2 => "奶油",
            3 => "查理",
            4 => "达夫",
            5 => "爱德华",
            6 => "弗莱迪",
            7 => "乔治",
            8 => "哈利",
            9 => "墨水",
            10 => "强尼",
            11 => "国王",
            12 => "伦敦",
            13 => "猿猴",
            14 => "疯子",
            15 => "橘子",
            _ => squadId.ToString(),
        };
    }

    /// <summary>
    /// 获取队伍1阵营图片路径
    /// </summary>
    /// <param name="mapName"></param>
    public static string GetTeam1Image(string mapName)
    {
        var result = MapDB.AllMapInfo.Find(var => var.English == mapName);
        if (result != null)
            return result.GetTeam1Image();

        return string.Empty;
    }

    /// <summary>
    /// 获取队伍2阵营图片路径
    /// </summary>
    /// <param name="mapName"></param>
    public static string GetTeam2Image(string mapName)
    {
        var result = MapDB.AllMapInfo.Find(var => var.English == mapName);
        if (result != null)
            return result.GetTeam2Image();

        return string.Empty;
    }

    /// <summary>
    /// 获取队伍1阵营中文名称
    /// </summary>
    /// <param name="mapName"></param>
    /// <returns></returns>
    public static string GetTeam1ChsName(string mapName)
    {
        var result = MapDB.AllMapInfo.Find(var => var.English == mapName);
        if (result != null)
            return result.Team1;

        return string.Empty;
    }

    /// <summary>
    /// 获取队伍2阵营中文名称
    /// </summary>
    /// <param name="mapName"></param>
    /// <returns></returns>
    public static string GetTeam2ChsName(string mapName)
    {
        var result = MapDB.AllMapInfo.Find(var => var.English == mapName);
        if (result != null)
            return result.Team2;

        return string.Empty;
    }

    /// <summary>
    /// 获取当前地图游戏模式
    /// </summary>
    /// <param name="modeName"></param>
    /// <returns></returns>
    public static string GetGameMode(string modeName)
    {
        var result = ModeDB.AllModeInfo.Find(var => var.Mark == modeName);
        if (result != null)
            return result.Chinese;

        return string.Empty;
    }

    /// <summary>
    /// 获取兵种中文名称
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public static string GetClassChs(string className)
    {
        return className switch
        {
            "Medic" => "医疗兵",
            "Support" => "支援兵",
            "Scout" => "侦察兵",
            "Assault" => "突击兵",
            "Pilot" => "驾驶员",
            "Cavalry" => "骑兵",
            "Tanker" => "坦克",
            _ => className,
        };
    }

    /// <summary>
    /// 获取玩家当前兵种图片
    /// </summary>
    /// <param name="kit"></param>
    /// <returns></returns>
    public static string GetPlayerKitImage(string kit)
    {
        return kit switch
        {
            // 坦克
            "ID_M_TANKER" => "\\Assets\\Caches\\Kits\\KitIconTankerLarge.png",
            // 飞机
            "ID_M_PILOT" => "\\Assets\\Caches\\Kits\\KitIconPilotLarge.png",
            // 骑兵
            "ID_M_CAVALRY" => "\\Assets\\Caches\\Kits\\KitIconRiderLarge.png",
            // 哨兵
            "ID_M_SENTRY" => "\\Assets\\Caches\\Kits\\KitIconSentryLarge.png",
            // 喷火兵
            "ID_M_FLAMETHROWER" => "\\Assets\\Caches\\Kits\\KitIconFlamethrowerLarge.png",
            // 入侵者
            "ID_M_INFILTRATOR" => "\\Assets\\Caches\\Kits\\KitIconInfiltratorLarge.png",
            // 战壕奇兵
            "ID_M_TRENCHRAIDER" => "\\Assets\\Caches\\Kits\\KitIconTrenchRaiderLarge.png",
            // 坦克猎手
            "ID_M_ANTITANK" => "\\Assets\\Caches\\Kits\\KitIconAntiTankLarge.png",
            // 突击兵
            "ID_M_ASSAULT" => "\\Assets\\Caches\\Kits\\KitIconAssaultLarge.png",
            // 医疗兵
            "ID_M_MEDIC" => "\\Assets\\Caches\\Kits\\KitIconMedicLarge.png",
            // 支援兵
            "ID_M_SUPPORT" => "\\Assets\\Caches\\Kits\\KitIconSupportLarge.png",
            // 侦察兵
            "ID_M_SCOUT" => "\\Assets\\Caches\\Kits\\KitIconScoutLarge.png",
            _ => string.Empty,
        };
    }

    /// <summary>
    /// 获取玩家当前兵种名称
    /// </summary>
    /// <param name="kit"></param>
    /// <returns></returns>
    public static string GetPlayerKitName(string kit)
    {
        return kit switch
        {
            // 坦克
            "ID_M_TANKER" => "12 坦克",
            // 飞机
            "ID_M_PILOT" => "11 飞机",
            // 骑兵
            "ID_M_CAVALRY" => "10 骑兵",
            // 哨兵
            "ID_M_SENTRY" => "09 哨兵",
            // 喷火兵
            "ID_M_FLAMETHROWER" => "08 喷火兵",
            // 入侵者
            "ID_M_INFILTRATOR" => "07 入侵者",
            // 战壕奇兵
            "ID_M_TRENCHRAIDER" => "06 战壕奇兵",
            // 坦克猎手
            "ID_M_ANTITANK" => "05 坦克猎手",
            // 突击兵
            "ID_M_ASSAULT" => "04 突击兵",
            // 医疗兵
            "ID_M_MEDIC" => "03 医疗兵",
            // 支援兵
            "ID_M_SUPPORT" => "02 支援兵",
            // 侦察兵
            "ID_M_SCOUT" => "01 侦察兵",
            _ => string.Empty,
        };
    }
}

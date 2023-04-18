namespace BF1ServerTools.Services;

public static class ClientUtil
{
    /// <summary>
    /// 获取地图对应中文名称
    /// </summary>
    /// <param name="originMapName"></param>
    /// <returns></returns>
    public static string GetMapChsName(string originMapName)
    {
        var index = MapData.AllMapInfo.FindIndex(var => var.English == originMapName);
        if (index != -1)
            return MapData.AllMapInfo[index].Chinese;
        else
            return originMapName;
    }

    /// <summary>
    /// 获取地图对应预览图
    /// </summary>
    /// <param name="originMapName"></param>
    /// <returns></returns>
    public static string GetMapPreImage(string originMapName)
    {
        var index = MapData.AllMapInfo.FindIndex(var => var.English == originMapName);
        if (index != -1)
            return MapData.AllMapInfo[index].Image;
        else
            return string.Empty;
    }

    /// <summary>
    /// 获取武器对应中文名称
    /// </summary>
    /// <param name="originWeaponName"></param>
    /// <returns></returns>
    public static string GetWeaponChsName(string originWeaponName)
    {
        if (string.IsNullOrEmpty(originWeaponName))
            return string.Empty;

        if (originWeaponName.Contains("_KBullet"))
            return "K 弹";

        if (originWeaponName.Contains("_RGL_Frag"))
            return "步枪手榴弹（破片）";

        if (originWeaponName.Contains("_RGL_Smoke"))
            return "步枪手榴弹（烟雾）";

        if (originWeaponName.Contains("_RGL_HE"))
            return "步枪手榴弹（高爆）";

        int index = WeaponData.AllWeaponInfo.FindIndex(var => var.English == originWeaponName);
        if (index != -1)
            return WeaponData.AllWeaponInfo[index].Chinese;
        else
            return originWeaponName;
    }

    /// <summary>
    /// 获取武器对应本地图片路径
    /// </summary>
    /// <param name="english"></param>
    /// <returns></returns>
    public static string GetWeaponImagePath(string english)
    {
        if (string.IsNullOrEmpty(english))
            return string.Empty;

        var imageName = string.Empty;

        if (english.Contains("_KBullet"))
            imageName = "GadgetKBullets-0ec1f92a.png";

        if (english.Contains("_RGL_Frag"))
            imageName = "MedicRifleLauncher_B-a712e224.png";

        if (english.Contains("_RGL_Smoke"))
            imageName = "MedicRifleLauncher_A-438b725e.png";

        if (english.Contains("_RGL_HE"))
            imageName = "MedicRifleLauncher_B-a712e224.png";

        var index = WeaponData.AllWeaponInfo.FindIndex(var => var.English == english);
        if (index != -1)
            imageName = WeaponData.AllWeaponInfo[index].ImageName;
        else
            return string.Empty;

        return WeaponImg.Weapon2Dict.ContainsKey(imageName) ? WeaponImg.Weapon2Dict[imageName] : string.Empty;
    }

    /// <summary>
    /// 获取本地图片路径，如果未找到会返回空字符串
    /// </summary>
    /// <param name="url"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetTempImagePath(string url, string type)
    {
        var extension = Path.GetFileName(url);
        return type switch
        {
            "map" => MapImg.MapDict.ContainsKey(extension) ? MapImg.MapDict[extension] : string.Empty,
            "weapon" => WeaponImg.WeaponDict.ContainsKey(extension) ? WeaponImg.WeaponDict[extension] : string.Empty,
            "weapon2" => WeaponImg.Weapon2Dict.ContainsKey(extension) ? WeaponImg.Weapon2Dict[extension] : string.Empty,
            "kit" => KitImg.KitDict.ContainsKey(extension) ? KitImg.KitDict[extension] : string.Empty,
            "kit2" => KitImg.Kit2Dict.ContainsKey(extension) ? KitImg.Kit2Dict[extension] : string.Empty,
            _ => string.Empty,
        };
    }

    /// <summary>
    /// 获取武器简短名称，用于踢人理由
    /// </summary>
    /// <param name="weaponName"></param>
    /// <returns></returns>
    public static string GetWeaponShortTxt(string weaponName)
    {
        var index = WeaponData.AllWeaponInfo.FindIndex(var => var.English.Equals(weaponName));
        if (index != -1)
            return WeaponData.AllWeaponInfo[index].ShortName;

        return weaponName;
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
        var index = MapData.AllMapInfo.FindIndex(var => var.English.Equals(mapName));
        if (index != -1 && mapName != "ID_M_LEVEL_MENU")
            return $"\\Assets\\Caches\\Teams\\{MapData.AllMapInfo[index].Team1Image}.png";

        return "\\Assets\\Caches\\Teams\\_DEF.png";
    }

    /// <summary>
    /// 获取队伍2阵营图片路径
    /// </summary>
    /// <param name="mapName"></param>
    public static string GetTeam2Image(string mapName)
    {
        var index = MapData.AllMapInfo.FindIndex(var => var.English.Equals(mapName));
        if (index != -1 && mapName != "ID_M_LEVEL_MENU")
            return $"\\Assets\\Caches\\Teams\\{MapData.AllMapInfo[index].Team2Image}.png";

        return "\\Assets\\Caches\\Teams\\_DEF.png";
    }

    /// <summary>
    /// 获取队伍阵营中文名称
    /// </summary>
    /// <param name="mapName"></param>
    /// <param name="teamId"></param>
    /// <returns></returns>
    public static string GetTeamChsName(string mapName, int teamId)
    {
        var index = MapData.AllMapInfo.FindIndex(var => var.English.Equals(mapName));
        if (index != -1 && mapName != "ID_M_LEVEL_MENU")
        {
            if (teamId == 1)
                return MapData.AllMapInfo[index].Team1;

            return MapData.AllMapInfo[index].Team2;
        }

        return string.Empty;
    }

    /// <summary>
    /// 获取当前地图游戏模式
    /// </summary>
    /// <param name="modeName"></param>
    /// <returns></returns>
    public static string GetGameMode(string modeName)
    {
        var index = ModeData.AllModeInfo.FindIndex(var => var.Mark.Equals(modeName));
        if (index != -1)
            return ModeData.AllModeInfo[index].Chinese;
        else
            return string.Empty;
    }

    /// <summary>
    /// 获取兵种中文名称
    /// </summary>
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
            "ID_M_TANKER" => "\\Assets\\Caches\\Kits2\\KitIconTankerLarge.png",
            // 飞机
            "ID_M_PILOT" => "\\Assets\\Caches\\Kits2\\KitIconPilotLarge.png",
            // 骑兵
            "ID_M_CAVALRY" => "\\Assets\\Caches\\Kits2\\KitIconRiderLarge.png",
            // 哨兵
            "ID_M_SENTRY" => "\\Assets\\Caches\\Kits2\\KitIconSentryLarge.png",
            // 喷火兵
            "ID_M_FLAMETHROWER" => "\\Assets\\Caches\\Kits2\\KitIconFlamethrowerLarge.png",
            // 入侵者
            "ID_M_INFILTRATOR" => "\\Assets\\Caches\\Kits2\\KitIconInfiltratorLarge.png",
            // 战壕奇兵
            "ID_M_TRENCHRAIDER" => "\\Assets\\Caches\\Kits2\\KitIconTrenchRaiderLarge.png",
            // 坦克猎手
            "ID_M_ANTITANK" => "\\Assets\\Caches\\Kits2\\KitIconAntiTankLarge.png",
            // 突击兵
            "ID_M_ASSAULT" => "\\Assets\\Caches\\Kits2\\KitIconAssaultLarge.png",
            // 医疗兵
            "ID_M_MEDIC" => "\\Assets\\Caches\\Kits2\\KitIconMedicLarge.png",
            // 支援兵
            "ID_M_SUPPORT" => "\\Assets\\Caches\\Kits2\\KitIconSupportLarge.png",
            // 侦察兵
            "ID_M_SCOUT" => "\\Assets\\Caches\\Kits2\\KitIconScoutLarge.png",
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

    /// <summary>
    /// 获取玩家当前兵种图片
    /// </summary>
    /// <param name="weaponS0">主要武器</param>
    /// <param name="weaponS2">配备一</param>
    /// <param name="weaponS5">配备二</param>
    /// <returns></returns>
    public static string GetPlayerKitImage(string weaponS0, string weaponS2, string weaponS5)
    {
        switch (weaponS0)
        {
            // 哨兵
            case "U_MaximMG0815":
            case "U_VillarPerosa":
                return "\\Assets\\Caches\\Kits2\\KitIconSentryLarge.png";
            // 喷火兵
            case "U_FlameThrower":
                return "\\Assets\\Caches\\Kits2\\KitIconFlamethrowerLarge.png";
            // 战壕奇兵
            case "U_RoyalClub":
                return "\\Assets\\Caches\\Kits2\\KitIconTrenchRaiderLarge.png";
            // 入侵者
            case "U_MartiniGrenadeLauncher":
                return "\\Assets\\Caches\\Kits2\\KitIconInfiltratorLarge.png";
            // 坦克猎手
            case "U_TankGewehr":
                return "\\Assets\\Caches\\Kits2\\KitIconAntiTankLarge.png";
            // 骑兵
            case "ID_P_VNAME_HORSE":
            case "U_WinchesterM1895_Horse":
                return "\\Assets\\Caches\\Kits2\\KitIconRiderLarge.png";
            // 机械巨兽 飞船 l30
            case "ID_P_VNAME_ZEPPELIN":
                return "\\Assets\\Caches\\Kits2\\ZEPPELIN.png";
            // 机械巨兽 装甲列车
            case "ID_P_VNAME_ARMOREDTRAIN":
                return "\\Assets\\Caches\\Kits2\\ARMOREDTRAIN.png";
            // 机械巨兽 无畏舰
            case "ID_P_VNAME_IRONDUKE":
                return "\\Assets\\Caches\\Kits2\\IRONDUKE.png";
            // 机械巨兽 Char 2C
            case "ID_P_VNAME_CHAR":
                return "\\Assets\\Caches\\Kits2\\CHAR.png";
        }

        // 坦克
        if (KitData.TankeKit.Contains(weaponS0) || (weaponS2 == "U_Wrench" && weaponS5 == "U_ATGrenade"))
        {
            return "\\Assets\\Caches\\Kits2\\KitIconTankerLarge.png";
        }

        // 飞机
        if (KitData.PilotKit.Contains(weaponS0) || (weaponS2 == "U_Wrench" && weaponS5 == "U_FlareGun"))
        {
            return "\\Assets\\Caches\\Kits2\\KitIconPilotLarge.png";
        }

        // 定点武器
        if (KitData.DingDianKit.Contains(weaponS0))
        {
            return "\\Assets\\Caches\\Kits2\\DINGDIAN.png";
        }

        // 运输载具
        if (KitData.YunShuKit.Contains(weaponS0))
        {
            return "\\Assets\\Caches\\Kits2\\YUNSHU.png";
        }

        // 突击兵
        if (KitData.AssaultKit.Contains(weaponS0) && (KitData.AssaultKit.Contains(weaponS2) || KitData.AssaultKit.Contains(weaponS5)))
        {
            return "\\Assets\\Caches\\Kits2\\KitIconAssaultLarge.png";
        }

        // 医疗兵
        if (KitData.MedicKit.Contains(weaponS0) && (KitData.MedicKit.Contains(weaponS2) || KitData.MedicKit.Contains(weaponS5)))
        {
            return "\\Assets\\Caches\\Kits2\\KitIconMedicLarge.png";
        }

        // 支援兵
        if (KitData.SupportKit.Contains(weaponS0) && (KitData.SupportKit.Contains(weaponS2) || KitData.SupportKit.Contains(weaponS5)))
        {
            return "\\Assets\\Caches\\Kits2\\KitIconSupportLarge.png";
        }

        // 侦察兵
        if (KitData.ScoutKit.Contains(weaponS0) && (KitData.ScoutKit.Contains(weaponS2) || KitData.ScoutKit.Contains(weaponS5)))
        {
            return "\\Assets\\Caches\\Kits2\\KitIconScoutLarge.png";
        }

        return string.Empty;
    }

    /// <summary>
    /// 获取玩家当前兵种名称
    /// </summary>
    /// <param name="weaponS0">主要武器</param>
    /// <param name="weaponS2">配备一</param>
    /// <param name="weaponS5">配备二</param>
    /// <returns></returns>
    public static string GetPlayerKitName(string weaponS0, string weaponS2, string weaponS5)
    {
        switch (weaponS0)
        {
            // 哨兵
            case "U_MaximMG0815":
            case "U_VillarPerosa":
            // 喷火兵
            case "U_FlameThrower":
            // 战壕奇兵
            case "U_RoyalClub":
            // 入侵者
            case "U_MartiniGrenadeLauncher":
            // 坦克猎手
            case "U_TankGewehr":
                return "15 精英兵";
            // 骑兵
            case "ID_P_VNAME_HORSE":
            case "U_WinchesterM1895_Horse":
                return "16 骑兵";
            // 机械巨兽 飞船 l30
            case "ID_P_VNAME_ZEPPELIN":
            // 机械巨兽 装甲列车
            case "ID_P_VNAME_ARMOREDTRAIN":
            // 机械巨兽 无畏舰
            case "ID_P_VNAME_IRONDUKE":
            // 机械巨兽 Char 2C
            case "ID_P_VNAME_CHAR":
                return "19 机械巨兽";
        }

        // 坦克
        if (KitData.TankeKit.Contains(weaponS0) || (weaponS2 == "U_Wrench" && weaponS5 == "U_ATGrenade"))
        {
            return "18 坦克";
        }

        // 飞机
        if (KitData.PilotKit.Contains(weaponS0) || (weaponS2 == "U_Wrench" && weaponS5 == "U_FlareGun"))
        {
            return "17 飞机";
        }

        // 定点武器
        if (KitData.DingDianKit.Contains(weaponS0))
        {
            return "13 定点武器";
        }

        // 运输载具
        if (KitData.YunShuKit.Contains(weaponS0))
        {
            return "14 运输载具";
        }

        // 突击兵
        if (KitData.AssaultKit.Contains(weaponS0) && (KitData.AssaultKit.Contains(weaponS2) || KitData.AssaultKit.Contains(weaponS5)))
        {
            return "09 突击兵";
        }

        // 医疗兵
        if (KitData.MedicKit.Contains(weaponS0) && (KitData.MedicKit.Contains(weaponS2) || KitData.MedicKit.Contains(weaponS5)))
        {
            return "08 医疗兵";
        }

        // 支援兵
        if (KitData.SupportKit.Contains(weaponS0) && (KitData.SupportKit.Contains(weaponS2) || KitData.SupportKit.Contains(weaponS5)))
        {
            return "07 支援兵";
        }

        // 侦察兵
        if (KitData.ScoutKit.Contains(weaponS0) && (KitData.ScoutKit.Contains(weaponS2) || KitData.ScoutKit.Contains(weaponS5)))
        {
            return "06 侦察兵";
        }

        return string.Empty;
    }
}

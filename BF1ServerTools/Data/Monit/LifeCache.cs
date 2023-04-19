namespace BF1ServerTools.Data;

public class LifeCache
{
    public string Name { get; set; }
    public long PersonaId { get; set; }

    public float KD { get; set; }
    public float KPM { get; set; }
    public int Time { get; set; }

    public BaseStat BaseStats { get; set; }
    public List<WeaponStat> WeaponStats { get; set; }
    public List<VehicleStat> VehicleStats { get; set; }

    public DateTime CreateTime { get; set; }
}

public class BaseStat
{
    public int timePlayed { get; set; }
    /// <summary>
    /// 胜利场数
    /// </summary>
    public int wins { get; set; }
    /// <summary>
    /// 战败场数
    /// </summary>
    public int losses { get; set; }
    /// <summary>
    /// 击杀
    /// </summary>
    public int kills { get; set; }
    /// <summary>
    /// 死亡
    /// </summary>
    public int deaths { get; set; }
    public float kpm { get; set; }
    public float spm { get; set; }
    /// <summary>
    /// 技巧值
    /// </summary>
    public float skill { get; set; }

    /// <summary>
    /// 最佳兵种
    /// </summary>
    public string favoriteClass { get; set; }
    /// <summary>
    /// 奖励分数
    /// </summary>
    public float awardScore { get; set; }
    /// <summary>
    /// 加成分数
    /// </summary>
    public float bonusScore { get; set; }
    /// <summary>
    /// 小隊分数
    /// </summary>
    public float squadScore { get; set; }
    /// <summary>
    /// 仇敌击杀数
    /// </summary>
    public int avengerKills { get; set; }
    /// <summary>
    /// 救星击杀数
    /// </summary>
    public int saviorKills { get; set; }
    /// <summary>
    /// 最高连续击杀数
    /// </summary>
    public int highestKillStreak { get; set; }
    /// <summary>
    /// 取得狗牌数
    /// </summary>
    public int dogtagsTaken { get; set; }
    /// <summary>
    /// 游戏总场数
    /// </summary>
    public int roundsPlayed { get; set; }
    public int flagsCaptured { get; set; }
    public int flagsDefended { get; set; }
    /// <summary>
    /// 命中率
    /// </summary>
    public float accuracyRatio { get; set; }
    /// <summary>
    /// 爆头数
    /// </summary>
    public int headShots { get; set; }
    /// <summary>
    /// 最远爆头距离
    /// </summary>
    public float longestHeadShot { get; set; }
    public float nemesisKills { get; set; }
    public float nemesisKillStreak { get; set; }
    /// <summary>
    /// 急救数
    /// </summary>
    public float revives { get; set; }
    /// <summary>
    /// 治疗分
    /// </summary>
    public float heals { get; set; }
    /// <summary>
    /// 修理分
    /// </summary>
    public float repairs { get; set; }
    public float suppressionAssist { get; set; }
    public float kdr { get; set; }
    /// <summary>
    /// 协助击杀数
    /// </summary>
    public float killAssists { get; set; }

    ///////////////////////////////////////

    public float kd { get; set; }
    /// <summary>
    /// 胜率
    /// </summary>
    public float winPercent { get; set; }
}

public class WeaponStat
{
    public string name { get; set; }
    public string imageUrl { get; set; }

    public float hits { get; set; }
    public float shots { get; set; }
    public float kills { get; set; }
    public float headshots { get; set; }
    public float accuracy { get; set; }
    public float seconds { get; set; }

    ///////////////////////////////////////

    public float kpm { get; set; }
    /// <summary>
    /// 爆头率
    /// </summary>
    public float headshotsVKills { get; set; }
    /// <summary>
    /// 命中率
    /// </summary>
    public float hitsVShots { get; set; }
    /// <summary>
    /// 效率
    /// </summary>
    public float hitVKills { get; set; }

    public int star { get; set; }
    public string time { get; set; }
}

public class VehicleStat
{
    public string name { get; set; }
    public string imageUrl { get; set; }

    public float seconds { get; set; }
    public float kills { get; set; }
    public float destroyed { get; set; }

    ///////////////////////////////////////

    public float kpm { get; set; }

    public int star { get; set; }
    public string time { get; set; }
}

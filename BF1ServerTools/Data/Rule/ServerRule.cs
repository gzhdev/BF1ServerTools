namespace BF1ServerTools.Data;

public class ServerRule
{
    public int MaxKill { get; set; }

    public int FlagKD { get; set; }
    public float MaxKD { get; set; }

    public int FlagKPM { get; set; }
    public float MaxKPM { get; set; }

    public int MinRank { get; set; }
    public int MaxRank { get; set; }

    public float LifeMaxKD { get; set; }
    public float LifeMaxKPM { get; set; }
    public int LifeMaxWeaponStar { get; set; }
    public int LifeMaxVehicleStar { get; set; }
}

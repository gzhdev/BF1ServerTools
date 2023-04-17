namespace BF1ServerTools.Data;

public class PlayerData
{
    public bool IsAdmin { get; set; }
    public bool IsVIP { get; set; }
    public bool IsWhite { get; set; }

    public byte Mark { get; set; }
    public int TeamId { get; set; }
    public byte Spectator { get; set; }
    public string Clan { get; set; }
    public string Name { get; set; }
    public long PersonaId { get; set; }

    public int SquadId { get; set; }
    public string SquadName { get; set; }

    public int Rank { get; set; }
    public int Kill { get; set; }
    public int Dead { get; set; }
    public int Score { get; set; }

    public float KD { get; set; }
    public float KPM { get; set; }

    public float LifeKD { get; set; }
    public float LifeKPM { get; set; }
    public int LifeTime { get; set; }

    public string Kit { get; set; }
    public string KitImg { get; set; }
    public string KitName { get; set; }

    public string WeaponS0 { get; set; }
    public string WeaponS1 { get; set; }
    public string WeaponS2 { get; set; }
    public string WeaponS3 { get; set; }
    public string WeaponS4 { get; set; }
    public string WeaponS5 { get; set; }
    public string WeaponS6 { get; set; }
    public string WeaponS7 { get; set; }
}

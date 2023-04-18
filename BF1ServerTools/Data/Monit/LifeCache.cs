namespace BF1ServerTools.Data;

public class LifeCache
{
    public string Name { get; set; }
    public long PersonaId { get; set; }

    public string DetailedStatsJson { get; set; }
    public string GetWeaponsJson { get; set; }
    public string GetVehiclesJson { get; set; }

    public DateTime CreateTime { get; set; }
}

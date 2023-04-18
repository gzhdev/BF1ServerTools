using FreeSql.DataAnnotations;

namespace BF1ServerTools.SQLite;

public class LifeCacheDb
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public int Id { get; set; }

    public string Name { get; set; }
    public long PersonaId { get; set; }

    public string DetailedStatsJson { get; set; }
    public string GetWeaponsJson { get; set; }
    public string GetVehiclesJson { get; set; }

    public DateTime CreateTime { get; set; }
}

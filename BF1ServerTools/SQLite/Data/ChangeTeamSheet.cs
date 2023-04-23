using FreeSql.DataAnnotations;

namespace BF1ServerTools.SQLite;

public class ChangeTeamSheet
{
    [Column(IsPrimary = true, IsIdentity = true)]
    public int Id { get; set; }

    public string ServerName { get; set; }
    public long GameId { get; set; }
    public string GameTime { get; set; }

    public int Rank { get; set; }
    public string Name { get; set; }
    public long PersonaId { get; set; }

    public string GameMode { get; set; }
    public string MapName { get; set; }

    public string Team1Name { get; set; }
    public string Team2Name { get; set; }
    public string TeamScore { get; set; }

    public string State { get; set; }

    [Column(ServerTime = DateTimeKind.Local, CanUpdate = false, IsNullable = false)]
    public DateTime CreateTime { get; set; }
}

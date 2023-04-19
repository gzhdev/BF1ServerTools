using FreeSql.DataAnnotations;

namespace BF1ServerTools.SQLite;

public class LifeCacheSheet
{
    [Column(IsPrimary = true)]
    public Guid Id { get; set; }

    [Column(DbType = "varchar(64)", IsNullable = false)]
    public string Name { get; set; }
    [Column(IsNullable = false)]
    public long PersonaId { get; set; }

    [Column(DbType = "text")]
    public string LifeCacheJson { get; set; }

    [Column(ServerTime = DateTimeKind.Local, CanUpdate = false, IsNullable = false)]
    public DateTime CreateTime { get; set; }
}

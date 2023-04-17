namespace BF1ServerTools.Data;

public class ServerData
{
    public string Name { get; set; }
    public long GameId { get; set; }
    public float Time { get; set; }
    public float TimeMM { get; set; }
    public string GameTime { get; set; }

    public string GameMode { get; set; }
    public string MapName { get; set; }
    public string MapImg { get; set; }

    public int AllPlayerCount { get; set; }
}

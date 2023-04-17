namespace BF1ServerTools.Data;

public class TeamData
{
    public int MaxScore { get; set; }

    public int AllScore { get; set; }
    public int ScoreKill { get; set; }
    public int ScoreFlag { get; set; }

    public string TeamImg { get; set; }
    public string TeamName { get; set; }

    public int AssaultKitCount { get; set; }
    public int MedicKitCount { get; set; }
    public int SupportKitCount { get; set; }
    public int ScoutKitCount { get; set; }

    public int PlayerCount { get; set; }
    public int MaxPlayerCount { get; set; }
    public int Rank150PlayerCount { get; set; }
    public int AllKillCount { get; set; }
    public int AllDeadCount { get; set; }

    public void Reset()
    {
        MaxScore = 0;

        AllScore = 0;
        ScoreKill = 0;
        ScoreFlag = 0;

        TeamImg = string.Empty;
        TeamName = string.Empty;

        AssaultKitCount = 0;
        MedicKitCount = 0;
        SupportKitCount = 0;
        ScoutKitCount = 0;

        PlayerCount = 0;
        MaxPlayerCount = 0;
        Rank150PlayerCount = 0;
        AllKillCount = 0;
        AllDeadCount = 0;
    }
}

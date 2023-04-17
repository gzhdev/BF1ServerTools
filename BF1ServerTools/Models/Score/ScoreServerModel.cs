using CommunityToolkit.Mvvm.ComponentModel;

namespace BF1ServerTools.Models;

public class ScoreServerModel : ObservableObject
{
    #region 服务器名称 Name
    private string name;
    /// <summary>
    /// 服务器名称
    /// </summary>
    public string Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }
    #endregion

    #region 服务器GameId GameId
    private long gameId;
    /// <summary>
    /// 服务器GameId
    /// </summary>
    public long GameId
    {
        get => gameId;
        set => SetProperty(ref gameId, value);
    }
    #endregion

    #region 服务器地图模式 GameMode
    private string gameMode;
    /// <summary>
    /// 服务器地图模式
    /// </summary>
    public string GameMode
    {
        get => gameMode;
        set => SetProperty(ref gameMode, value);
    }
    #endregion

    #region 服务器地图名称 MapName
    private string mapName;
    /// <summary>
    /// 服务器地图名称
    /// </summary>
    public string MapName
    {
        get => mapName;
        set => SetProperty(ref mapName, value);
    }
    #endregion

    #region 服务器地图预览图 MapImg
    private string mapImg;
    /// <summary>
    /// 服务器地图预览图
    /// </summary>
    public string MapImg
    {
        get => mapImg;
        set => SetProperty(ref mapImg, value);
    }
    #endregion

    #region 服务器游戏时间 GameTime
    private string gameTime;
    /// <summary>
    /// 服务器游戏时间
    /// </summary>
    public string GameTime
    {
        get => gameTime;
        set => SetProperty(ref gameTime, value);
    }
    #endregion

    ///////////////////////////////////

    #region 服务器总人数 AllPlayerCount
    private int allPlayerCount;
    /// <summary>
    /// 服务器总人数
    /// </summary>
    public int AllPlayerCount
    {
        get => allPlayerCount;
        set => SetProperty(ref allPlayerCount, value);
    }
    #endregion
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace BF1ServerTools.Models;

public class DetailModel : ObservableObject
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

    #region 服务器描述 Description
    private string description;
    /// <summary>
    /// 服务器描述
    /// </summary>
    public string Description
    {
        get => description;
        set => SetProperty(ref description, value);
    }
    #endregion

    #region 服务器游戏Id GameId
    private string gameId;
    /// <summary>
    /// 服务器游戏Id
    /// </summary>
    public string GameId
    {
        get => gameId;
        set => SetProperty(ref gameId, value);
    }
    #endregion

    #region 服务器Guid Guid
    private string guid;
    /// <summary>
    /// 服务器Guid
    /// </summary>
    public string Guid
    {
        get => guid;
        set => SetProperty(ref guid, value);
    }
    #endregion

    #region 服务器Id ServerId
    private string serverId;
    /// <summary>
    /// 服务器ServerId
    /// </summary>
    public string ServerId
    {
        get => serverId;
        set => SetProperty(ref serverId, value);
    }
    #endregion

    #region 服务器收藏数 Bookmark
    private string bookmark;
    /// <summary>
    /// 服务器收藏数
    /// </summary>
    public string Bookmark
    {
        get => bookmark;
        set => SetProperty(ref bookmark, value);
    }
    #endregion

    #region 服主Id OwnerName
    private string ownerName;
    /// <summary>
    /// 服主Id
    /// </summary>
    public string OwnerName
    {
        get => ownerName;
        set => SetProperty(ref ownerName, value);
    }
    #endregion

    #region 服主数字Id OwnerPersonaId
    private string ownerPersonaId;
    /// <summary>
    /// 服主数字Id
    /// </summary>
    public string OwnerPersonaId
    {
        get => ownerPersonaId;
        set => SetProperty(ref ownerPersonaId, value);
    }
    #endregion

    #region 服主头像 OwnerImage
    private string ownerImage;
    /// <summary>
    /// 服主头像
    /// </summary>
    public string OwnerImage
    {
        get => ownerImage;
        set => SetProperty(ref ownerImage, value);
    }
    #endregion
}

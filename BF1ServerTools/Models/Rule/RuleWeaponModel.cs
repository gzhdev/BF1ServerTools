using CommunityToolkit.Mvvm.ComponentModel;

namespace BF1ServerTools.Models;

public class RuleWeaponModel : ObservableObject
{
    #region 种类 Kind
    private string kind;
    /// <summary>
    /// 种类
    /// </summary>
    public string Kind
    {
        get => kind;
        set => SetProperty(ref kind, value);
    }
    #endregion

    #region 中文名称 Name
    private string name;
    /// <summary>
    /// 中文名称
    /// </summary>
    public string Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }
    #endregion

    #region 英文名称 English
    private string english;
    /// <summary>
    /// 英文名称
    /// </summary>
    public string English
    {
        get => english;
        set => SetProperty(ref english, value);
    }
    #endregion

    #region 预览图片 Image
    private string image;
    /// <summary>
    /// 预览图片
    /// </summary>
    public string Image
    {
        get => image;
        set => SetProperty(ref image, value);
    }
    #endregion

    #region 队伍1限制 Team1
    private bool team1;
    /// <summary>
    /// 队伍1限制
    /// </summary>
    public bool Team1
    {
        get => team1;
        set => SetProperty(ref team1, value);
    }
    #endregion

    #region 队伍2限制 Team2
    private bool team2;
    /// <summary>
    /// 队伍2限制
    /// </summary>
    public bool Team2
    {
        get => team2;
        set => SetProperty(ref team2, value);
    }
    #endregion
}

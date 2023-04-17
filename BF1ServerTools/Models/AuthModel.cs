using CommunityToolkit.Mvvm.ComponentModel;

namespace BF1ServerTools.Models;

public class AuthModel : ObservableObject
{
    #region 头像 Avatar2
    private string avatar2;
    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar2
    {
        get => avatar2;
        set => SetProperty(ref avatar2, value);
    }
    #endregion

    #region 显示名称 DisplayName2
    private string displayName2;
    /// <summary>
    /// 显示名称
    /// </summary>
    public string DisplayName2
    {
        get => displayName2;
        set => SetProperty(ref displayName2, value);
    }
    #endregion

    #region 数字Id PersonaId2
    private long personaId2;
    /// <summary>
    /// 数字Id
    /// </summary>
    public long PersonaId2
    {
        get => personaId2;
        set => SetProperty(ref personaId2, value);
    }
    #endregion

    ////////////////////////////////////

    #region Remid Remid
    private string remid;
    /// <summary>
    /// Remid
    /// </summary>
    public string Remid
    {
        get => remid;
        set => SetProperty(ref remid, value);
    }
    #endregion

    #region Sid Sid
    private string sid;
    /// <summary>
    /// Sid
    /// </summary>
    public string Sid
    {
        get => sid;
        set => SetProperty(ref sid, value);
    }
    #endregion

    #region AccessToken AccessToken
    private string accessToken;
    /// <summary>
    /// AccessToken
    /// </summary>
    public string AccessToken
    {
        get => accessToken;
        set => SetProperty(ref accessToken, value);
    }
    #endregion

    #region SessionId2 SessionId2
    private string sessionId2;
    /// <summary>
    /// SessionId2
    /// </summary>
    public string SessionId2
    {
        get => sessionId2;
        set => SetProperty(ref sessionId2, value);
    }
    #endregion
}

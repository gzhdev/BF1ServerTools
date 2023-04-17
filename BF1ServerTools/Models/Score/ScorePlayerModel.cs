using CommunityToolkit.Mvvm.ComponentModel;

namespace BF1ServerTools.Models;

public class ScorePlayerModel : ObservableObject, IComparable<ScorePlayerModel>
{
    #region 序号 Index
    private int index;
    /// <summary>
    /// 序号
    /// </summary>
    public int Index
    {
        get => index;
        set => SetProperty(ref index, value);
    }
    #endregion

    ///////////////////////////////////

    #region 玩家战队 Clan
    private string clan;
    /// <summary>
    /// 玩家战队
    /// </summary>
    public string Clan
    {
        get => clan;
        set => SetProperty(ref clan, value);
    }
    #endregion

    #region 玩家昵称 Name
    private string name;
    /// <summary>
    /// 玩家昵称
    /// </summary>
    public string Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }
    #endregion

    #region 玩家数字Id PersonaId
    private long personaId;
    /// <summary>
    /// 玩家数字Id
    /// </summary>
    public long PersonaId
    {
        get => personaId;
        set => SetProperty(ref personaId, value);
    }
    #endregion

    #region 玩家小队Id SquadId
    private int squadId;
    /// <summary>
    /// 玩家小队Id
    /// </summary>
    public int SquadId
    {
        get => squadId;
        set => SetProperty(ref squadId, value);
    }
    #endregion

    #region 玩家小队名称 SquadName
    private string squadName;
    /// <summary>
    /// 玩家小队名称
    /// </summary>
    public string SquadName
    {
        get => squadName;
        set => SetProperty(ref squadName, value);
    }
    #endregion

    ///////////////////////////////////

    #region 是否为管理员 IsAdmin
    private bool isAdmin;
    /// <summary>
    /// 是否为管理员
    /// </summary>
    public bool IsAdmin
    {
        get => isAdmin;
        set => SetProperty(ref isAdmin, value);
    }
    #endregion

    #region 是否为VIP IsVIP
    private bool isVIP;
    /// <summary>
    /// 是否为VIP
    /// </summary>
    public bool IsVIP
    {
        get => isVIP;
        set => SetProperty(ref isVIP, value);
    }
    #endregion

    #region 是否为白名单 IsWhite
    private bool isWhite;
    /// <summary>
    /// 是否为白名单
    /// </summary>
    public bool IsWhite
    {
        get => isWhite;
        set => SetProperty(ref isWhite, value);
    }
    #endregion

    ///////////////////////////////////

    #region 玩家等级 Rank
    private int rank;
    /// <summary>
    /// 玩家等级
    /// </summary>
    public int Rank
    {
        get => rank;
        set => SetProperty(ref rank, value);
    }
    #endregion

    #region 击杀数 Kill
    private int kill;
    /// <summary>
    /// 击杀数
    /// </summary>
    public int Kill
    {
        get => kill;
        set => SetProperty(ref kill, value);
    }
    #endregion

    #region 死亡数 Dead
    private int dead;
    /// <summary>
    /// 死亡数
    /// </summary>
    public int Dead
    {
        get => dead;
        set => SetProperty(ref dead, value);
    }
    #endregion

    #region 当局得分 Score
    private int score;
    /// <summary>
    /// 当局得分
    /// </summary>
    public int Score
    {
        get => score;
        set => SetProperty(ref score, value);
    }
    #endregion

    #region 当局KD KD
    private float kd;
    /// <summary>
    /// 当局KD
    /// </summary>
    public float KD
    {
        get => kd;
        set => SetProperty(ref kd, value);
    }
    #endregion

    #region 当局KPM KPM
    private float kpm;
    /// <summary>
    /// 当局KPM
    /// </summary>
    public float KPM
    {
        get => kpm;
        set => SetProperty(ref kpm, value);
    }
    #endregion

    #region 生涯KD LifeKD
    private float lifeKD;
    /// <summary>
    /// 生涯KD
    /// </summary>
    public float LifeKD
    {
        get => lifeKD;
        set => SetProperty(ref lifeKD, value);
    }
    #endregion

    #region 生涯KPM LifeKPM
    private float lifeKPM;
    /// <summary>
    /// 生涯KPM
    /// </summary>
    public float LifeKPM
    {
        get => lifeKPM;
        set => SetProperty(ref lifeKPM, value);
    }
    #endregion

    #region 生涯时长 LifeTime
    private int lifeTime;
    /// <summary>
    /// 生涯时长
    /// </summary>
    public int LifeTime
    {
        get => lifeTime;
        set => SetProperty(ref lifeTime, value);
    }
    #endregion

    ///////////////////////////////////

    #region 兵种 Kit
    private string kit;
    /// <summary>
    /// 兵种
    /// </summary>
    public string Kit
    {
        get => kit;
        set => SetProperty(ref kit, value);
    }
    #endregion

    #region 兵种预览图 KitImg
    private string kitImg;
    /// <summary>
    /// 兵种预览图
    /// </summary>
    public string KitImg
    {
        get => kitImg;
        set => SetProperty(ref kitImg, value);
    }
    #endregion

    #region 兵种名称 KitName
    private string kitName;
    /// <summary>
    /// 兵种名称
    /// </summary>
    public string KitName
    {
        get => kitName;
        set => SetProperty(ref kitName, value);
    }
    #endregion

    #region 武器槽0 WeaponS0
    private string weaponS0;
    /// <summary>
    /// 武器槽0
    /// </summary>
    public string WeaponS0
    {
        get => weaponS0;
        set => SetProperty(ref weaponS0, value);
    }
    #endregion

    #region 武器槽1 WeaponS1
    private string weaponS1;
    /// <summary>
    /// 武器槽1
    /// </summary>
    public string WeaponS1
    {
        get => weaponS1;
        set => SetProperty(ref weaponS1, value);
    }
    #endregion

    #region 武器槽2 WeaponS2
    private string weaponS2;
    /// <summary>
    /// 武器槽2
    /// </summary>
    public string WeaponS2
    {
        get => weaponS2;
        set => SetProperty(ref weaponS2, value);
    }
    #endregion

    #region 武器槽3 WeaponS3
    private string weaponS3;
    /// <summary>
    /// 武器槽3
    /// </summary>
    public string WeaponS3
    {
        get => weaponS3;
        set => SetProperty(ref weaponS3, value);
    }
    #endregion

    #region 武器槽4 WeaponS4
    private string weaponS4;
    /// <summary>
    /// 武器槽4
    /// </summary>
    public string WeaponS4
    {
        get => weaponS4;
        set => SetProperty(ref weaponS4, value);
    }
    #endregion

    #region 武器槽5 WeaponS5
    private string weaponS5;
    /// <summary>
    /// 武器槽5
    /// </summary>
    public string WeaponS5
    {
        get => weaponS5;
        set => SetProperty(ref weaponS5, value);
    }
    #endregion

    #region 武器槽6 WeaponS6
    private string weaponS6;
    /// <summary>
    /// 武器槽6
    /// </summary>
    public string WeaponS6
    {
        get => weaponS6;
        set => SetProperty(ref weaponS6, value);
    }
    #endregion

    #region 武器槽7 WeaponS7
    private string weaponS7;
    /// <summary>
    /// 武器槽7
    /// </summary>
    public string WeaponS7
    {
        get => weaponS7;
        set => SetProperty(ref weaponS7, value);
    }
    #endregion

    ///////////////////////////////////

    /// <summary>
    /// 自定义排序
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(ScorePlayerModel other)
    {
        return Globals.OrderBy switch
        {
            OrderBy.Score => other.score.CompareTo(this.score),
            OrderBy.Rank => other.rank.CompareTo(this.rank),
            OrderBy.Clan => other.clan.CompareTo(this.clan),
            OrderBy.Name => this.name.CompareTo(other.name),
            OrderBy.SquadId => this.squadId.CompareTo(other.squadId),
            OrderBy.Kill => other.kill.CompareTo(this.kill),
            OrderBy.Dead => other.dead.CompareTo(this.dead),
            OrderBy.KD => other.kd.CompareTo(this.kd),
            OrderBy.KPM => other.kpm.CompareTo(this.kpm),
            OrderBy.LKD => other.lifeKD.CompareTo(this.lifeKD),
            OrderBy.LKPM => other.lifeKPM.CompareTo(this.lifeKPM),
            OrderBy.LTime => other.lifeTime.CompareTo(this.lifeTime),
            OrderBy.Kit3 => other.kitName.CompareTo(this.kitName),
            OrderBy.Weapon => other.weaponS0.CompareTo(this.weaponS0),
            _ => other.score.CompareTo(this.score),
        };
    }
}

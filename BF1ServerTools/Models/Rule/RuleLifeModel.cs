using CommunityToolkit.Mvvm.ComponentModel;

namespace BF1ServerTools.Models;

public class RuleLifeModel : ObservableObject
{
    #region 最大生涯KD LifeMaxKD
    private float lifeMaxKD;
    /// <summary>
    /// 最大生涯KD
    /// </summary>
    public float LifeMaxKD
    {
        get => lifeMaxKD;
        set => SetProperty(ref lifeMaxKD, value);
    }
    #endregion

    #region 最大生涯KPM LifeMaxKPM
    private float lifeMaxKPM;
    /// <summary>
    /// 最大生涯KPM
    /// </summary>
    public float LifeMaxKPM
    {
        get => lifeMaxKPM;
        set => SetProperty(ref lifeMaxKPM, value);
    }
    #endregion

    #region 最大生涯武器星数 LifeMaxWeaponStar
    private int lifeMaxWeaponStar;
    /// <summary>
    /// 最大生涯武器星数
    /// </summary>
    public int LifeMaxWeaponStar
    {
        get => lifeMaxWeaponStar;
        set => SetProperty(ref lifeMaxWeaponStar, value);
    }
    #endregion

    #region 最大生涯载具星数 LifeMaxVehicleStar
    private int lifeMaxVehicleStar;
    /// <summary>
    /// 最大生涯载具星数
    /// </summary>
    public int LifeMaxVehicleStar
    {
        get => lifeMaxVehicleStar;
        set => SetProperty(ref lifeMaxVehicleStar, value);
    }
    #endregion
}

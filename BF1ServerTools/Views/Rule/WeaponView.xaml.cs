using BF1ServerTools.Models;
using BF1ServerTools.Services;

namespace BF1ServerTools.Views.Rule;

/// <summary>
/// WeaponView.xaml 的交互逻辑
/// </summary>
public partial class WeaponView : UserControl
{
    /// <summary>
    /// 绑定UI 武器数据
    /// </summary>
    public ObservableCollection<RuleWeaponModel> DataGrid_RuleWeaponModels { get; set; } = new();

    public WeaponView()
    {
        InitializeComponent();

        // 添加武器数据列表
        foreach (var item in WeaponData.AllWeaponInfo)
        {
            DataGrid_RuleWeaponModels.Add(new()
            {
                Kind = item.Kind,
                Name = item.Chinese,
                English = item.English,
                Image = ClientUtil.GetWeaponImagePath(item.English),
                Team1 = false,
                Team2 = false
            });
        }
    }
}

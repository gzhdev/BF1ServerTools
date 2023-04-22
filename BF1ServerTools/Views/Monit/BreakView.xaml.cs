using BF1ServerTools.Models;
using BF1ServerTools.Services;

namespace BF1ServerTools.Views.Monit;

/// <summary>
/// BreakView.xaml 的交互逻辑
/// </summary>
public partial class BreakView : UserControl
{
    /// <summary>
    /// 绑定UI动态数据集合，用于更新违规玩家列表
    /// </summary>
    public ObservableCollection<MonitBreakModel> ListView_MonitBreakModels { get; set; } = new();

    public BreakView()
    {
        InitializeComponent();

        MonitService.UpdateBreakPlayerEvent += MonitService_UpdateBreakPlayerEvent;
    }

    private void MonitService_UpdateBreakPlayerEvent()
    {
        this.Dispatcher.BeginInvoke(() =>
        {
            UpdateListViewBreakRule();
        });
    }

    /// <summary>
    /// 动态更新 ListView 违规玩家列表
    /// </summary>
    private void UpdateListViewBreakRule()
    {
        // 如果玩家列表为空，则清空UI数据
        if (Globals.BreakRuleInfo_PlayerList.Count == 0 &&
            ListView_MonitBreakModels.Count != 0)
        {
            ListView_MonitBreakModels.Clear();
        }

        // 如果玩家列表为空，则退出
        if (Globals.BreakRuleInfo_PlayerList.Count == 0)
            return;

        // 更新ListView中现有的玩家数据，并把ListView中已经不在服务器的玩家清除
        for (int i = 0; i < ListView_MonitBreakModels.Count; i++)
        {
            var breakData = Globals.BreakRuleInfo_PlayerList.Find(val => val.PersonaId == ListView_MonitBreakModels[i].PersonaId);
            if (breakData != null)
            {
                ListView_MonitBreakModels[i].Rank = breakData.Rank;
                ListView_MonitBreakModels[i].Name = breakData.Name;
                ListView_MonitBreakModels[i].PersonaId = breakData.PersonaId;
                ListView_MonitBreakModels[i].IsAdmin = breakData.IsAdmin;
                ListView_MonitBreakModels[i].IsWhite = breakData.IsWhite;
                ListView_MonitBreakModels[i].Reason = breakData.Reason;
                ListView_MonitBreakModels[i].Count = breakData.BreakInfos.Count;

                var builder = new StringBuilder();
                foreach (var item in breakData.BreakInfos)
                {
                    builder.Append($"{item.BreakType}, ");
                }
                ListView_MonitBreakModels[i].AllReason = builder.ToString();
            }
            else
            {
                ListView_MonitBreakModels.RemoveAt(i);
            }
        }

        // 增加ListView没有的玩家数据
        for (int i = 0; i < Globals.BreakRuleInfo_PlayerList.Count; i++)
        {
            var breakData = ListView_MonitBreakModels.ToList().Find(val => val.PersonaId == Globals.BreakRuleInfo_PlayerList[i].PersonaId);
            if (breakData == null)
            {
                var builder = new StringBuilder();
                foreach (var item in Globals.BreakRuleInfo_PlayerList[i].BreakInfos)
                {
                    builder.Append($"{item.BreakType}, ");
                }

                ListView_MonitBreakModels.Add(new()
                {
                    Rank = Globals.BreakRuleInfo_PlayerList[i].Rank,
                    Name = Globals.BreakRuleInfo_PlayerList[i].Name,
                    PersonaId = Globals.BreakRuleInfo_PlayerList[i].PersonaId,
                    IsAdmin = Globals.BreakRuleInfo_PlayerList[i].IsAdmin,
                    IsWhite = Globals.BreakRuleInfo_PlayerList[i].IsWhite,
                    Reason = Globals.BreakRuleInfo_PlayerList[i].Reason,
                    Count = Globals.BreakRuleInfo_PlayerList[i].BreakInfos.Count,
                    AllReason = builder.ToString()
                });
            }
        }

        // 修正序号
        for (int i = 0; i < ListView_MonitBreakModels.Count; i++)
            ListView_MonitBreakModels[i].Index = i + 1;
    }
}

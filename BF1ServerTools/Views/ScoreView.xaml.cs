using BF1ServerTools.API;
using BF1ServerTools.Data;
using BF1ServerTools.Helpers;
using BF1ServerTools.Models;
using BF1ServerTools.Services;
using BF1ServerTools.Extensions;

namespace BF1ServerTools.Views;

/// <summary>
/// ScoreView.xaml 的交互逻辑
/// </summary>
public partial class ScoreView : UserControl
{
    /// <summary>
    /// 服务器信息
    /// </summary>
    public ScoreServerModel ScoreServerModel { get; set; } = new();

    /// <summary>
    /// 队伍1信息
    /// </summary>
    public ScoreTeamModel ScoreTeam1Model { get; set; } = new();
    /// <summary>
    /// 队伍2信息
    /// </summary>
    public ScoreTeamModel ScoreTeam2Model { get; set; } = new();

    ///////////////////////////////////////////////////////

    /// <summary>
    /// 绑定UI队伍1动态数据集合，用于更新ListView
    /// </summary>
    public ObservableCollection<ScorePlayerModel> ListView_PlayerList_Team1 { get; set; } = new();
    /// <summary>
    /// 绑定UI队伍2动态数据集合，用于更新ListView
    /// </summary>
    public ObservableCollection<ScorePlayerModel> ListView_PlayerList_Team2 { get; set; } = new();

    /// <summary>
    /// 绑定UI队伍0动态数据集合，用于更新观战列表
    /// </summary>
    public ObservableCollection<SpectatorData> ListBox_PlayerList_Team01 { get; set; } = new();
    /// <summary>
    /// 绑定UI队伍0动态数据集合，用于更新载入中列表
    /// </summary>
    public ObservableCollection<SpectatorData> ListBox_PlayerList_Team02 { get; set; } = new();

    ///////////////////////////////////////////////////////

    public ScoreView()
    {
        InitializeComponent();

        /////////////////////////////////

        GameService.UpdateServerDataEvent += GameService_UpdateServerDataEvent;

        GameService.UpdateTeam1DataEvent += GameService_UpdateTeam1DataEvent;
        GameService.UpdateTeam2DataEvent += GameService_UpdateTeam2DataEvent;

        GameService.UpdatePlayerListTeam1Event += GameService_UpdatePlayerListTeam1Event;
        GameService.UpdatePlayerListTeam2Event += GameService_UpdatePlayerListTeam2Event;

        GameService.UpdatePlayerListTeam01Event += GameService_UpdatePlayerListTeam01Event;
        GameService.UpdatePlayerListTeam02Event += GameService_UpdatePlayerListTeam02Event;
    }

    private void GameService_UpdateServerDataEvent(ServerData server)
    {
        ScoreServerModel.Name = server.Name;
        ScoreServerModel.GameId = server.GameId;
        ScoreServerModel.GameMode = server.GameMode;
        ScoreServerModel.MapName = server.MapName;
        ScoreServerModel.MapImg = server.MapImg;
        ScoreServerModel.GameTime = server.GameTime;

        ScoreServerModel.AllPlayerCount = server.AllPlayerCount;
    }

    private void GameService_UpdateTeam1DataEvent(TeamData team1Data)
    {
        UpdateTeam2Data(ScoreTeam1Model, team1Data);
    }

    private void GameService_UpdateTeam2DataEvent(TeamData team2Data)
    {
        UpdateTeam2Data(ScoreTeam2Model, team2Data);
    }

    private void UpdateTeam2Data(ScoreTeamModel scoreTeamModel, TeamData teamData)
    {
        FixedServerScore(scoreTeamModel, teamData);

        scoreTeamModel.TeamImg = teamData.TeamImg;
        scoreTeamModel.TeamName = teamData.TeamName;

        scoreTeamModel.AssaultKitCount = teamData.AssaultKitCount;
        scoreTeamModel.MedicKitCount = teamData.MedicKitCount;
        scoreTeamModel.SupportKitCount = teamData.SupportKitCount;
        scoreTeamModel.ScoutKitCount = teamData.ScoutKitCount;

        scoreTeamModel.PlayerCount = teamData.PlayerCount;
        scoreTeamModel.MaxPlayerCount = teamData.MaxPlayerCount;
        scoreTeamModel.Rank150PlayerCount = teamData.Rank150PlayerCount;
        scoreTeamModel.AllKillCount = teamData.AllKillCount;
        scoreTeamModel.AllDeadCount = teamData.AllDeadCount;
    }

    /// <summary>
    /// 修正服务器得分数据
    /// </summary>
    /// <param name="scoreTeamModel"></param>
    /// <param name="teamData"></param>
    private void FixedServerScore(ScoreTeamModel scoreTeamModel, TeamData teamData)
    {
        // 考虑其他特殊情况
        if (teamData.MaxScore <= 0 || teamData.MaxScore > 2000)
        {
            scoreTeamModel.ScoreWidth = 0;

            scoreTeamModel.AllScore = 0;
            scoreTeamModel.ScoreFlag = 0;
            scoreTeamModel.ScoreKill = 0;

            return;
        }

        scoreTeamModel.ScoreWidth = teamData.AllScore * 125 / teamData.MaxScore;

        scoreTeamModel.AllScore = teamData.AllScore;
        scoreTeamModel.ScoreFlag = teamData.ScoreFlag;
        scoreTeamModel.ScoreKill = teamData.ScoreKill;
    }

    private void GameService_UpdatePlayerListTeam1Event(List<PlayerData> playerList)
    {
        this.Dispatcher.BeginInvoke(() =>
        {
            UpdateListViewTeam(playerList, ListView_PlayerList_Team1);
        });
    }

    private void GameService_UpdatePlayerListTeam2Event(List<PlayerData> playerList)
    {
        this.Dispatcher.BeginInvoke(() =>
        {
            UpdateListViewTeam(playerList, ListView_PlayerList_Team2);
        });
    }

    private void GameService_UpdatePlayerListTeam01Event(List<PlayerData> playerList)
    {
        this.Dispatcher.BeginInvoke(() =>
        {
            UpdateListBoxTeam(playerList, ListBox_PlayerList_Team01);
        });
    }

    private void GameService_UpdatePlayerListTeam02Event(List<PlayerData> playerList)
    {
        this.Dispatcher.BeginInvoke(() =>
        {
            UpdateListBoxTeam(playerList, ListBox_PlayerList_Team02);
        });
    }

    /// <summary>
    /// 动态更新 ListView 队伍信息
    /// </summary>
    private void UpdateListViewTeam(List<PlayerData> playerList_Team, ObservableCollection<ScorePlayerModel> listView_Team)
    {
        // 如果玩家列表为空，则清空UI数据
        if (playerList_Team.Count == 0 && listView_Team.Count != 0)
        {
            listView_Team.Clear();
            return;
        }

        // 如果玩家列表为空，则退出
        if (playerList_Team.Count == 0)
            return;

        // 更新ListView中现有的玩家数据，并把ListView中已经不在服务器的玩家移除
        for (int i = 0; i < listView_Team.Count; i++)
        {
            var teamData = playerList_Team.Find(val => val.PersonaId == listView_Team[i].PersonaId);
            if (teamData != null)
            {
                listView_Team[i].Rank = teamData.Rank;
                listView_Team[i].Clan = teamData.Clan;
                listView_Team[i].IsAdmin = teamData.IsAdmin;
                listView_Team[i].IsVIP = teamData.IsVIP;
                listView_Team[i].IsWhite = teamData.IsWhite;
                listView_Team[i].SquadId = teamData.SquadId;
                listView_Team[i].SquadName = teamData.SquadName;
                listView_Team[i].Kill = teamData.Kill;
                listView_Team[i].Dead = teamData.Dead;
                listView_Team[i].KD = teamData.KD;
                listView_Team[i].KPM = teamData.KPM;
                listView_Team[i].LifeKD = teamData.LifeKD;
                listView_Team[i].LifeKPM = teamData.LifeKPM;
                listView_Team[i].LifeTime = teamData.LifeTime;
                listView_Team[i].Score = teamData.Score;
                listView_Team[i].Kit = teamData.Kit;
                listView_Team[i].KitImg = teamData.KitImg;
                listView_Team[i].KitName = teamData.KitName;
                listView_Team[i].WeaponS0 = teamData.WeaponS0;
                listView_Team[i].WeaponS1 = teamData.WeaponS1;
                listView_Team[i].WeaponS2 = teamData.WeaponS2;
                listView_Team[i].WeaponS3 = teamData.WeaponS3;
                listView_Team[i].WeaponS4 = teamData.WeaponS4;
                listView_Team[i].WeaponS5 = teamData.WeaponS5;
                listView_Team[i].WeaponS6 = teamData.WeaponS6;
                listView_Team[i].WeaponS7 = teamData.WeaponS7;
            }
            else
            {
                listView_Team.RemoveAt(i);
            }
        }

        // 增加ListView没有的玩家数据
        foreach (var item in playerList_Team)
        {
            var teamData = listView_Team.ToList().Find(val => val.PersonaId == item.PersonaId);
            if (teamData == null)
            {
                listView_Team.Add(new()
                {
                    Rank = item.Rank,
                    Clan = item.Clan,
                    Name = item.Name,
                    PersonaId = item.PersonaId,
                    IsAdmin = item.IsAdmin,
                    IsVIP = item.IsVIP,
                    IsWhite = item.IsWhite,
                    SquadId = item.SquadId,
                    SquadName = item.SquadName,
                    Kill = item.Kill,
                    Dead = item.Dead,
                    KD = item.KD,
                    KPM = item.KPM,
                    LifeKD = item.LifeKD,
                    LifeKPM = item.LifeKPM,
                    LifeTime = item.LifeTime,
                    Score = item.Score,
                    Kit = item.Kit,
                    KitImg = item.KitImg,
                    KitName = item.KitName,
                    WeaponS0 = item.WeaponS0,
                    WeaponS1 = item.WeaponS1,
                    WeaponS2 = item.WeaponS2,
                    WeaponS3 = item.WeaponS3,
                    WeaponS4 = item.WeaponS4,
                    WeaponS5 = item.WeaponS5,
                    WeaponS6 = item.WeaponS6,
                    WeaponS7 = item.WeaponS7
                });
            }
        }

        // 自定义排序
        listView_Team.Sort();

        // 修正序号
        for (int i = 0; i < listView_Team.Count; i++)
            listView_Team[i].Index = i + 1;
    }

    /// <summary>
    /// 动态更新 ListBox 队伍信息
    /// </summary>
    /// <param name="playerList_Team"></param>
    /// <param name="listBox_Team"></param>
    private void UpdateListBoxTeam(List<PlayerData> playerList_Team, ObservableCollection<SpectatorData> listBox_Team)
    {
        // 如果玩家列表为空，则清空UI数据
        if (playerList_Team.Count == 0 && listBox_Team.Count != 0)
        {
            listBox_Team.Clear();
            return;
        }

        // 如果玩家列表为空，则退出
        if (playerList_Team.Count == 0)
            return;

        // 把ListBox中已经不在服务器的玩家移除
        for (int i = 0; i < listBox_Team.Count; i++)
        {
            var teamData = playerList_Team.Find(val => val.PersonaId == listBox_Team[i].PersonaId);
            if (teamData == null)
                listBox_Team.RemoveAt(i);
        }

        // 增加ListBox没有的玩家数据
        foreach (var item in playerList_Team)
        {
            var teamData = listBox_Team.ToList().Find(val => val.PersonaId == item.PersonaId);
            if (teamData == null)
            {
                listBox_Team.Add(new()
                {
                    Name = item.Name,
                    PersonaId = item.PersonaId
                });
            }
        }
    }

    /// <summary>
    /// 得分板排序规则选中变更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ComboBox_ScoreSortRule_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Globals.OrderBy = (OrderBy)ComboBox_ScoreSortRule.SelectedIndex;
    }

    /// <summary>
    /// 自动调整列宽
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_AutoColumWidth_Click(object sender, RoutedEventArgs e)
    {
        Button_AutoColumWidth.IsEnabled = false;

        if (ListView_Team1.View is GridView gv1)
        {
            foreach (GridViewColumn gvc in gv1.Columns)
            {
                gvc.Width = 100;
                gvc.Width = double.NaN;
            }
        }

        if (ListView_Team2.View is GridView gv2)
        {
            foreach (GridViewColumn gvc in gv2.Columns)
            {
                gvc.Width = 100;
                gvc.Width = double.NaN;
            }
        }

        CheckBox_GridView_Header0.IsChecked = true;
        CheckBox_GridView_Header1.IsChecked = true;
        CheckBox_GridView_Header2.IsChecked = true;
        CheckBox_GridView_Header3.IsChecked = true;
        CheckBox_GridView_Header4.IsChecked = true;
        CheckBox_GridView_Header5.IsChecked = true;
        CheckBox_GridView_Header6.IsChecked = true;
        CheckBox_GridView_Header7.IsChecked = true;
        CheckBox_GridView_Header8.IsChecked = true;
        CheckBox_GridView_Header9.IsChecked = true;
        CheckBox_GridView_Header10.IsChecked = true;
        CheckBox_GridView_Header11.IsChecked = true;
        CheckBox_GridView_Header12.IsChecked = true;
        CheckBox_GridView_Header13.IsChecked = true;
        CheckBox_GridView_Header14.IsChecked = true;
        CheckBox_GridView_Header15.IsChecked = true;
        CheckBox_GridView_Header16.IsChecked = true;
        CheckBox_GridView_Header17.IsChecked = true;
        CheckBox_GridView_Header18.IsChecked = true;
        CheckBox_GridView_Header19.IsChecked = true;
        CheckBox_GridView_Header20.IsChecked = true;

        Button_AutoColumWidth.IsEnabled = true;
    }

    /// <summary>
    /// 队伍1 ListView选中变更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ListView_Team1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ListView_Team1.SelectedItem is ScorePlayerModel item)
            MenuItem_Team1.Header = $"[队伍1]  {item.Name}";
        else
            MenuItem_Team1.Header = "[队伍1]  当前未选中";
    }

    /// <summary>
    /// 队伍2 ListView选中变更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ListView_Team2_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ListView_Team2.SelectedItem is ScorePlayerModel item)
            MenuItem_Team2.Header = $"[队伍2]  {item.Name}";
        else
            MenuItem_Team2.Header = "[队伍2]  当前未选中";
    }

    /// <summary>
    /// 观战 ListBox选中变更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ListBox_Team01_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ListBox_Team01.SelectedItem is SpectatorData item)
            MenuItem_Team01.Header = $"[观战]  {item.Name}";
        else
            MenuItem_Team01.Header = "[观战]  当前未选中";
    }

    /// <summary>
    /// 载入中 ListBox选中变更事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ListBox_Team02_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ListBox_Team02.SelectedItem is SpectatorData item)
            MenuItem_Team02.Header = $"[载入中]  {item.Name}";
        else
            MenuItem_Team02.Header = "[载入中]  当前未选中";
    }

    #region 队伍1、队伍2 公共方法
    private async void ChangePlayerTeam(ListView listView, Team team)
    {
        var teamInfo = PlayerUtil.GetTeamInfo(team);

        if (listView.SelectedItem is ScorePlayerModel item)
        {
            // 检查权限
            if (!PlayerUtil.CheckPlayerAuth())
                return;

            NotifierHelper.Show(NotifierType.Information, $"正在更换玩家 {item.Name} 队伍中...");

            var result = await BF1API.RSPMovePlayer(Globals.SessionId, Globals.GameId, item.PersonaId, (int)team);
            if (result.IsSuccess)
                NotifierHelper.Show(NotifierType.Success, $"[{result.ExecTime:0.00} 秒]  更换玩家 {item.Name} 队伍成功");
            else
                NotifierHelper.Show(NotifierType.Error, $"[{result.ExecTime:0.00} 秒]  更换玩家 {item.Name} 队伍失败\n{result.Content}");
        }
        else
        {
            ViewUtil.UnSelectedNotifier(teamInfo);
        }
    }

    private void KickPlayerCustom(ListView listView, Team team)
    {
        var teamInfo = PlayerUtil.GetTeamInfo(team);

        if (listView.SelectedItem is ScorePlayerModel item)
            ViewUtil.KickPlayerCustom(item.Name, item.PersonaId);
        else
            ViewUtil.UnSelectedNotifier(teamInfo);
    }

    private void KickPlayer(ListView listView, string reason, Team team)
    {
        var teamInfo = PlayerUtil.GetTeamInfo(team);

        if (listView.SelectedItem is ScorePlayerModel item)
            ViewUtil.KickPlayer(item.Name, item.PersonaId, reason);
        else
            ViewUtil.UnSelectedNotifier(teamInfo);
    }

    private void CopyPlayerName(ListView listView, Team team)
    {
        var teamInfo = PlayerUtil.GetTeamInfo(team);

        if (listView.SelectedItem is ScorePlayerModel item)
            ViewUtil.Copy2Clipboard(item.Name);
        else
            ViewUtil.UnSelectedNotifier(teamInfo);
    }

    private void CopyPlayerPersonaId(ListView listView, Team team)
    {
        var teamInfo = PlayerUtil.GetTeamInfo(team);

        if (listView.SelectedItem is ScorePlayerModel item)
            ViewUtil.Copy2Clipboard(item.PersonaId.ToString());
        else
            ViewUtil.UnSelectedNotifier(teamInfo);
    }

    private void CopyPlayerAllData(ListView listView, Team team)
    {
        var teamInfo = PlayerUtil.GetTeamInfo(team);

        if (listView.SelectedItem is ScorePlayerModel item)
        {
            var builder = new StringBuilder();

            builder.Append($"序号：{item.Index}，");
            builder.Append($"等级：{item.Rank}，");
            builder.Append($"战队：{item.Clan}，");
            builder.Append($"玩家ID：{item.Name}，");
            builder.Append($"数字ID：{item.PersonaId}，");
            builder.Append($"小队：{item.SquadName}，");
            builder.Append($"击杀：{item.Kill}，");
            builder.Append($"死亡：{item.Dead}，");
            builder.Append($"KD：{item.KD}，");
            builder.Append($"KPM：{item.KPM}，");
            builder.Append($"得分：{item.Score}，");
            builder.Append($"生涯KD：{item.LifeKD}，");
            builder.Append($"生涯KPM：{item.LifeKPM}，");
            builder.Append($"生涯时长：{item.LifeTime}，");
            builder.Append($"兵种：{item.KitName}，");
            builder.Append($"主武器：{item.WeaponS0}，");
            builder.Append($"配枪：{item.WeaponS1}，");
            builder.Append($"配备一：{item.WeaponS2}，");
            builder.Append($"配备二：{item.WeaponS5}，");
            builder.Append($"特殊：{item.WeaponS3}，");
            builder.Append($"手榴弹：{item.WeaponS6}，");
            builder.Append($"近战：{item.WeaponS7}。");

            ViewUtil.Copy2Clipboard(builder.ToString());
        }
        else
        {
            ViewUtil.UnSelectedNotifier(teamInfo);
        }
    }

    private void QueryPlayerRecord(ListView listView, Team team)
    {
        var teamInfo = PlayerUtil.GetTeamInfo(team);

        if (listView.SelectedItem is ScorePlayerModel item)
            ViewUtil.QueryPlayerRecord(item.Name, item.PersonaId, item.Rank);
        else
            ViewUtil.UnSelectedNotifier(teamInfo);
    }
    #endregion

    #region 观战、载入中 公用方法
    private void KickPlayerCustom(ListBox listBox, Team team)
    {
        var teamInfo = PlayerUtil.GetTeamInfo(team);

        if (listBox.SelectedItem is SpectatorData item)
            ViewUtil.KickPlayerCustom(item.Name, item.PersonaId);
        else
            ViewUtil.UnSelectedNotifier(teamInfo);
    }

    private void KickPlayer(ListBox listBox, string reason, Team team)
    {
        var teamInfo = PlayerUtil.GetTeamInfo(team);

        if (listBox.SelectedItem is SpectatorData item)
            ViewUtil.KickPlayer(item.Name, item.PersonaId, reason);
        else
            ViewUtil.UnSelectedNotifier(teamInfo);
    }

    private void CopyPlayerName(ListBox listBox, Team team)
    {
        var teamInfo = PlayerUtil.GetTeamInfo(team);

        if (listBox.SelectedItem is SpectatorData item)
            ViewUtil.Copy2Clipboard(item.Name);
        else
            ViewUtil.UnSelectedNotifier(teamInfo);
    }

    private void CopyPlayerPersonaId(ListBox listBox, Team team)
    {
        var teamInfo = PlayerUtil.GetTeamInfo(team);

        if (listBox.SelectedItem is SpectatorData item)
            ViewUtil.Copy2Clipboard(item.PersonaId.ToString());
        else
            ViewUtil.UnSelectedNotifier(teamInfo);
    }

    private void QueryPlayerRecord(ListBox listBox, Team team)
    {
        var teamInfo = PlayerUtil.GetTeamInfo(team);

        if (listBox.SelectedItem is SpectatorData item)
            ViewUtil.QueryPlayerRecord(item.Name, item.PersonaId, (int)team);
        else
            ViewUtil.UnSelectedNotifier(teamInfo);
    }
    #endregion

    #region 队伍1 右键菜单事件
    private void MenuItem_Team1_ChangePlayerTeam_Click(object sender, RoutedEventArgs e)
    {
        ChangePlayerTeam(ListView_Team1, Team.Team1);
    }

    private void MenuItem_Team1_KickPlayerCustom_Click(object sender, RoutedEventArgs e)
    {
        KickPlayerCustom(ListView_Team1, Team.Team1);
    }

    private void MenuItem_Team1_KickPlayerOffensiveBehavior_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListView_Team1, "OFFENSIVEBEHAVIOR", Team.Team1);
    }

    private void MenuItem_Team1_KickPlayerLatency_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListView_Team1, "LATENCY", Team.Team1);
    }

    private void MenuItem_Team1_KickPlayerRuleViolation_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListView_Team1, "RULEVIOLATION", Team.Team1);
    }

    private void MenuItem_Team1_KickPlayerGeneral_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListView_Team1, "GENERAL", Team.Team1);
    }

    private void MenuItem_Team1_CopyPlayerName_Click(object sender, RoutedEventArgs e)
    {
        CopyPlayerName(ListView_Team1, Team.Team1);
    }

    private void MenuItem_Team1_CopyPlayerPersonaId_Click(object sender, RoutedEventArgs e)
    {
        CopyPlayerPersonaId(ListView_Team1, Team.Team1);
    }

    private void MenuItem_Team1_CopyPlayerAllData_Click(object sender, RoutedEventArgs e)
    {
        CopyPlayerAllData(ListView_Team1, Team.Team1);
    }

    private void MenuItem_Team1_QueryPlayerRecord_Click(object sender, RoutedEventArgs e)
    {
        QueryPlayerRecord(ListView_Team1, Team.Team1);
    }
    #endregion

    #region 队伍2 右键菜单事件
    private void MenuItem_Team2_ChangePlayerTeam_Click(object sender, RoutedEventArgs e)
    {
        ChangePlayerTeam(ListView_Team2, Team.Team2);
    }

    private void MenuItem_Team2_KickPlayerCustom_Click(object sender, RoutedEventArgs e)
    {
        KickPlayerCustom(ListView_Team2, Team.Team2);
    }

    private void MenuItem_Team2_KickPlayerOffensiveBehavior_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListView_Team2, "OFFENSIVEBEHAVIOR", Team.Team2);
    }

    private void MenuItem_Team2_KickPlayerLatency_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListView_Team2, "LATENCY", Team.Team2);
    }

    private void MenuItem_Team2_KickPlayerRuleViolation_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListView_Team2, "RULEVIOLATION", Team.Team2);
    }

    private void MenuItem_Team2_KickPlayerGeneral_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListView_Team2, "GENERAL", Team.Team2);
    }

    private void MenuItem_Team2_CopyPlayerName_Click(object sender, RoutedEventArgs e)
    {
        CopyPlayerName(ListView_Team2, Team.Team2);
    }

    private void MenuItem_Team2_CopyPlayerPersonaId_Click(object sender, RoutedEventArgs e)
    {
        CopyPlayerPersonaId(ListView_Team2, Team.Team2);
    }

    private void MenuItem_Team2_CopyPlayerAllData_Click(object sender, RoutedEventArgs e)
    {
        CopyPlayerAllData(ListView_Team2, Team.Team2);
    }

    private void MenuItem_Team2_QueryPlayerRecord_Click(object sender, RoutedEventArgs e)
    {
        QueryPlayerRecord(ListView_Team2, Team.Team2);
    }
    #endregion

    #region 观战 右键菜单事件
    private void MenuItem_Team01_KickPlayerCustom_Click(object sender, RoutedEventArgs e)
    {
        KickPlayerCustom(ListBox_Team01, Team.Team01);
    }

    private void MenuItem_Team01_KickPlayerOffensiveBehavior_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListBox_Team01, "OFFENSIVEBEHAVIOR", Team.Team01);
    }

    private void MenuItem_Team01_KickPlayerLatency_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListBox_Team01, "LATENCY", Team.Team01);
    }

    private void MenuItem_Team01_KickPlayerRuleViolation_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListBox_Team01, "RULEVIOLATION", Team.Team01);
    }

    private void MenuItem_Team01_KickPlayerGeneral_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListBox_Team01, "GENERAL", Team.Team01);
    }

    private void MenuItem_Team01_CopyPlayerName_Click(object sender, RoutedEventArgs e)
    {
        CopyPlayerName(ListBox_Team01, Team.Team01);
    }

    private void MenuItem_Team01_CopyPlayerPersonaId_Click(object sender, RoutedEventArgs e)
    {
        CopyPlayerPersonaId(ListBox_Team01, Team.Team01);
    }

    private void MenuItem_Team01_QueryPlayerRecord_Click(object sender, RoutedEventArgs e)
    {
        QueryPlayerRecord(ListBox_Team01, Team.Team01);
    }
    #endregion

    #region 载入中 右键菜单事件
    private void MenuItem_Team02_KickPlayerCustom_Click(object sender, RoutedEventArgs e)
    {
        KickPlayerCustom(ListBox_Team02, Team.Team02);
    }

    private void MenuItem_Team02_KickPlayerOffensiveBehavior_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListBox_Team02, "OFFENSIVEBEHAVIOR", Team.Team02);
    }

    private void MenuItem_Team02_KickPlayerLatency_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListBox_Team02, "LATENCY", Team.Team02);
    }

    private void MenuItem_Team02_KickPlayerRuleViolation_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListBox_Team02, "RULEVIOLATION", Team.Team02);
    }

    private void MenuItem_Team02_KickPlayerGeneral_Click(object sender, RoutedEventArgs e)
    {
        KickPlayer(ListBox_Team02, "GENERAL", Team.Team02);
    }

    private void MenuItem_Team02_CopyPlayerName_Click(object sender, RoutedEventArgs e)
    {
        CopyPlayerName(ListBox_Team02, Team.Team02);
    }

    private void MenuItem_Team02_CopyPlayerPersonaId_Click(object sender, RoutedEventArgs e)
    {
        CopyPlayerPersonaId(ListBox_Team02, Team.Team02);
    }

    private void MenuItem_Team02_QueryPlayerRecord_Click(object sender, RoutedEventArgs e)
    {
        QueryPlayerRecord(ListBox_Team02, Team.Team02);
    }
    #endregion

    #region 自定义ListView显示/隐藏列
    private void MakeGridViewColumns(bool isChecked, int index)
    {
        if (isChecked)
        {
            GridView_Team1.Columns[index].Width = 100;
            GridView_Team1.Columns[index].Width = double.NaN;

            GridView_Team2.Columns[index].Width = 100;
            GridView_Team2.Columns[index].Width = double.NaN;
        }
        else
        {
            GridView_Team1.Columns[index].Width = 0;
            GridView_Team2.Columns[index].Width = 0;
        }
    }

    private void CheckBox_GridView_Header0_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header0.IsChecked == true, 0);
    }

    private void CheckBox_GridView_Header1_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header1.IsChecked == true, 1);
    }

    private void CheckBox_GridView_Header2_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header2.IsChecked == true, 2);
    }

    private void CheckBox_GridView_Header3_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header3.IsChecked == true, 3);
    }

    private void CheckBox_GridView_Header4_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header4.IsChecked == true, 4);
    }

    private void CheckBox_GridView_Header5_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header5.IsChecked == true, 5);
    }

    private void CheckBox_GridView_Header6_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header6.IsChecked == true, 6);
    }

    private void CheckBox_GridView_Header7_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header7.IsChecked == true, 7);
    }

    private void CheckBox_GridView_Header8_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header8.IsChecked == true, 8);
    }

    private void CheckBox_GridView_Header9_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header9.IsChecked == true, 9);
    }

    private void CheckBox_GridView_Header10_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header10.IsChecked == true, 10);
    }

    private void CheckBox_GridView_Header11_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header11.IsChecked == true, 11);
    }

    private void CheckBox_GridView_Header12_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header12.IsChecked == true, 12);
    }

    private void CheckBox_GridView_Header13_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header13.IsChecked == true, 13);
    }

    private void CheckBox_GridView_Header14_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header14.IsChecked == true, 14);
    }

    private void CheckBox_GridView_Header15_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header15.IsChecked == true, 15);
    }

    private void CheckBox_GridView_Header16_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header16.IsChecked == true, 16);
    }

    private void CheckBox_GridView_Header17_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header17.IsChecked == true, 17);
    }

    private void CheckBox_GridView_Header18_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header18.IsChecked == true, 18);
    }

    private void CheckBox_GridView_Header19_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header19.IsChecked == true, 19);
    }

    private void CheckBox_GridView_Header20_Click(object sender, RoutedEventArgs e)
    {
        MakeGridViewColumns(CheckBox_GridView_Header20.IsChecked == true, 20);
    }
    #endregion
}

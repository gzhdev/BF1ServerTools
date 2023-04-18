using BF1ServerTools.Data;

namespace BF1ServerTools.Configs;

public class RuleConfig
{
    public int SelectedIndex { get; set; }
    public List<RuleInfo> RuleInfos { get; set; }

    public class RuleInfo
    {
        public string RuleName { get; set; }

        public IgnoreData WhiteIgnore { get; set; }

        public GeneralData Team1General { get; set; }
        public GeneralData Team2General { get; set; }

        public LifeData Team1Life { get; set; }
        public LifeData Team2Life { get; set; }

        public List<string> Team1Weapon { get; set; }
        public List<string> Team2Weapon { get; set; }

        public List<string> BlackData { get; set; }
        public List<string> WhiteData { get; set; }
    }
}

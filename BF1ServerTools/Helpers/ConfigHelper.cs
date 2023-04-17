using SharpConfig;

namespace BF1ServerTools.Helpers;

public static class ConfigHelper
{
    private static readonly Configuration _config;

    private static readonly string cfgPath = Path.Combine(FileHelper.Dir_Config, "Config.cfg");

    static ConfigHelper()
    {
        // 如果文件不存在则创建后关闭
        if (!File.Exists(cfgPath))
            File.Create(cfgPath).Close();

        // 加载配置文件
        _config = Configuration.LoadFromFile(cfgPath);
    }

    public static void SaveConfig()
    {
        _config.SaveToFile(cfgPath);
    }

    //////////////////////////////////////////////////////

    public static int ReadInt(string section, string key)
    {
        return _config[section][key].IntValue;
    }

    public static float ReadFloat(string section, string key)
    {
        return _config[section][key].FloatValue;
    }

    public static bool ReadBool(string section, string key)
    {
        return _config[section][key].BoolValue;
    }

    public static string ReadString(string section, string key)
    {
        return _config[section][key].StringValue;
    }

    //////////////////////////////////////////////////////

    public static void WriteInt(string section, string key, int value)
    {
        _config[section][key].IntValue = value;
    }

    public static void WriteFloat(string section, string key, float value)
    {
        _config[section][key].FloatValue = value;
    }

    public static void WriteBool(string section, string key, bool value)
    {
        _config[section][key].BoolValue = value;
    }

    public static void WriteString(string section, string key, string value)
    {
        _config[section][key].StringValue = value;
    }
}

using Chinese;

namespace BF1ServerTools.Helpers;

public static class ChsHelper
{
    /// <summary>
    /// 预热简繁库
    /// </summary>
    public static void PreHeat()
    {
        ToTraditional("免费，跨平台，开源！");
    }

    /// <summary>
    /// 字符串简体转繁体
    /// </summary>
    /// <param name="simpleStr"></param>
    /// <returns></returns>
    public static string ToTraditional(string simpleStr)
    {
        return ChineseConverter.ToTraditional(simpleStr);
    }

    /// <summary>
    /// 字符串繁体转简体
    /// </summary>
    /// <param name="traditionalStr"></param>
    /// <returns></returns>
    public static string ToSimplified(string traditionalStr)
    {
        return ChineseConverter.ToSimplified(traditionalStr);
    }
}

using Microsoft.Web.WebView2.Core;

namespace BF1ServerTools.Utils;

public static class MiscUtil
{
    [DllImport("User32")]
    private static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("User32")]
    private static extern bool CloseClipboard();

    [DllImport("User32")]
    private static extern bool EmptyClipboard();

    [DllImport("User32")]
    private static extern bool IsClipboardFormatAvailable(int format);

    [DllImport("User32")]
    private static extern IntPtr GetClipboardData(int uFormat);

    [DllImport("User32", CharSet = CharSet.Unicode)]
    private static extern IntPtr SetClipboardData(int uFormat, IntPtr hMem);

    /// <summary>
    /// 向剪贴板中添加文本
    /// </summary>
    /// <param name="text">文本</param>
    public static void SetText(string text)
    {
        if (!OpenClipboard(IntPtr.Zero))
        {
            SetText(text);
            return;
        }

        EmptyClipboard();
        SetClipboardData(13, Marshal.StringToHGlobalUni(text));
        CloseClipboard();
    }

    /// <summary>
    /// 计算时间差，即软件运行时间
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <returns></returns>
    public static string ExecDateDiff(DateTime startTime, DateTime endTime)
    {
        var ts1 = new TimeSpan(startTime.Ticks);
        var ts2 = new TimeSpan(endTime.Ticks);

        return ts1.Subtract(ts2).Duration().ToString("c")[..8];
    }

    /// <summary>
    /// 检查WebView2依赖
    /// </summary>
    /// <returns></returns>
    public static bool CheckWebView2Env()
    {
        try
        {
            var env = CoreWebView2Environment.GetAvailableBrowserVersionString();
            return !string.IsNullOrWhiteSpace(env);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 获取子控件集合
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static IList<Control> GetControls(this DependencyObject parent)
    {
        var result = new List<Control>();

        for (int x = 0; x < VisualTreeHelper.GetChildrenCount(parent); x++)
        {
            var child = VisualTreeHelper.GetChild(parent, x);

            if (child is Control instance)
                result.Add(instance);

            result.AddRange(child.GetControls());
        }

        return result;
    }
}

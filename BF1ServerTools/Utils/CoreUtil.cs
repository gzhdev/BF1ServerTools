using Microsoft.Web.WebView2.Core;

namespace BF1ServerTools.Utils;

public static class CoreUtil
{
    #region Native方法
    [DllImport("user32.dll")]
    private static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll")]
    private static extern bool CloseClipboard();

    [DllImport("user32.dll")]
    private static extern bool EmptyClipboard();

    [DllImport("user32.dll")]
    private static extern bool IsClipboardFormatAvailable(int format);

    [DllImport("user32.dll")]
    private static extern IntPtr GetClipboardData(int uFormat);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SetClipboardData(int uFormat, IntPtr hMem);

    [DllImport("kernel32.dll")]
    private static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
    #endregion

    /// <summary>
    /// 程序客户端版本号，如：1.2.3.4
    /// </summary>
    public static Version VersionInfo = Application.ResourceAssembly.GetName().Version;

    /// <summary>
    /// 程序客户端最后编译时间
    /// </summary>
    public static DateTime BuildDate = File.GetLastWriteTime(Environment.ProcessPath);

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

    /// <summary>
    /// 内存回收
    /// </summary>
    public static void ClearMemory()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();

        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            _ = SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
    }
}

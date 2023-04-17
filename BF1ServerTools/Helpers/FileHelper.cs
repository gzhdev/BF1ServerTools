namespace BF1ServerTools.Helpers;

public static class FileHelper
{
    public const string ResFiles = "BF1ServerTools.Assets.Files";

    public const string Res_Robot_Config = $"{ResFiles}.Robot.config.yml";
    public const string Res_Robot_GoCqHttp = $"{ResFiles}.Robot.go-cqhttp.exe";

    //////////////////////////////////////////////////////////////////

    public static string Dir_MyDocuments { get; private set; }

    public static string Dir_Default { get; private set; }

    public static string Dir_Cache { get; private set; }
    public static string Dir_Config { get; private set; }
    public static string Dir_Data { get; private set; }
    public static string Dir_Log { get; private set; }
    public static string Dir_Robot { get; private set; }

    public static string Dir_Log_Crash { get; private set; }
    public static string Dir_Log_NLog { get; private set; }

    public static string File_Robot_Config { get; private set; }
    public static string File_Robot_GoCqHttp { get; private set; }

    static FileHelper()
    {
        Dir_MyDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        Dir_Default = Path.Combine(Dir_MyDocuments, "BF1ServerTools");

        Dir_Cache = Path.Combine(Dir_Default, "Cache");
        Dir_Config = Path.Combine(Dir_Default, "Config");
        Dir_Data = Path.Combine(Dir_Default, "Data");
        Dir_Log = Path.Combine(Dir_Default, "Log");
        Dir_Robot = Path.Combine(Dir_Default, "Robot");

        Dir_Log_Crash = Path.Combine(Dir_Log, "Crash");
        Dir_Log_NLog = Path.Combine(Dir_Log, "NLog");

        File_Robot_Config = Path.Combine(Dir_Robot, "config.yml");
        File_Robot_GoCqHttp = Path.Combine(Dir_Robot, "go-cqhttp.exe");
    }

    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="dirPath"></param>
    public static void CreateDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
    }

    /// <summary>
    /// 保存崩溃日志
    /// </summary>
    /// <param name="log">日志内容</param>
    public static void SaveCrashLog(string log)
    {
        var path = Path.Combine(Dir_Log_Crash, $"#Crash#{DateTime.Now:yyyyMMdd_HH-mm-ss_ffff}.log");
        File.WriteAllText(path, log);
    }

    /// <summary>
    /// 从资源文件中抽取资源文件
    /// </summary>
    /// <param name="resFileName"></param>
    /// <param name="outputFile"></param>
    /// <param name="isOverride"></param>
    public static void ExtractResFile(string resFileName, string outputFile, bool isOverride = false)
    {
        var assembly = Assembly.GetExecutingAssembly();
        ExtractResFile(assembly, resFileName, outputFile, isOverride);
    }

    /// <summary>
    /// 从资源文件中抽取资源文件，默认不覆盖源文件
    /// </summary>
    /// <param name="assembly">程序集信息</param>
    /// <param name="resFileName">资源文件路径</param>
    /// <param name="outputFile">输出文件</param>
    /// <param name="isOverride">是否覆盖文件</param>
    public static void ExtractResFile(Assembly assembly, string resFileName, string outputFile, bool isOverride = false)
    {
        // 如果输出文件存在，并且不覆盖文件，则退出
        if (File.Exists(outputFile) && !isOverride)
            return;

        BufferedStream inStream = null;
        FileStream outStream = null;

        try
        {
            inStream = new BufferedStream(assembly.GetManifestResourceStream(resFileName));
            outStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);

            var buffer = new byte[1024];
            int length;

            while ((length = inStream.Read(buffer, 0, buffer.Length)) > 0)
                outStream.Write(buffer, 0, length);

            outStream.Flush();
        }
        finally
        {
            outStream?.Close();
            inStream?.Close();
        }
    }

    /// <summary>
    /// 清空指定文件夹下的文件及文件夹
    /// </summary>
    /// <param name="srcPath">文件夹路径</param>
    public static void ClearDirectory(string srcPath)
    {
        try
        {
            var dir = new DirectoryInfo(srcPath);
            var fileinfo = dir.GetFileSystemInfos();

            foreach (var file in fileinfo)
            {
                if (file is DirectoryInfo)
                {
                    var subdir = new DirectoryInfo(file.FullName);
                    subdir.Delete(true);
                }
                else
                {
                    File.Delete(file.FullName);
                }
            }
        }
        catch { }
    }
}

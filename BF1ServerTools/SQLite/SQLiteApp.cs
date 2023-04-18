using BF1ServerTools.Helpers;

using FreeSql;

namespace BF1ServerTools.SQLite;

public static class SQLiteApp
{
    private static IFreeSql _freeSql;

    /// <summary>
    /// SQLite数据库初始化
    /// </summary>
    public static bool Initialize()
    {
        if (_freeSql != null)
            return true;

        try
        {
            string connectionString = $"Data Source={Path.Combine(FileHelper.Dir_Data, "Server.db")}";

            _freeSql = new FreeSqlBuilder()
                .UseConnectionString(DataType.Sqlite, connectionString)
                .UseAutoSyncStructure(true)
                .Build();

            LoggerHelper.Info("SQLite数据库初始化成功");
            return true;
        }
        catch (Exception ex)
        {
            LoggerHelper.Error("SQLite数据库初始化异常", ex);
            return false;
        }
    }

    /// <summary>
    /// 获取生涯缓存信息
    /// </summary>
    /// <returns></returns>
    public static List<LifeCacheDb> ReadLifeCacheDb()
    {
        return _freeSql.Select<LifeCacheDb>().ToList();
    }

    /// <summary>
    /// 保持生涯缓存信息
    /// </summary>
    /// <param name="lifeCacheDbs"></param>
    public static void SaveLifeCacheDb(List<LifeCacheDb> lifeCacheDbs)
    {
        // 清空表
        _freeSql.Delete<LifeCacheDb>().Where("1=1").ExecuteAffrows();
        // 批量插入数据
        _freeSql.Insert(lifeCacheDbs).ExecuteAffrows();
    }
}

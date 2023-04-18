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
    /// 查询生涯缓存信息
    /// </summary>
    /// <param name="personaId"></param>
    /// <returns></returns>
    public static List<LifeCacheDb> QueryLifeCacheDb()
    {
        return _freeSql.Select<LifeCacheDb>().ToList();
    }

    /// <summary>
    /// 查询生涯缓存信息
    /// </summary>
    /// <param name="personaId"></param>
    /// <returns></returns>
    public static LifeCacheDb QueryLifeCacheDb(long personaId)
    {
        return _freeSql.Select<LifeCacheDb>().Where(x => x.PersonaId == personaId).ToOne();
    }

    /// <summary>
    /// 插入生涯缓存信息
    /// </summary>
    /// <param name="lifeCacheDb"></param>
    public static void AddLifeCacheDb(LifeCacheDb lifeCacheDb)
    {
        _freeSql.Insert(lifeCacheDb).ExecuteIdentity();
    }

    /// <summary>
    /// 删除生涯缓存信息
    /// </summary>
    /// <param name="lifeCacheDb"></param>
    public static void DeleteLifeCacheDb(long personaId)
    {
        _freeSql.Delete<LifeCacheDb>().Where(x => x.PersonaId == personaId).ExecuteAffrows();
    }
}

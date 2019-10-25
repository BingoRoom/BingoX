using System;

namespace BingoX.Domain
{
    /// <summary>
    /// 数据库实体接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IEntity<T, TId> where T : class
    {
        TId ID { get; set; }
    }

    /// <summary>
    /// 支持自增主键的数据库实体接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface IIdentityEntity<T> : IEntity<T, int> where T : class
    {

    }
    /// <summary>
    /// 支持字符串主键的数据实体接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface IStringEntity<T> : IEntity<T, string> where T : class
    {

    }
    /// <summary>
    /// 支持雪花主键的数据库实体接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface ISnowflakeEntity<T> : IEntity<T, long> where T : class
    {

    }
    /// <summary>
    /// 支持GUID主键的数据库实体接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface IGuidEntity<T> : IEntity<T, Guid> where T : class
    {

    }
}
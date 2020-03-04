using System;

namespace BingoX.Domain
{
    /// <summary>
    /// 表示一个数据库实体
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IEntity<T>  
    {

    }
    /// <summary>
    /// 表示一个数据库实体
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="TId">主键类型</typeparam>
    public interface IEntity<T, TId> : IEntity<T>  
    {
        TId ID { get; set; }
    }



    /// <summary>
    /// 表示一个自增主键的数据库实体
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IIdentityEntity<T> : IEntity<T, int>  
    {

    }
    /// <summary>
    /// 表示一个以字符串类型为主键的数据库实体
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IStringEntity<T> : IEntity<T, string>  
    {

    }
    /// <summary>
    /// 表示一个以雪花类型为主键的数据库实体
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface ISnowflakeEntity<T> : IEntity<T, long>  
    {

    }
    /// <summary>
    /// 表示一个以GUID类型为主键的数据库实体
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IGuidEntity<T> : IEntity<T, Guid>  
    {

    }
}
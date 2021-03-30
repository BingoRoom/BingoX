using System;
using System.Collections.Generic;

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

    public interface ISoftDelete<TKey> : IEntity<TKey>
    {


        bool SoftDeleted { get; set; }
    }
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        public virtual TKey ID { get; protected set; }
        public virtual object[] GetKeys()
        {
            return new object[] { ID };
        }
        int? _requestedHashCode;
        /// <summary>
        /// 表示对象是否相等
        /// 这个方法的重载使我们可以正确的判断两个实体是否是同一个实体
        /// 根据 Id 判断，如果没有 Id 的话，两个实体是不会相等的
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TKey>))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity<TKey> item = (Entity<TKey>)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.ID.Equals(this.ID);
        }

        /// <summary>
        /// 这个方法用来辅助对比两个对象是否相等
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.ID.GetHashCode() ^ 31;

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }

        /// <summary>
        /// 表示对象是否为全新创建的，未持久化的
        /// </summary>
        /// <returns></returns>
        public bool IsTransient()
        {
            // 如果它没有 Id 就表示它没有持久化
            return EqualityComparer<TKey>.Default.Equals(ID, default);
        }

        public override string ToString()
        {
            return $"[Entity: {GetType().Name}] Id = {ID}";
        }

        /// <summary>
        /// 操作符 == 重载
        /// 借助上面的 Equals 方法
        /// 使得可以直接用 == 判断两个领域对象是否相等
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        /// <summary>
        /// 操作符 != 重载
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
        {
            return !(left == right);
        }
    }
}

using System;

namespace BingoX.DataAccessor
{
    /// <summary>
    /// 表示一个DAO当前的数据库操作状态信息
    /// </summary>
    public class DbEntityInfo
    {
        /// <summary>
        /// 为当前被操作的DAO创建信息
        /// </summary>
        /// <param name="entity">当前的DAO实例</param>
        public DbEntityInfo(object entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entity = entity;
        }
        /// <summary>
        /// 获取一个值表示是否接授当前操作
        /// </summary>
        public bool Accept { get; set; }
        /// <summary>
        /// 当前的DAO实例
        /// </summary>
        public object Entity { get; private set; }
    }
}
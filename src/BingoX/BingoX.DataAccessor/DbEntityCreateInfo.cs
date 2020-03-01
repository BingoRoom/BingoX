
using System;
using System.Collections.Generic;

namespace BingoX.DataAccessor
{
    /// <summary>
    /// 表示一个DAO的新增状态信息
    /// </summary>
    public class DbEntityCreateInfo : DbEntityInfo
    {
        /// <summary>
        /// 为当前新增的DAO创建一个新增状态信息实例
        /// </summary>
        /// <param name="entity">当前新增的DAO</param>
        /// <param name="currentValues">当前提交的DAO的属性值的字典</param>
        public DbEntityCreateInfo(object entity, IDictionary<string, object> currentValues) : base(entity)
        {
            if (currentValues == null)
            {
                throw new ArgumentNullException(nameof(currentValues));
            }

            CurrentValues = currentValues;
        }
        /// <summary>
        /// 当前提交的DAO的属性值的字典
        /// </summary>
        public IDictionary<string, object> CurrentValues { get; private set; }
        /// <summary>
        /// 设置当前DAO的属性值。被设置的属性将会成为当前新增后的DAO的值
        /// </summary>
        /// <param name="name">要设置DAO属性名</param>
        /// <param name="value">要设置DAO属性值</param>
        public virtual void SetValue(string name, object value)
        {
            if (CurrentValues.ContainsKey(name)) CurrentValues[name] = value;
        }
    }
}
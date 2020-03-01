
using System;
using System.Collections.Generic;

namespace BingoX.DataAccessor
{
    /// <summary>
    /// 表示一个DAO的修改状态信息
    /// </summary>
    public class DbEntityChangeInfo : DbEntityInfo
    {
        /// <summary>
        /// 为当前被修改的DAO创建一个修改状态信息实例
        /// </summary>
        /// <param name="entity">当前被修改的DAO</param>
        /// <param name="currentValues">修改后的DAO的属性值的字典</param>
        /// <param name="originalValues">修改前的DAO的属性值的字典</param>
        /// <param name="changeValues">当前被改动的DAO属性值的字典</param>
        public DbEntityChangeInfo(object entity, IDictionary<string, object> currentValues, IDictionary<string, object> originalValues, IDictionary<string, object> changeValues) : base(entity)
        {
            if (currentValues == null)
            {
                throw new ArgumentNullException(nameof(currentValues));
            }

            if (originalValues == null)
            {
                throw new ArgumentNullException(nameof(originalValues));
            }

            if (changeValues == null)
            {
                throw new ArgumentNullException(nameof(changeValues));
            }

            CurrentValues = currentValues;
            OriginalValues = originalValues;
            ChangeValues = changeValues;

        }
        /// <summary>
        /// 当前提交的DAO的属性值的字典
        /// </summary>
        public IDictionary<string, object> CurrentValues { get; private set; }
        /// <summary>
        /// 修改前的DAO的属性值的字典
        /// </summary>
        public IDictionary<string, object> OriginalValues { get; private set; }
        /// <summary>
        /// 当前被改动的DAO属性值的字典
        /// </summary>
        public IDictionary<string, object> ChangeValues { get; private set; }
        /// <summary>
        /// 设置当前DAO的属性值。被设置的属性将会成为当前更改后的DAO的值
        /// </summary>
        /// <param name="name">要设置DAO属性名</param>
        /// <param name="value">要设置DAO属性值</param>
        public virtual void SetValue(string name, object value)
        {
            if (ChangeValues.ContainsKey(name)) ChangeValues[name] = value;
            else if (CurrentValues.ContainsKey(name))
            {
                ChangeValues.Add(name, value);
            }
        }
    }
}
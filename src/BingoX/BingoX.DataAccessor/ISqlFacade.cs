using BingoX.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BingoX.DataAccessor
{
    /// <summary>
    /// 表示一个可通过SQL命令操作数据的接口
    /// </summary>
    public interface ISqlFacade
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlcommand">SQL命令</param>
        /// <returns></returns>
        T Query<T>(string sqlcommand) where T : class, IEntity<T>;
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlcommand">SQL命令</param>
        /// <returns></returns>
        IList<T> QueryList<T>(string sqlcommand) where T : class, IEntity<T>;
        /// <summary>
        /// 执行无返回结果的SQL命令
        /// </summary>
        /// <param name="sqlcommand">SQL命令</param>
        /// <returns></returns>
        int ExecuteNonQuery(string sqlcommand);
        /// <summary>
        /// 返回第一行第一列的指
        /// </summary>
        /// <param name="sqlcommand">SQL命令</param>
        /// <returns></returns>
        object ExecuteScalar(string sqlcommand);
        /// <summary>
        /// 以事务批量执行SQL命令
        /// </summary>
        /// <param name="sqlcommands">SQL命令</param>
        void TransactionExecute(IEnumerable<string> sqlcommands);
    }
}

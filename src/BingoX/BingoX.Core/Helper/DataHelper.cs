using BingoX.Utility;
using System.Data;

namespace BingoX.Helper 
{
    /// <summary>
    /// 提供一个针对System.Data空间下的类的辅助
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// 获取当前DataRow的指定字段的值，并转型为T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T Cast<T>(this DataRow row, string name)
        {
            var obj = row[name];
            return ObjectUtility.Cast<T>(obj);
        }
    }
}


using System;

namespace BingoX.DataAccessor
{
    /// <summary>
    /// 表示一个关于数据访问器的异常
    /// </summary>
    [Serializable]
    public class DataAccessorException : System.Data.Common.DbException
    {
        public DataAccessorException() { }
        public DataAccessorException(string message) : base(message) { }
        public DataAccessorException(string message, Exception inner) : base(message, inner) { }
        protected DataAccessorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
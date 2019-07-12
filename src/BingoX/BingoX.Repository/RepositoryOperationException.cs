using System;

namespace BingoX.Repository
{
    /// <summary>
    /// 表示在仓储操作过程中发生的异常信息
    /// </summary>
    [Serializable]
    public class RepositoryOperationException : Exception
    {
        public RepositoryOperationException() { }
        public RepositoryOperationException(string message) : base(message) { }
        public RepositoryOperationException(string message, Exception inner) : base(message, inner) { }
        protected RepositoryOperationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
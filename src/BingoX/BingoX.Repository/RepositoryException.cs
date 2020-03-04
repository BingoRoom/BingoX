using System;

namespace BingoX.Repository
{
    /// <summary>
    /// 表示在仓储操作过程中发生的异常信息
    /// </summary>
    [Serializable]
    public class RepositoryException : LogicException
    {
        public RepositoryException() { }
        public RepositoryException(string message) : base(message) { }
        public RepositoryException(string message, Exception inner) : base(message, inner) { }
        protected RepositoryException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
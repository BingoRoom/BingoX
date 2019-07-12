using System;

namespace BingoX.ComponentModel.DocumentManagementProvider
{
    /// <summary>
    /// 表示一个文档操作过程中产生的错误
    /// </summary>
    [Serializable]
    public class DocumentManagementException : Exception
    {
        public DocumentManagementException() { }
        public DocumentManagementException(string message) : base(message) { }
        public DocumentManagementException(string message, Exception inner) : base(message, inner) { }
        protected DocumentManagementException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

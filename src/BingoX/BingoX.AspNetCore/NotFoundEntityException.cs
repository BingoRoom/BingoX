using System;
using System.Runtime.Serialization;

namespace BingoX.AspNetCore
{
    [Serializable]
    public class NotFoundEntityException : LogicException
    {
        public NotFoundEntityException() : base("记录不存在")
        {
        }

        public NotFoundEntityException(string message) : base(message)
        {
        }

        public NotFoundEntityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotFoundEntityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

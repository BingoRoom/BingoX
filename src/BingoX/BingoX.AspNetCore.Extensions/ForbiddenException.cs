using System;

namespace BingoX.AspNetCore.Extensions
{
    [Serializable]
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base("无权访问") { }
        public ForbiddenException(string message) : base(message) { }
        public ForbiddenException(string message, Exception inner) : base(message, inner) { }
        protected ForbiddenException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

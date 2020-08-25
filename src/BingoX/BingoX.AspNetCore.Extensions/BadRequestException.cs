using System;
using System.Runtime.Serialization;
using BingoX;

namespace BingoX.AspNetCore.Extensions
{
    [Serializable]
    public class BadRequestException : Exception
    {
        public BadRequestException() : base("错误请求")
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

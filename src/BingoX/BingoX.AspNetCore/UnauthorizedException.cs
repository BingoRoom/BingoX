﻿using System;

namespace BingoX.AspNetCore
{
    [Serializable]
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("未登录") { }
        public UnauthorizedException(string message) : base(message) { }
        public UnauthorizedException(string message, Exception inner) : base(message, inner) { }
        protected UnauthorizedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

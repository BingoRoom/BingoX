﻿using System;
using System.Runtime.Serialization;

namespace BingoX
{
    [Serializable]
    public class LogicException : Exception
    {
        public LogicException()
        {
        }

        public LogicException(string message) : base(message)
        {
        }

        public LogicException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LogicException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.ComponentModel.Compress
{
    /// <summary>
    /// 表示在压缩、解压过程中出现的错误
    /// </summary>
    public class CompressException : Exception
    {
        public CompressException() { }
        public CompressException(string message) : base(message) { }
        public CompressException(string message, Exception inner) : base(message, inner) { }
        protected CompressException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}

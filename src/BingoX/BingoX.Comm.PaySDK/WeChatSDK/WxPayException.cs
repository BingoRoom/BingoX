using System;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    [Serializable]
    public class WxPayException : Exception
    {
        public WxPayException() { }
        public WxPayException(string message) : base(message) { }
        public WxPayException(string message, Exception inner) : base(message, inner) { }
        protected WxPayException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}






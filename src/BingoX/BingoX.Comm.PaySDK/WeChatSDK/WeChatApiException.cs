using System;
using System.Runtime.Serialization;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    [System.Serializable]
    public class WeChatApiException : Exception
    {
        public WeChatApiException() { }
        public WeChatApiException(string message) : base(message) { }

        internal WeChatApiException(WechatError error) : base(error.Message)
        {
            Code = error.Code;
        }
        public WeChatApiException(string message, int errcode) : this(message)
        {
            Code = errcode;
        }
        public int Code { get; private set; }
        public WeChatApiException(string message, System.Exception inner) : base(message, inner) { }
        protected WeChatApiException(SerializationInfo info, StreamingContext context) : base(info, context) { }


    }
}






using Newtonsoft.Json;
using System.Collections.Generic;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class WechatError
    {
        [JsonProperty("errcode")]
        public int Code { get; set; }
        [JsonProperty("errmsg")]
        public string Message { get; set; }
    }



}



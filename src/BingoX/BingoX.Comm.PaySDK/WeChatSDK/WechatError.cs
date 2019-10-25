using Newtonsoft.Json;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    internal class WechatError
    {
        [JsonProperty("errcode")]
        public int Code { get; set; }
        [JsonProperty("errmsg")]
        public string Message { get; set; }
    }





}



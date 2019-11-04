using Newtonsoft.Json;
using RestSharp;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class WeChatMessageApi
    {
        private readonly WeChatApi weChatApi;
        //https://developers.weixin.qq.com/doc/offiaccount/Message_Management/Receiving_standard_messages.html
        internal WeChatMessageApi(WeChatApi weChatApi)
        {
            this.weChatApi = weChatApi;
        }
    }
}








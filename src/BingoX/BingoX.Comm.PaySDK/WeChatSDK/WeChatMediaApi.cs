using RestSharp;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class WeChatMediaApi
    {
        private readonly WeChatApi weChatApi;
        //https://developers.weixin.qq.com/doc/offiaccount/Message_Management/Receiving_standard_messages.html
        internal WeChatMediaApi(WeChatApi weChatApi)
        {
            this.weChatApi = weChatApi;
        }


        public byte[] Get(string mediaID)
        {
            var accesstoken = weChatApi.GetAccessToken();
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", accesstoken.AccessToken, mediaID);
            var client = weChatApi.GetClient();
            var req = new RestRequest(url);

            var obj = client.Execute(req, Method.GET);
            if (obj.ContentType == "image/jpeg") return obj.RawBytes;
            var error = Newtonsoft.Json.JsonConvert.DeserializeObject<WechatError>(obj.Content);
            throw new WeChatApiException(error.GetMessage());
        }
        public BathchMaterial Batch(MaterialType type = MaterialType.Image)
        {
            var accesstoken = weChatApi.GetAccessToken();
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token={0}", accesstoken.AccessToken);
            var client = weChatApi.GetClient();
            var req = new RestRequest(url);
            req.AddJsonBody(new
            {
                type = type.ToString().ToLower(),
                offset = 0,
                count = 20
            });
            var obj = client.Execute<BathchMaterial>(req, Method.POST);
            return obj.Data;
        }
        public MaterialCount GetCount()
        {
            var accesstoken = weChatApi.GetAccessToken();
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/material/get_materialcount?access_token={0}", accesstoken.AccessToken);
            var client = weChatApi.GetClient();
            var req = new RestRequest(url);
            var obj = client.Execute<MaterialCount>(req, Method.GET);
            return obj.Data;
        }
    }
}








using Newtonsoft.Json;
using RestSharp;
using System.IO;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class WeChatUserApi
    {
        private readonly WeChatApi weChatApi;

        internal WeChatUserApi(WeChatApi weChatApi)
        {
            this.weChatApi = weChatApi;
        }
        public WeChatUserInfo GetUserInfo(string openid)
        {
            var accesstoken = weChatApi.GetAccessToken();
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", accesstoken.AccessToken, openid);
            RestClient client = weChatApi.GetClient();
            var req = new RestRequest(url);
            var obj = client.Execute<WeChatUserInfo>(req, Method.GET);
            return obj.Data;
        }
        public WeChatUserInfo GetUserInfoBySns(string openid)
        {
            var accesstoken = weChatApi.GetAccessTokenCache(openid);
            var url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", accesstoken.AccessToken, openid);
            RestClient client = weChatApi.GetClient();
            var req = new RestRequest(url);
            var obj = client.Execute<WeChatUserInfo>(req, Method.GET);
            return obj.Data;
        }
        public string GetOpenId(string code)
        {
            RestClient client = weChatApi.GetClient();

            var req = new RestRequest(WeChatConfig.SnsAccessTokenUrl);
            req.AddParameter(new Parameter("appid", weChatApi.Config.AppID, ParameterType.QueryString));
            req.AddParameter(new Parameter("secret", weChatApi.Config.AppSecret, ParameterType.QueryString));
            req.AddParameter(new Parameter("code", code, ParameterType.QueryString));
            req.AddParameter(new Parameter("grant_type", "authorization_code", ParameterType.QueryString));
            var obj = client.Execute(req, Method.GET);
            JsonReader reader = new JsonTextReader(new StringReader(obj.Content));
            string openid = string.Empty;
            while (reader.Read())
            {
                if (string.Equals(reader.Path, "openid"))
                {
                    openid = reader.ReadAsString();
                    break;
                }
            }
            reader.Close();
            return openid;
        }
    }
}








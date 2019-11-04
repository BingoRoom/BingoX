

using Newtonsoft.Json;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class WechatSnsToken : IAccessToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("openid")]
        public string OpenId { get; set; }
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }


        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        string IAccessToken.UserId { get { return OpenId; } set { OpenId = value; } }

    }


}



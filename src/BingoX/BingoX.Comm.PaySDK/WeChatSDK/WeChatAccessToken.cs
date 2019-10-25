

using Newtonsoft.Json;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class WeChatAccessToken : IAccessToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }

    }





}



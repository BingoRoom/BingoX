namespace BingoX.Comm.PaySDK.AlipaySDK
{
    public class AlipayConfig
    {
        public AlipayConfig()
        {
            ServiceUrl = "https://openapi.alipay.com/gateway.do";
            GrantType = "authorization_code";
        }

        public AlipayConfig(string appID, string publicKey, string privateKey) : this()
        {
            AppID = appID;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
        public AlipayConfig(string appID, string publicKey, string privateKey, string serviceUrl, string grantType)
        {
            AppID = appID;
            PublicKey = publicKey;
            PrivateKey = privateKey;
            ServiceUrl = serviceUrl;
            GrantType = grantType;
        }


        public string AppID { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }

        public string ServiceUrl { get; set; }
        public string GrantType { get; set; }

        internal const string oauth2 = "https://openauth.alipay.com/oauth2/publicAppAuthorize.htm?";
    }
}




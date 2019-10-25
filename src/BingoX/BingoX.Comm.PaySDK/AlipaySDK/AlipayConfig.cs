namespace BingoX.Comm.PaySDK.AlipapSDK
    {
        public class AlipayConfig
        {
            public AlipayConfig()
            {
                ServiceUrl = "https://openapi.alipay.com/gateway.do";
            }
            public string AppID { get; set; }
            public string PrivateKey { get; set; }
            public string PublicKey { get; set; }

            public string ServiceUrl { get; set; }
            public string GrantType { get; set; }

            internal const string oauth2 = "https://openauth.alipay.com/oauth2/publicAppAuthorize.htm?";
        }
    }

 


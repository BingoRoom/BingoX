using System.Collections.Specialized;
using System.Xml;

namespace BingoX.Comm.PaySDK.WeChatSDK
{

    public class WeChatConfig
    {


        public WeChatConfig(string appID, string appSecret, string grantType, string verifyContent, string verifyName)
        {
            AppID = appID;
            AppSecret = appSecret;
            GrantType = grantType;
            VerifyContent = verifyContent;
            VerifyName = verifyName;
        }
        public WeChatConfig(string appID, string appSecret)
        {
            AppID = appID;
            AppSecret = appSecret;
            GrantType = "client_credential";
        }

        public string AppID { get; private set; }
        public string AppSecret { get; private set; }
        public string VerifyName { get; private set; }
        public string VerifyContent { get; private set; }
        public string GrantType { get; private set; }

        internal const string SnsAccessTokenUrl = "https://api.weixin.qq.com/sns/oauth2/access_token";
        internal const string CgiBinTokenUrl = "https://api.weixin.qq.com/cgi-bin/token";
        internal const string ScantTitleUrl = "https://api.weixin.qq.com/card/invoice/scantitle";
        internal const string GetTicketUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket";

        internal const string oauth2 = "https://open.weixin.qq.com/connect/oauth2/authorize?";
    }
}





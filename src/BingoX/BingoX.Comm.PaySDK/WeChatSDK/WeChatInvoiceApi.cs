using RestSharp;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class WeChatInvoiceApi
    {
        private readonly WeChatApi weChatApi;

        internal WeChatInvoiceApi(WeChatApi weChatApi)
        {
            this.weChatApi = weChatApi;
        }
        public string ScanTitle(string content)
        {
            var AccessToken = weChatApi.GetAccessToken();
            if (AccessToken == null) throw new WxPayException("没登陆授权");
            RestClient client = weChatApi.GetClient();
            var req = new RestRequest(WeChatConfig.ScantTitleUrl);
            req.AddParameter(new Parameter("access_token", AccessToken.AccessToken, ParameterType.QueryString));
            //    req.AddParameter("scan_text", content, ParameterType.RequestBody);
            req.AddJsonBody(new { scan_text = content });
            var obj = client.Execute(req, Method.POST);
            return obj.Content;
        }
    }
}








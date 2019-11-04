using Newtonsoft.Json;
using RestSharp;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class WeChatOCRApi
    {
        private readonly WeChatApi weChatApi;
        internal WeChatOCRApi(WeChatApi weChatApi)
        {
            this.weChatApi = weChatApi;
        }
        //https://developers.weixin.qq.com/doc/offiaccount/Intelligent_Interface/OCR.html

        public OCRIDCard IDCard(string fileUrl)
        {
            var accesstoken = weChatApi.GetAccessToken();
            var url = string.Format("https://api.weixin.qq.com/cv/ocr/idcard?img_url={1}&access_token={0}", accesstoken.AccessToken, System.Web.HttpUtility.UrlEncode(fileUrl));
            var client = weChatApi.GetClient();
            var req = new RestRequest(url);

            var obj = client.Execute<OCRIDCard>(req, Method.GET);
            return obj.Data;

        }
        public OCRBankCard BankCard(string fileUrl)
        {
            var accesstoken = weChatApi.GetAccessToken();
            var url = string.Format("https://api.weixin.qq.com/cv/ocr/bankcard?img_url={1}&access_token={0}", accesstoken.AccessToken, System.Web.HttpUtility.UrlEncode(fileUrl));
            var client = weChatApi.GetClient();
            var req = new RestRequest(url);

            var obj = client.Execute<OCRBankCard>(req, Method.GET);
            return obj.Data;

        }

        public OCRDriving Driving(string fileUrl)
        {
            var accesstoken = weChatApi.GetAccessToken();
            var url = string.Format("https://api.weixin.qq.com/cv/ocr/driving?img_url={1}&access_token={0}", accesstoken.AccessToken, System.Web.HttpUtility.UrlEncode(fileUrl));
            var client = weChatApi.GetClient();
            var req = new RestRequest(url);

            var obj = client.Execute<OCRDriving>(req, Method.GET);
            return obj.Data;

        }
        public OCRDrivingLicense DrivingLicense(string fileUrl)
        {
            var accesstoken = weChatApi.GetAccessToken();
            var url = string.Format("https://api.weixin.qq.com/cv/ocr/drivinglicense?img_url={1}&access_token={0}", accesstoken.AccessToken, System.Web.HttpUtility.UrlEncode(fileUrl));
            var client = weChatApi.GetClient();
            var req = new RestRequest(url);

            var obj = client.Execute<OCRDrivingLicense>(req, Method.GET);
            return obj.Data;

        }
        public OCRBizlicense Bizlicense(string fileUrl)
        {
            var accesstoken = weChatApi.GetAccessToken();
            var url = string.Format("https://api.weixin.qq.com/cv/ocr/bizlicense?img_url={1}&access_token={0}", accesstoken.AccessToken, System.Web.HttpUtility.UrlEncode(fileUrl));
            var client = weChatApi.GetClient();
            var req = new RestRequest(url);

            var obj = client.Execute<OCRBizlicense>(req, Method.GET);
            return obj.Data;

        }
        //public OCRIDCard Text(string fileUrl)
        //{
        //    var accesstoken = weChatApi.GetAccessToken();
        //    var url = string.Format("https://api.weixin.qq.com/cv/ocr/comm?img_url={1}&access_token={0}", accesstoken.AccessToken, System.Web.HttpUtility.UrlEncode(fileUrl));
        //    var client = weChatApi.GetClient();
        //    var req = new RestRequest(url);

        //    var obj = client.Execute<OCRIDCard>(req, Method.GET);
        //    return obj.Data;

        //}
    }
}








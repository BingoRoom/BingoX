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

        public void TemplateMessage(string openId, string template_id, string form_id, object data)
        {
            throw new System.NotSupportedException();
            //RestClient client = new RestClient();
            //var url = string.Format("https://api.weixin.qq.com/cgi-bin/message/wxopen/template/send?access_token={0}", weChatApi.GetAccessToken().AccessToken);
            //var request = new RestRequest(url);
            //request.AddJsonBody(new { touser = openId, template_id = template_id, form_id = form_id });
            //var response = client.Execute<WechatError>(request, RestSharp.Method.POST);
            //if (response.Data != null && response.Data.Code != 0) throw new WeChatApiException(response.Data.GetMessage());

        }  
        public void SubscribeMessageMessage(string openId, string template_id,   object data)
        {
            throw new System.NotSupportedException();
            //RestClient client = new RestClient();
            //var url = string.Format("https://api.weixin.qq.com/cgi-bin/message/subscribe/send?access_token={0}", weChatApi.GetAccessToken().AccessToken);
            //var request = new RestRequest(url);
            //request.AddJsonBody(new { touser = openId, template_id = template_id, data = data });
            //var response = client.Execute<WechatError>(request, RestSharp.Method.POST);
            //if (response.Data != null && response.Data.Code != 0) throw new WeChatApiException(response.Data.GetMessage());

        }
        public void UniformMessage(string openId, string template_id, object weapp_template_msg, object mp_template_msg)
        {
            throw new System.NotSupportedException();
            //RestClient client = new RestClient();
            //var url = string.Format("https://api.weixin.qq.com/cgi-bin/message/wxopen/template/uniform_send?access_token={0}", weChatApi.GetAccessToken().AccessToken);
            //var request = new RestRequest(url);
            //request.AddJsonBody(new { touser = openId, template_id = template_id, form_id = form_id });
            //var response = client.Execute<WechatError>(request, RestSharp.Method.POST);
            //if (response.Data != null && response.Data.Code != 0) throw new WeChatApiException(response.Data.GetMessage());

        }
        public void Text(string openId, string message)
        {
            RestClient client = new RestClient();
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", weChatApi.GetAccessToken().AccessToken);
            var request = new RestRequest(url);
            request.AddJsonBody(new { touser = openId, msgtype = "text", text = new { content = message } });
            var response = client.Execute<WechatError>(request, RestSharp.Method.POST);
            if (response.Data != null && response.Data.Code != 0) throw new WeChatApiException(response.Data.GetMessage());


        }

        public void Image(string openId, string mediaId)
        {
            RestClient client = new RestClient();
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", weChatApi.GetAccessToken().AccessToken);
            var request = new RestRequest(url);
            request.AddJsonBody(new { touser = openId, msgtype = "image", image = new { media_id = mediaId } });
            var response = client.Execute<WechatError>(request, RestSharp.Method.POST);

            if (response.Data != null && response.Data.Code != 0) throw new WeChatApiException(response.Data.GetMessage());
        }

        public void Link(string openId, string title, string description, string url, string thumb_url)
        {
            RestClient client = new RestClient();
            var apiurl = string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", weChatApi.GetAccessToken().AccessToken);
            var request = new RestRequest(apiurl);
            request.AddJsonBody(new { touser = openId, msgtype = "link", link = new { title = title, description = description, url = url, thumb_url = thumb_url } });
            var response = client.Execute<WechatError>(request, RestSharp.Method.POST);
            if (response.Data != null && response.Data.Code != 0) throw new WeChatApiException(response.Data.GetMessage());

        }
        public void Typing(string openId)
        {
            Typing(openId, true);

        }
        public void CancelTyping(string openId)
        {
            Typing(openId, false);

        }

        private void Typing(string openId, bool isTyping)
        {
            RestClient client = new RestClient();
            var apiurl = string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/typing?access_token={0}", weChatApi.GetAccessToken().AccessToken);
            var request = new RestRequest(apiurl);
            request.AddJsonBody(new { touser = openId, command = isTyping ? "Typing" : "CancelTyping" });
            var response = client.Execute<WechatError>(request, RestSharp.Method.POST);
            if (response.Data != null && response.Data.Code != 0) throw new WeChatApiException(response.Data.GetMessage());
        }
    }
}








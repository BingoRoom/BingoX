﻿
using BingoX.Cache;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    internal class NewtonsoftJsonSerializer : IDeserializer
    {






        public T Deserialize<T>(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
    public class WeChatApi : IPaySDKApi
    {

        public WeChatApi(WeChatConfig config, ICacheManager cacheManager)
        {
            Config = config;
            this.cacheManager = cacheManager;
        }
        static readonly IDeserializer deserializer = new NewtonsoftJsonSerializer();
        private readonly ICacheManager cacheManager;
        //     public WeChatAccessToken AccessToken { get;private set; }
        public WeChatConfig Config { get; set; }
        public WeChatUserInfo GetUserInfo(string openid)
        {
            var accesstoken = GetAccessToken();
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", accesstoken.AccessToken, openid);
            RestClient client = GetClient();
            var req = new RestRequest(url);
            var obj = client.Execute<WeChatUserInfo>(req, Method.GET);
            return obj.Data;
        }
        public WeChatUserInfo GetUserInfoBySns(string openid)
        {
            var accesstoken = GetAccessTokenCache(openid);
            var url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", accesstoken.AccessToken, openid);
            RestClient client = GetClient();
            var req = new RestRequest(url);
            var obj = client.Execute<WeChatUserInfo>(req, Method.GET);
            return obj.Data;
        }
        IAccessToken IPaySDKApi.GetAccessToken(string auth_code)
        {
            var token = GetSnsAccessToken(auth_code);

            return token;
        }
        IUserInfo IPaySDKApi.GetUserInfo(string openid)
        {
            return GetUserInfo(openid);
        }

        /// <summary>
        /// 获取微信接口调用全局票据
        /// </summary>
        public WechatSnsToken GetSnsAccessToken(string auth_code)
        {
            RestClient client = GetClient();

            var req = new RestRequest(WeChatConfig.SnsAccessTokenUrl);
            req.AddParameter(new Parameter("appid", Config.AppID, ParameterType.QueryString));
            req.AddParameter(new Parameter("secret", Config.AppSecret, ParameterType.QueryString));
            req.AddParameter(new Parameter("grant_type", "authorization_code", ParameterType.QueryString));
            req.AddParameter(new Parameter("code", auth_code, ParameterType.QueryString));
            var obj = client.Execute<WechatSnsToken>(req, Method.GET);
            if (obj.Data == null || string.IsNullOrEmpty(obj.Data.AccessToken))
            {

                var error = JsonConvert.DeserializeObject<WechatError>(obj.Content);

                throw new WeChatApiException(error);
            }
            var key = "WeChatSnsAccessToken" + obj.Data.OpenId;
            cacheManager.Add(key, new CacheItem<WechatSnsToken>(key, obj.Data, ExpirationMode.Absolute, TimeSpan.FromSeconds(obj.Data.ExpiresIn)));
            return obj.Data;
        }

        private static RestClient GetClient()
        {
            var client = new RestClient();
            client.AddHandler(() => deserializer, "application/json", "json", "text/plain");
            return client;
        }

        WechatSnsToken GetAccessTokenCache(string openid)
        {
            var key = "WeChatSnsAccessToken" + openid;
            var cacheitem = cacheManager.Get<WechatSnsToken>(key);
            return cacheitem;
        }
        /// <summary>
        /// 获取微信接口调用全局票据
        /// </summary>
        public WeChatAccessToken GetAccessToken()
        {
            const string key = "WeChatAccessToken";
            var cacheitem = cacheManager.Get<WeChatAccessToken>(key);
            if (cacheitem != null) return cacheitem;
            if (Config == null) throw new WxPayException("没有配置");

            RestClient client = GetClient();

            var req = new RestRequest(WeChatConfig.CgiBinTokenUrl);
            req.AddParameter(new Parameter("appid", Config.AppID, ParameterType.QueryString));
            req.AddParameter(new Parameter("secret", Config.AppSecret, ParameterType.QueryString));
            req.AddParameter(new Parameter("grant_type", Config.GrantType, ParameterType.QueryString));
            var obj = client.Execute<WeChatAccessToken>(req, Method.GET);
            if (obj.Data == null || string.IsNullOrEmpty(obj.Data.AccessToken))
            {
                cacheManager.Remove(key);
                var error = JsonConvert.DeserializeObject<WechatError>(obj.Content);
                if (error.Code == 40001)
                {
                    cacheManager.Remove(key);

                }
                throw new WeChatApiException(error);
            }

            cacheManager.Add(key, new CacheItem<WeChatAccessToken>(key, obj.Data, ExpirationMode.Absolute, TimeSpan.FromSeconds(obj.Data.ExpiresIn)));
            return obj.Data;
        }

        public string GetAuthURL(string redirectUrl, IDictionary<string, string> querys = null)
        {
            string stringQuery = string.Empty;
            if (querys != null) stringQuery = string.Join("&", querys.Select(n => string.Format("{0}={1}", n.Key, System.Web.HttpUtility.UrlEncode(n.Value))));


            var tmprul = redirectUrl;
            if (string.IsNullOrEmpty(stringQuery)) tmprul = string.Format("{0}{1}{2}", redirectUrl, redirectUrl.Contains("?") ? "&" : "?", stringQuery);
            string redirect = System.Web.HttpUtility.UrlEncode(tmprul);
            string url = string.Format("{0}appid={1}&redirect_uri={2}&response_type=code&scope=snsapi_base&state=123#wechat_redirect", WeChatConfig.oauth2, Config.AppID, redirect);
            //response_type=code&scope=snsapi_userinfo&state=1&connect_redirect=1#wechat_redirect
            return url;
        }
        public string GetAuthSnsURL(string redirectUrl, IDictionary<string, string> querys = null)
        {
            string stringQuery = string.Empty;
            if (querys != null) stringQuery = string.Join("&", querys.Select(n => string.Format("{0}={1}", n.Key, System.Web.HttpUtility.UrlEncode(n.Value))));


            var tmprul = redirectUrl;
            if (string.IsNullOrEmpty(stringQuery)) tmprul = string.Format("{0}{1}{2}", redirectUrl, redirectUrl.Contains("?") ? "&" : "?", stringQuery);
            string redirect = System.Web.HttpUtility.UrlEncode(tmprul);
            string url = string.Format("{0}appid={1}&redirect_uri={2}&response_type=code&scope=snsapi_userinfo&state=1&connect_redirect=1#wechat_redirect", WeChatConfig.oauth2, Config.AppID, redirect);

            return url;
        }

        public string GetJsSdk(string url)
        {
            var res = new Hashtable();
            WeChatJsSdk jsSdk = new WeChatJsSdk(this.GetTicket(), url);

            jsSdk.Sign();


            res.Add("appId", Config.AppID);

            res.Add("timestap", jsSdk.Timestamp);

            res.Add("nonceStr", jsSdk.Noncestr);

            res.Add("signature", jsSdk.Signature);
            res.Add("ticket", jsSdk.JsapiTicket);

            var result = JsonConvert.SerializeObject(res);
            return result;
        }



        public string GetTicket()
        {
            var AccessToken = GetAccessToken();
            if (AccessToken == null) throw new WxPayException("没登陆授权");
            const string key = "WeChatTicket";
            var cacheitem = cacheManager.Get<WeChatTicket>(key);
            if (cacheitem != null) return cacheitem.Ticket;
            RestClient client = GetClient();
            var req = new RestRequest(WeChatConfig.GetTicketUrl);
            req.AddParameter(new Parameter("access_token", AccessToken.AccessToken, ParameterType.QueryString));
            req.AddParameter(new Parameter("type", "jsapi", ParameterType.QueryString));

            var obj = client.Execute<WeChatTicket>(req, Method.GET);

            if (obj.Data != null && !string.IsNullOrEmpty(obj.Data.Ticket))
            {
                cacheManager.Add(key, new CacheItem<WeChatTicket>(key, obj.Data, ExpirationMode.Absolute, TimeSpan.FromSeconds(obj.Data.ExpiresIn)));
                return obj.Data.Ticket;
            }

            cacheManager.Remove(key);

            return string.Empty;
        }
        public string GetOpenId(string code)
        {
            RestClient client = GetClient();

            var req = new RestRequest(WeChatConfig.SnsAccessTokenUrl);
            req.AddParameter(new Parameter("appid", Config.AppID, ParameterType.QueryString));
            req.AddParameter(new Parameter("secret", Config.AppSecret, ParameterType.QueryString));
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


        public string ScanTitle(string content)
        {
            var AccessToken = GetAccessToken();
            if (AccessToken == null) throw new WxPayException("没登陆授权");
            RestClient client = GetClient();
            var req = new RestRequest(WeChatConfig.ScantTitleUrl);
            req.AddParameter(new Parameter("access_token", AccessToken.AccessToken, ParameterType.QueryString));
            //    req.AddParameter("scan_text", content, ParameterType.RequestBody);
            req.AddJsonBody(new { scan_text = content });
            var obj = client.Execute(req, Method.POST);
            return obj.Content;
        }
    }
}








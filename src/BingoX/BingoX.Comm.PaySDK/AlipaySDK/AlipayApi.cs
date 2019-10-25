using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using BingoX.Cache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BingoX.Comm.PaySDK.AlipapSDK
{
    public class AlipayApi : IPaySDKApi
    {
        private readonly ICacheManager cacheManager;

        public AlipayApi(AlipayConfig config, ICacheManager cacheManager)
        {
            Config = config;
            this.cacheManager = cacheManager;
        }
        public AlipayConfig Config { get; private set; }

        public string GetAuthURL(string redirectUrl, IDictionary<string, string> querys = null)
        {
            string stringQuery = string.Empty;
            if (querys != null) stringQuery = string.Join("&", querys.Select(n => string.Format("{0}={1}", n.Key, System.Web.HttpUtility.UrlEncode(n.Value))));

            var tmprul = redirectUrl;
            if (string.IsNullOrEmpty(stringQuery)) tmprul = string.Format("{0}{1}{2}", redirectUrl, redirectUrl.Contains("?") ? "&" : "?", stringQuery);
            string redirect = System.Web.HttpUtility.UrlEncode(tmprul);

            string url = string.Format("{0}app_id={1}&scope=auth_invoice_info&redirect_uri={2}", AlipayConfig.oauth2, Config.AppID, redirect);
            return url;
        }
        private IAopClient CreateClient()
        {
            return new DefaultAopClient(Config.ServiceUrl, Config.AppID, Config.PrivateKey, "json", "UTF-8", "RSA2", Config.PublicKey);
        }
        public AlipayOauthToken GetAccessToken(string auth_code)
        {

            AlipaySystemOauthTokenRequest alipayRequest = new AlipaySystemOauthTokenRequest();
            alipayRequest.Code = auth_code;
            alipayRequest.GrantType = Config.GrantType;
            IAopClient client = CreateClient();
            AlipaySystemOauthTokenResponse oauthTokenResponse = client.Execute(alipayRequest);
            var key = "AlipayOauthToken" + oauthTokenResponse.UserId;
            var oauthToken = new AlipayOauthToken { AccessToken = oauthTokenResponse.AccessToken, UserId = oauthTokenResponse.UserId, ExpiresIn = 3600 };
            cacheManager.Add(key, new CacheItem<AlipayOauthToken>(key, oauthToken, ExpirationMode.Absolute, TimeSpan.FromSeconds(oauthToken.ExpiresIn)));
            return oauthToken;
        }

        public AlipayUserUserinfoShareResponse GetUserInfo(string userid)
        {
            var key = "AlipayOauthToken" + userid;
            var oauthToken = cacheManager.Get<AlipayOauthToken>(key);
            if (oauthToken == null) throw new LogicException("未授权");
            IAopClient client = CreateClient();
            AlipayUserUserinfoShareRequest alipayRequest = new AlipayUserUserinfoShareRequest();
 
            AlipayUserUserinfoShareResponse response = client.Execute(alipayRequest, oauthToken.AccessToken);
            return response;

        }
        IAccessToken IPaySDKApi.GetAccessToken(string auth_code)
        {
            return GetAccessToken(auth_code);
        }

        IUserInfo IPaySDKApi.GetUserInfo(string userid)
        {
            return GetUserInfo(userid);
        }
    }
}




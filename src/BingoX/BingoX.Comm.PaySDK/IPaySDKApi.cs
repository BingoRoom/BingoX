using System.Collections.Generic;

namespace BingoX.Comm.PaySDK
{
    public interface IPaySDKApi
    {
        IAccessToken GetAccessToken(string auth_code = null);
        string GetAuthURL(string redirectUrl, IDictionary<string, string> querys = null);

        IUserInfo GetUserInfo(string openid);
    }
}








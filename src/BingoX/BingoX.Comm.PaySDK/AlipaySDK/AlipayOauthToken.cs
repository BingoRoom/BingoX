namespace BingoX.Comm.PaySDK.AlipapSDK
{
    public class AlipayOauthToken : IAccessToken
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }

        public long ExpiresIn { get; set; }
    }
}




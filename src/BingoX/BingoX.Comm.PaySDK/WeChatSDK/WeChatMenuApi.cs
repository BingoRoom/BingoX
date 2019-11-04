namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class WeChatMenuApi
    {
        private readonly WeChatApi weChatApi;
        //https://developers.weixin.qq.com/doc/offiaccount/Custom_Menus/Creating_Custom-Defined_Menu.html
        //https://developers.weixin.qq.com/doc/offiaccount/Custom_Menus/Custom_Menu_Push_Events.html
        // https://api.weixin.qq.com/cgi-bin/menu/create?access_token=ACCESS_TOKEN
        internal WeChatMenuApi(WeChatApi weChatApi)
        {
            this.weChatApi = weChatApi;
        }
    }  
}








using Newtonsoft.Json;
using System;



namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class WeChatUserInfo : IWeChatUserInfo, IUserInfo
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("subscribe")]
        public int Subscribe { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("nickname")]
        public string NickName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("openid")]
        public string OpenId { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        [JsonProperty("sex")]
        public int Sex { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// </summary>
        [JsonProperty("headimgurl")]
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("province")]
        public string Province { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }
        /// <summary>
        /// 用户所在的分组ID（兼容旧的用户分组接口）
        /// </summary>
        [JsonProperty("groupid")]
        public string Groupid { get; set; }
        /// <summary>
        /// 用户被打上的标签ID列表
        /// </summary>
        [JsonProperty("tagid_list")]
        public int[] TagidList { get; set; }
        /// <summary>
        /// 返回用户关注的渠道来源，
        /// ADD_SCENE_SEARCH 公众号搜索，
        /// ADD_SCENE_ACCOUNT_MIGRATION 公众号迁移，
        /// ADD_SCENE_PROFILE_CARD 名片分享，
        /// ADD_SCENE_QR_CODE 扫描二维码，
        /// ADD_SCENE_PROFILE_ LINK 图文页内名称点击，
        /// ADD_SCENE_PROFILE_ITEM 图文页右上角菜单，
        /// ADD_SCENE_PAID 支付后关注，
        /// ADD_SCENE_OTHERS 其他
        /// </summary>
        [JsonProperty("subscribe_scene")]
        public string SubscribeScene { get; set; }
        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        [JsonProperty("subscribe_time")]
        public string SubscribeTime { get; set; }
        string IUserInfo.Avatar { get => HeadImgUrl;   }
        string IUserInfo.Gender { get => Sex.ToString();  }
    }
}

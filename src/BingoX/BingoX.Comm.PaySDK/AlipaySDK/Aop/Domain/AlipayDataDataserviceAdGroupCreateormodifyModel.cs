using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Aop.Api.Domain
{
    /// <summary>
    /// AlipayDataDataserviceAdGroupCreateormodifyModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayDataDataserviceAdGroupCreateormodifyModel : AopObject
    {
        /// <summary>
        /// 灯火平台提供给外部系统的访问token
        /// </summary>
        [XmlElement("biz_token")]
        public string BizToken { get; set; }

        /// <summary>
        /// 业务扩展参数字段，根据第三方需要使用，投放端只做存储并向检索端透传
        /// </summary>
        [XmlElement("extend_info")]
        public string ExtendInfo { get; set; }

        /// <summary>
        /// 单元出价，单位：分
        /// </summary>
        [XmlElement("group_charge")]
        public long GroupCharge { get; set; }

        /// <summary>
        /// 单元名称
        /// </summary>
        [XmlElement("group_name")]
        public string GroupName { get; set; }

        /// <summary>
        /// 外部单元编号
        /// </summary>
        [XmlElement("group_outer_id")]
        public string GroupOuterId { get; set; }

        /// <summary>
        /// 商品id列表
        /// </summary>
        [XmlArray("item_id_list")]
        [XmlArrayItem("string")]
        public List<string> ItemIdList { get; set; }

        /// <summary>
        /// 外部计划编号
        /// </summary>
        [XmlElement("plan_outer_id")]
        public string PlanOuterId { get; set; }

        /// <summary>
        /// 单元定向列表
        /// </summary>
        [XmlArray("targeting_list")]
        [XmlArrayItem("outer_targeting_item")]
        public List<OuterTargetingItem> TargetingList { get; set; }

        /// <summary>
        /// 投放区间内的投放时段
        /// </summary>
        [XmlElement("time_option")]
        public string TimeOption { get; set; }
    }
}

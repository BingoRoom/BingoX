using System;
using System.Xml.Serialization;

namespace Aop.Api.Domain
{
    /// <summary>
    /// AlipayCommerceEducateFacefeatureReportUploadModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayCommerceEducateFacefeatureReportUploadModel : AopObject
    {
        /// <summary>
        /// 图片encode base64 String
        /// </summary>
        [XmlElement("auth_img")]
        public string AuthImg { get; set; }

        /// <summary>
        /// 学校团餐
        /// </summary>
        [XmlElement("biz_code")]
        public string BizCode { get; set; }

        /// <summary>
        /// 商户机具编码
        /// </summary>
        [XmlElement("device_num")]
        public string DeviceNum { get; set; }

        /// <summary>
        /// 机构ID
        /// </summary>
        [XmlElement("inst_id")]
        public string InstId { get; set; }

        /// <summary>
        /// 合作商户简称
        /// </summary>
        [XmlElement("isv_name")]
        public string IsvName { get; set; }

        /// <summary>
        /// 场景码
        /// </summary>
        [XmlElement("scene_code")]
        public string SceneCode { get; set; }

        /// <summary>
        /// 商户透传人脸客户端信息
        /// </summary>
        [XmlElement("z_face_info")]
        public string ZFaceInfo { get; set; }
    }
}

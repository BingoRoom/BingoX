using System;
using System.Xml.Serialization;
using Aop.Api.Domain;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobileBeaconDeviceQueryResponse.
    /// </summary>
    public class AlipayMobileBeaconDeviceQueryResponse : AopResponse
    {
        /// <summary>
        /// 蓝牙设备信息
        /// </summary>
        [XmlElement("beacon_device_info")]
        public BeaconDeviceInfo BeaconDeviceInfo { get; set; }

      
    }
}

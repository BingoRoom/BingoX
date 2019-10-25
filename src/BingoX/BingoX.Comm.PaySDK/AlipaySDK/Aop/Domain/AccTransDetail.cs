using System;
using System.Xml.Serialization;

namespace Aop.Api.Domain
{
    /// <summary>
    /// AccTransDetail Data Structure.
    /// </summary>
    [Serializable]
    public class AccTransDetail : AopObject
    {
        /// <summary>
        /// 支付宝订单号。仅付汇失败后，商户重试时填写。首次批次请求时设置为空，否则会失败。    biz_scene=LOCAL时忽略该参数。
        /// </summary>
        [XmlElement("alipay_order_no")]
        public string AlipayOrderNo { get; set; }

        /// <summary>
        /// 收款方身份认证信息。biz_scene=LOCAL时忽略该参数。
        /// </summary>
        [XmlElement("cert_info")]
        public CertInfo CertInfo { get; set; }

        /// <summary>
        /// 明细流水号
        /// </summary>
        [XmlElement("detail_no")]
        public string DetailNo { get; set; }

        /// <summary>
        /// 代发明细原始交易信息
        /// </summary>
        [XmlElement("ori_txn_info")]
        public OriTxnInfo OriTxnInfo { get; set; }

        /// <summary>
        /// 收款方信息
        /// </summary>
        [XmlElement("payee_info")]
        public AccPayeeInfo PayeeInfo { get; set; }

        /// <summary>
        /// 资金到账时效。biz_scene=LOCAL时忽略该参数。
        /// </summary>
        [XmlElement("reach_time")]
        public string ReachTime { get; set; }

        /// <summary>
        /// 转账明细备注。
        /// </summary>
        [XmlElement("remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 结算金额。biz_scene=LOCAL时忽略该参数。
        /// </summary>
        [XmlElement("settlement_currency")]
        public string SettlementCurrency { get; set; }

        /// <summary>
        /// 转账金额
        /// </summary>
        [XmlElement("trans_amount")]
        public string TransAmount { get; set; }

        /// <summary>
        /// 转账币种, 用来修饰转账金额 trans_amount. biz_scene=LOCAL时忽略该参数。
        /// </summary>
        [XmlElement("trans_currency")]
        public string TransCurrency { get; set; }
    }
}

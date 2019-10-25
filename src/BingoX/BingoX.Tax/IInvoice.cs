using System.Collections.Generic;

namespace BingoX.Tax
{
    public interface IInvoice
    {
        /// <summary>
        /// 发票类型
        /// s:增值税专用发票
        /// c:增值税普通发票
        /// d:增值税普通电子发票
        /// </summary>
        string InvoiceType { get; set; }
        /// <summary>
        /// 发票代码
        /// </summary>
        string InvoiceCode { get; set; }
        /// <summary>
        /// 发票号码
        /// </summary>
        string InvoiceNumber { get; set; }

        /// <summary>
        /// 合计不含税金额
        /// </summary>
        decimal SumAmount { get; set; }
        /// <summary>
        /// 合计含税金额
        /// </summary>
        decimal SumAmountTax { get; set; }
        /// <summary>
        /// 合计税额
        /// </summary>
        decimal SumTax { get; set; }
        /// <summary>
        /// 发票备注
        /// </summary>
        string Memo { get; set; }
        /// <summary>
        /// 开票人
        /// </summary>
        string Invoicer { get; set; }
        /// <summary>
        /// 复核人
        /// </summary>
        string Recheckr { get; set; }
        /// <summary>
        /// 购方企业类型
        /// 1:企业
        /// 2:个人
        /// </summary>
        int BuyerType { get; set; }
        /// <summary>
        /// 购方信息
        /// </summary>
        IClientInfo Buyer { get; set; }
        /// <summary>
        /// 销方信息
        /// </summary>
        IClientInfo Sale { get; set; }
        /// <summary>
        /// 开票明细
        /// </summary>
        ICollection<IInvoiceDetail> Details { get; set; }

        /// <summary>
        /// 此发票是否为红票
        /// </summary>
        bool IsRed { get; set; }
        /// <summary>
        /// 被冲红的原票信息
        /// </summary>
        IBlueInvoice BlueInvoice { get; set; }

        /// <summary>
        /// 蓝票情况被冲红标志。（0正常，1已冲红）
        /// </summary>
        bool RedFlag { get; set; }
    }
}

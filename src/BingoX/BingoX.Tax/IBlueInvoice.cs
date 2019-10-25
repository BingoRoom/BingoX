using System;

namespace BingoX.Tax
{
    public interface IBlueInvoice
    {
        /// <summary>
        /// 原发票代码
        /// </summary>
        string InvoiceCode { get; set; }
        /// <summary>
        /// 原发票号码
        /// </summary>
        string InvoiceNumber { get; set; }
        /// <summary>
        /// 冲红原因
        /// </summary>
        string RedReason { get; set; }
    }
}

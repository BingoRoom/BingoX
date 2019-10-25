namespace BingoX.Tax
{
    public interface ISourceInvoiceDetail
    {
        /// <summary>
        /// 商品税率
        /// </summary> 
        decimal TaxRate { get; set; }
        /// <summary>
        /// 商品单位
        /// </summary>
        string Unit { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        decimal? Quantity { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        decimal? TaxAmount { get; set; }

        /// <summary>
        /// 商品含税单价
        /// </summary>
        decimal? TaxUnitPrice { get; set; }
        /// <summary>
        /// 商品不含税单价
        /// </summary>
        decimal? UnitPrice { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        IDefined Defined { get; }
    }
}

namespace BingoX.Tax
{
    public interface IInvoiceDetail : ISourceInvoiceDetail
    {
        /// <summary>
        /// 
        /// </summary>
        long? SourceID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ISourceInvoiceDetail Source { get; }
    }
}

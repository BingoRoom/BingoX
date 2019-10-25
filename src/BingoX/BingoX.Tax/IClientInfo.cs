namespace BingoX.Tax
{
    public interface IClientInfo
    {
        /// <summary>
        /// 代号
        /// </summary>
        string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 税号
        /// </summary>
        string Tariff { get; set; }
        /// <summary>
        /// 地址电话
        /// </summary>
        string AddressAndPhone { get; set; }
        /// <summary>
        /// 银行账号
        /// </summary>
        string BankAndAccount { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        string Email { get; set; }
    }
}

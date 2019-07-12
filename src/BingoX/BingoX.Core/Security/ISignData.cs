using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Security
{
    /// <summary>
    /// 签名/验签接口
    /// </summary>
    public interface ISignData
    {
        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        string SignData(string content);
        /// <summary>
        /// 验签
        /// </summary>
        /// <param name="content"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        bool VerifyData(string content, string signature);
    }
}

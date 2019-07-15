using BingoX.ComponentModel.IDCard;
using System;

namespace BingoX.Validation
{
    /// <summary>
    /// 身份证号验证器
    /// </summary>
    public class IDCardValidations
    {
        /// <summary>
        /// 验证身份证号
        /// </summary>
        /// <param name="idcard">身份证号</param>
        /// <returns>验证结果</returns>
        public static bool ValidIDCard(string idcard)
        {
            if (string.IsNullOrEmpty(idcard)) throw new Exception("身份证号码为空");
            switch (idcard.Length)
            {
                case 16: return IDCard16.Valid(idcard);
                case 18: return IDCard18.Valid(idcard);
                default:
                    throw new Exception("身份证号码无效");
            }
        }
        /// <summary>
        /// 表示一个18位身份证号的信息
        /// </summary>
        public readonly static IDCard18 IDCard18 = new IDCard18();
        /// <summary>
        /// 表示一个16位身份证号的信息
        /// </summary>
        public readonly static IDCard16 IDCard16 = new IDCard16();
    }
}

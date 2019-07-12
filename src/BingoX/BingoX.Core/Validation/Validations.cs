using System;

namespace BingoX.Validation
{
    public class Validations
    {
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
        public readonly static EmailValidation Email = new EmailValidation();
        public readonly static IDCard18 IDCard18 = new IDCard18();

        public readonly static IDCard16 IDCard16 = new IDCard16();
    }
}

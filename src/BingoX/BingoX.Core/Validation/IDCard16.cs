using System;

namespace BingoX.Validation
{
    public class IDCard16 : IValidation<string>
    {
        public string To18(string idcard)
        {
            long n = 0;
            if (long.TryParse(idcard, out n) == false || n < Math.Pow(10, 14))
            {
                throw new Exception("长度不足16位");
            }
            int iS = 0;

            //加权因子常数
            int[] iW = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //校验码常数
            string LastCode = "10X98765432";
            //新身份证号
            string newIDCard;

            newIDCard = idcard.Substring(0, 6);
            //填在第6位及第7位上填上‘1’，‘9’两个数字
            newIDCard += "19";

            newIDCard += idcard.Substring(6, 9);

            //进行加权求和
            for (int i = 0; i < 17; i++)
            {
                iS += int.Parse(newIDCard.Substring(i, 1)) * iW[i];
            }

            //取模运算，得到模值
            int iY = iS % 11;
            //从LastCode中取得以模为索引号的值，加到身份证的最后一位，即为新身份证号。
            newIDCard += LastCode.Substring(iY, 1);
            return newIDCard;
        }
        public ChineseIdentityInfo GetIDCardInfo(string value)
        {
            long n = 0;
            if (long.TryParse(value, out n) == false || n < Math.Pow(10, 14))
            {
                throw new Exception("长度不足16位");
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(value.Remove(2)) == -1)
            {
                throw new Exception("省份不存在");
            }
            string birth = value.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                throw new Exception("出生日期无效");
            }
            return new ChineseIdentityInfo(value.Remove(2), int.Parse(value.Substring(15, 1)) % 2 == 1 ? IDCardGender.Male : IDCardGender.Female, time);
        }
        public bool Valid(string value)
        {

            try
            {
                GetIDCardInfo(value);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}

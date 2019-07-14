using BingoX.Validation;
using System;

namespace BingoX.ComponentModel.IDCard
{
    /// <summary>
    /// 表示一个18位身份证号的信息
    /// </summary>
    public class IDCard18 : IValidation<string>
    {

        public ChineseIdentityInfo GetIDCardInfo(string value)
        {
            long n = 0;
            if (long.TryParse(value.Remove(17), out n) == false
                || n < Math.Pow(10, 16) || long.TryParse(value.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                throw new Exception("身份证无效,长度不足18位");
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(value.Remove(2)) == -1)
            {
                throw new Exception("身份证无效,省份不存在");

            }
            string birth = value.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                throw new Exception("身份证无效,出生日期无效");
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = value.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != value.Substring(17, 1).ToLower())
            {
                throw new Exception("身份证无效,校验码无效");

            }
            return new ChineseIdentityInfo(value.Remove(2), int.Parse(value.Substring(16, 1)) % 2 == 1 ? IDCardGender.Male : IDCardGender.Female, time);
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

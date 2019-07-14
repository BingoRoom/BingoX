using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Validation
{
    /// <summary>
    /// 正则表达式常量
    /// </summary>
    public class ConstRegex
    {
        /// <summary>    
        /// 电子邮件正则表达式    
        /// </summary>    
        public const string EmailRegex = @"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$";
        // @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样     

        /// <summary>
        /// 复杂电子邮件正则表达式
        /// </summary>
        public const string ComplexEmailRegex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

        /// <summary>    
        /// 检测是否有中文字符正则表达式    
        /// </summary>    
        public const string CHZNRegex = "[\u4e00-\u9fa5]";

        /// <summary>    
        /// 检测用户名格式是否有效(只能是汉字、字母、下划线、数字)    
        /// </summary>    
        public const string UserNameRegex = @"^([\u4e00-\u9fa5A-Za-z_0-9]{0,})$";

        /// <summary>    
        /// 密码有效性正则表达式(仅包含字符数字下划线）6~16位    
        /// </summary>    
        public const string PasswordCharNumberRegex = @"^[A-Za-z_0-9]{6,16}$";

        /// <summary>    
        /// 密码有效性正则表达式（纯数字或者纯字母，不通过） 6~16位    
        /// </summary>    
        public const string PasswordRegex = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\s).{6,16}$";

        /// <summary>    
        /// INT类型数字正则表达式    
        /// </summary>    
        public const string ValidIntRegex = @"^[1-9]\d*\.?[0]*$";

        /// <summary>    
        /// 是否数字正则表达式    
        /// </summary>    
        public const string NumericRegex = @"^[-]?\d+[.]?\d*$";

        /// <summary>    
        /// 是否整数字正则表达式    
        /// </summary>    
        public const string NumberRegex = @"^[0-9]+$";

        /// <summary>    
        /// 是否整数正则表达式（可带带正负号）    
        /// </summary>    
        public const string NumberSignRegex = @"^[+-]?[0-9]+$";

        /// <summary>    
        /// 是否是浮点数正则表达式    
        /// </summary>    
        public const string DecimalRegex = "^[0-9]+[.]?[0-9]+$";

        /// <summary>    
        /// 是否是浮点数正则表达式(可带正负号)    
        /// </summary>    
        public const string DecimalSignRegex = "^[+-]?[0-9]+[.]?[0-9]+$";//等价于^[+-]?\d+[.]?\d+$    

        /// <summary>    
        /// 固定电话正则表达式    
        /// </summary>    
        public const string PhoneRegex = @"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$";

        /// <summary>    
        /// 移动电话正则表达式    
        /// </summary>    
        public const string MobileRegex = @"^(13|15|18)\d{9}$";

        /// <summary>    
        /// 固定电话、移动电话正则表达式    
        /// </summary>    
        public const string PhoneMobileRegex = @"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$|^(13|15|18)\d{9}$";

        /// <summary>    
        /// 身份证15位正则表达式    
        /// </summary>    
        public const string ID15Regex = @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$";

        /// <summary>    
        /// 身份证18位正则表达式    
        /// </summary>    
        public const string ID18Regex = @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$";

        /// <summary>    
        /// URL正则表达式    
        /// </summary>    
        public const string UrlRegex = @"\b(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]*[-A-Za-z0-9+&@#/%=~_|]";

        /// <summary>    
        /// IP正则表达式    
        /// </summary>    
        public const string IPRegex = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";

        /// <summary>    
        /// Base64编码正则表达式。    
        /// 大小写字母各26个，加上10个数字，和加号“+”，斜杠“/”，一共64个字符，等号“=”用来作为后缀用途    
        /// </summary>    
        public const string Base64Regex = @"[A-Za-z0-9\+\/\=]";

        /// <summary>    
        /// 是否为纯字符的正则表达式    
        /// </summary>    
        public const string LetterRegex = @"^[A-Za-z]+$";

        /// <summary>    
        /// GUID正则表达式    
        /// </summary>    
        public const string GuidRegex = "[A-F0-9]{8}(-[A-F0-9]{4}){3}-[A-F0-9]{12}|[A-F0-9]{32}";

        /// <summary>
        /// 日期正则表达式
        /// </summary>
        public const string DateRegex = @"((^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(10|12|0?[13578])([-\/\._])(3[01]|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(11|0?[469])([-\/\._])(30|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(0?2)([-\/\._])(2[0-8]|1[0-9]|0?[1-9])$)|(^([2468][048]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([3579][26]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][13579][26])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][13579][26])([-\/\._])(0?2)([-\/\._])(29)$))";

    }
}

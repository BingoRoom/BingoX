using System;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class WeChatJsSdk
    {
        public WeChatJsSdk(string jsapiTicket, string url)
        {
            JsapiTicket = jsapiTicket;
            Noncestr = GetNoncestr(16);
            Timestamp = GetTimeStamp() / 1000;
            Url = url;
        }

        public WeChatJsSdk(string jsapiTicket, string noncestr, long timestamp, string url) : this(jsapiTicket, url)
        {

            Noncestr = noncestr;
            Timestamp = timestamp;

        }
        public string JsapiTicket { get; private set; }
        public string Noncestr { get; private set; }
        public long Timestamp { get; private set; }
        public string Url { get; private set; }
        public string Signature { get; private set; }

        public string StringSign { get; private set; }
        public void Sign()
        {
            StringSign = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", JsapiTicket, Noncestr, Timestamp, Url);
            Signature = SHA1(StringSign);
        }

        long GetTimeStamp()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            DateTime nowTime = DateTime.Now;
            long unixTime = (long)System.Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            return unixTime;
        }

        /// <summary>
        /// SHA1 加密，返回大写字符串
        /// </summary>
        /// <param name="content">需要加密字符串</param>

        string SHA1(string content)
        {
            return SHA1(content, Encoding.ASCII);
        }
        /// <summary>
        /// SHA1 加密，返回大写字符串
        /// </summary>
        /// <param name="content">需要加密字符串</param>
        /// <param name="encode">指定加密编码</param>
        /// <returns>返回40位大写字符串</returns>
        string SHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }

        string GetNoncestr(int length)
        { //length表示生成字符串的长度
            string st = "abcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int number = random.Next(st.Length);
                sb.Append(st[number]);
            }
            return sb.ToString();
        }
    }
}




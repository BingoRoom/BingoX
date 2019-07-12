using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Security
{
    public interface IRSAKeyGenerator
    {
        /// <summary>
        /// 生成PKCS1的密钥。（非JAVA语言通用）
        /// </summary>
        /// <param name="keysize"></param>
        /// <returns></returns>
        string GeneratePKCS1(int keysize);
        /// <summary>
        /// 生成PKCS8的密钥。（JAVA、PHP可用）
        /// </summary>
        /// <param name="keysize"></param>
        /// <returns></returns>
        string GeneratePKCS8(int keysize);
        /// <summary>
        /// 生成XML的密钥。（NET专用）
        /// </summary>
        /// <param name="keysize"></param>
        /// <returns></returns>
        string GenerateXMLKey(int keysize);
    }
}

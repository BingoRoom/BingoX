using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Utility
{
    public static class FileUtility
    {
        static readonly string[] unit = { "KB", "MB", "GB", "TB", "PB" };
        const int filter = 1024;
        /// <summary>
        ///
        /// </summary>
        /// <param name="filesize"></param>
        /// <returns></returns>
        public static string GetFileSizeDisplay(long filesize)
        {
            if (filesize < filter) return string.Format("{0}b", filesize);

            long unitsize = 1;
            var flag = true;
            decimal size = filesize;
            int index = -1;
            while (flag)
            {
                size = size / filter;
                unitsize = unitsize * filter;
                flag = size > filter;
                index++;
                if (index >= unit.Length - 1) flag = false;
            }
            return string.Format("{0:f2}{1}", size, unit[index]);
        }
  

        public static string GetMD5(Stream stream)
        {
            byte[] bs = MD5.Create().ComputeHash(stream);
            return BitConverter.ToString(bs).Replace("-", "");
        }
        public static string GetMD5(byte[] stream)
        {
            byte[] bs = MD5.Create().ComputeHash(stream);
            return BitConverter.ToString(bs).Replace("-", "");
        }
    }
}

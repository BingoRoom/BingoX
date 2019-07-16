using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Utility
{
    /// <summary>
    /// 提供一个针对文件的操作工具
    /// </summary>
    public class FileUtility
    {
        static readonly string[] unit = { "KB", "MB", "GB", "TB", "PB" };
        const int filter = 1024;
        /// <summary>
        /// 获取文件大小的显示值。
        /// </summary>
        /// <param name="filesize">文件大小（KB）</param>
        /// <returns>返回文件大小的显示值。例如：276.34MB</returns>
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

        /// <summary>
        /// 获取文件的MD5
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public static string GetFileMD5(Stream stream)
        {
            return GetHash(MD5.Create(), stream);
        }

        /// <summary>
        /// 获取文件的MD5
        /// </summary>
        /// <param name="stream">字节数组</param>
        /// <returns></returns>
        public static string GetFileMD5(byte[] stream)
        {
            return GetHash(MD5.Create(), new MemoryStream(stream));
        }

        private static string GetHash(HashAlgorithm algorithm, Stream stream)
        {
            byte[] bs = algorithm.ComputeHash(stream);
            return BitConverter.ToString(bs).Replace("-", "");
        }

        /// <summary>
        /// 获取文件的SHA256
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public static string GetFileSHA256(Stream stream)
        {
            return GetHash(HashAlgorithm.Create("SHA256"), stream);
        }

        /// <summary>
        /// 获取文件的SHA256
        /// </summary>
        /// <param name="stream">字节数组</param>
        /// <returns></returns>
        public static string GetFileSHA256(byte[] stream)
        {
            return GetHash(HashAlgorithm.Create("SHA256"), new MemoryStream(stream));
        }
    }
}

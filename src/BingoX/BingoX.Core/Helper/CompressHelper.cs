using BingoX.ComponentModel.Compress;
using BingoX.Utility;
using System;
using System.IO;
using System.Text;

namespace BingoX.Helper
{
    public static class CompressHelper
    {
        private static string ByteToString(byte[] bytes, ByteTo byteTo)
        {
            switch (byteTo)
            {
                case ByteTo.Base64:
                    return ByteUtility.ByteToBase64(bytes);
                case ByteTo.Hex:
                    return ByteUtility.ByteToHex(bytes);
                default:
                    throw new CompressException("不支持ByteTo设置的字节流的转换方式");
            }
        }

        /// <summary>
        /// 压缩文件并返回指定编码的字符串
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="byteTo">指定编码方式</param>
        /// <param name="password">压缩密码</param>
        /// <returns>返回编码后的字符串</returns>
        public static string ZipCompressFromFile(this ICompress compress, string path, ByteTo byteTo, string password = null)
        {
            CompressFileEntry compressEntry = new CompressFileEntry(path);
            return ByteToString(compress.Compress(compressEntry, password), byteTo);
        }

        /// <summary>
        /// 压缩文件并返回Base64的字符串
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="password">压缩密码</param>
        /// <returns>返回编码后的字符串</returns>
        public static string ZipCompressFromFile(this ICompress compress, string path, string password = null)
        {
            return ZipCompressFromFile(compress, path, ByteTo.Base64, password);
        }

        /// <summary>
        /// 通过指定的编码压缩字符串并返回指定编码的字符串
        /// </summary>
        /// <param name="compress">当前压缩接口</param>
        /// <param name="entryName">压缩项名称</param>
        /// <param name="content">压缩内容字符串</param>
        /// <param name="encoding">压缩内容字符串编码</param>
        /// <param name="byteTo">指定编码方式</param>
        /// <param name="password">压缩密码</param>
        /// <returns>返回编码后的字符串</returns>
        public static string ZipCompressContent(this ICompress compress, string entryName, string content, Encoding encoding, ByteTo byteTo = ByteTo.Base64, string password = null)
        {
            CompressEntry compressEntry = new CompressEntry() { Name = entryName };
            compressEntry.FileContent = encoding.GetBytes(content);
            return ByteToString(compress.Compress(compressEntry, password), byteTo);
        }

        /// <summary>
        /// 通过UTF8编码压缩字符串并返回Base64的字符串
        /// </summary>
        /// <param name="compress">当前压缩接口</param>
        /// <param name="entryName">压缩项名称</param>
        /// <param name="content">压缩内容字符串</param>
        /// <param name="password">压缩密码</param>
        /// <returns>返回编码后的字符串</returns>
        public static string ZipCompressContent(this ICompress compress, string entryName, string content, string password = null)
        {
            return ZipCompressContent(compress, entryName, content, Encoding.UTF8, ByteTo.Base64, password);
        }

        /// <summary>
        /// 通过压缩字节流并返回指定编码的字符串
        /// </summary>
        /// <param name="compress">当前压缩接口</param>
        /// <param name="entryName">压缩项名称</param>
        /// <param name="buffer">待压缩字节流</param>
        /// <param name="byteTo">指定编码方式</param>
        /// <param name="password">压缩密码</param>
        /// <returns>返回编码后的字符串</returns>
        public static string ZipCompressContent(this ICompress compress, string entryName, byte[] buffer, ByteTo byteTo, string password = null)
        {
            CompressEntry compressEntry = new CompressEntry() { Name = entryName, FileContent = buffer };
            return ByteToString(compress.Compress(compressEntry, password), byteTo);
        }

        /// <summary>
        /// 通过压缩流并返回指定编码的字符串
        /// </summary>
        /// <param name="compress">当前压缩接口</param>
        /// <param name="entryName">压缩项名称</param>
        /// <param name="stream">待压缩流</param>
        /// <param name="byteTo">指定编码方式</param>
        /// <param name="password">压缩密码</param>
        /// <returns>返回编码后的字符串</returns>
        public static string ZipCompressContent(this ICompress compress, string entryName, Stream stream, ByteTo byteTo, string password = null)
        {
            CompressEntry compressEntry = new CompressEntry() { Name = entryName, FileContent = stream.ToArray() };
            return ByteToString(compress.Compress(compressEntry, password), byteTo);
        }
    }
}

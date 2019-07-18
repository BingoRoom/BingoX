using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BingoX.ComponentModel.Compress
{
    /// <summary>
    /// 提供一个压缩\解压相关的接口
    /// </summary>
    public interface ICompress
    {
        /// <summary>
        /// 压缩流
        /// </summary>
        /// <param name="compressEntry">压缩项</param>
        /// <returns></returns>
        byte[] Compress(CompressEntry compressEntry, string password = null);
        /// <summary>
        /// 批量压缩流
        /// </summary>
        /// <param name="compressEntrys">压缩项</param>
        /// <returns></returns>
        byte[] Compress(IEnumerable<CompressEntry> compressEntrys, string password = null);
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        IEnumerable<CompressEntry> Extract(byte[] bytes, string password = null);
    }

}

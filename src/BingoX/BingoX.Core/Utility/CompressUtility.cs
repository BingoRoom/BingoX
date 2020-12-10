using BingoX.ComponentModel.Compress;
using BingoX.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.Utility
{
    public class CompressUtility
    {
        private readonly ICompress _compress;
        public ICompress CurrentCompress { get { return _compress; } }
#if !NET40
        public CompressUtility()
        {
            var assembly = Assembly.Load(new AssemblyName("BingoX.Core.Extension"));
            if (assembly == null) throw new FileNotFoundException("BingoX.Core.Extension");
            var type = assembly.GetType("BingoX.ComponentModel.Compress.ZipCompress");
            if (type == null) throw new TypeAccessException("找不到解压类：BingoX.ComponentModel.Compress.ZipCompress");
            _compress = type.CreateInstance<ICompress>();
            if (_compress == null) throw new TypeAccessException("解压类创建失败：BingoX.ComponentModel.Compress.ZipCompress");
        }
#endif
        public CompressUtility(ICompress compress)
        {
            _compress = compress ?? throw new ArgumentNullException(nameof(compress));
        }

        private void ValidateEntry(CompressEntry compressEntry)
        {
            if (string.IsNullOrWhiteSpace(compressEntry.Name)) throw new CompareException("压缩项名称为空");
        }
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
        public string ZipCompressFromFile(string path, ByteTo byteTo, string password = null)
        {
            CompressFileEntry compressEntry = new CompressFileEntry(path);
            return ByteToString(this._compress.Compress(compressEntry, password), byteTo);
        }

        /// <summary>
        /// 压缩文件并返回Base64的字符串
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="password">压缩密码</param>
        /// <returns>返回编码后的字符串</returns>
        public string ZipCompressFromFile(string path, string password = null)
        {
            return ZipCompressFromFile(  path, ByteTo.Base64, password);
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
        public string ZipCompressContent(string entryName, string content, Encoding encoding, ByteTo byteTo = ByteTo.Base64, string password = null)
        {
            CompressEntry compressEntry = new CompressEntry() { Name = entryName };
            compressEntry.FileContent = encoding.GetBytes(content);
            return ByteToString(this._compress.Compress(compressEntry, password), byteTo);
        }

        /// <summary>
        /// 通过UTF8编码压缩字符串并返回Base64的字符串
        /// </summary>
        /// <param name="compress">当前压缩接口</param>
        /// <param name="entryName">压缩项名称</param>
        /// <param name="content">压缩内容字符串</param>
        /// <param name="password">压缩密码</param>
        /// <returns>返回编码后的字符串</returns>
        public string ZipCompressContent(string entryName, string content, string password = null)
        {
            return ZipCompressContent(entryName, content, Encoding.UTF8, ByteTo.Base64, password);
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
        public string ZipCompressContent(string entryName, byte[] buffer, ByteTo byteTo, string password = null)
        {
            CompressEntry compressEntry = new CompressEntry() { Name = entryName, FileContent = buffer };
            return ByteToString(this._compress.Compress(compressEntry, password), byteTo);
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
        public string ZipCompressContent(string entryName, Stream stream, ByteTo byteTo, string password = null)
        {
            CompressEntry compressEntry = new CompressEntry() { Name = entryName, FileContent = stream.ToArray() };
            return ByteToString(this._compress.Compress(compressEntry, password), byteTo);
        }
        private void ValidateEntry(IEnumerable<CompressEntry> compressEntrys)
        {
            if (compressEntrys.Any(n => string.IsNullOrWhiteSpace(n.Name))) throw new CompareException("存在空压缩项名称");
            if (compressEntrys.GroupBy(n => n.Name).Any(n => n.Count() > 1)) throw new CompareException("存在重复的压缩项名称");
        }

        /// <summary>
        /// 压缩文件并返回字节流
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="password">压缩密码</param>
        /// <returns>字节流</returns>
        public byte[] CompressFromFile(string path, string password = null)
        {
            CompressFileEntry compressEntry = new CompressFileEntry(path);
            return _compress.Compress(compressEntry, password);
        }
        /// <summary>
        /// 把文件压缩到指定路径
        /// </summary>
        /// <param name="inPath">待压缩文件路径</param>
        /// <param name="outPath">压缩后的指定路径</param>
        /// <param name="password">压缩密码</param>
        public void CompressFromFile(string inPath, string outPath, string password = null)
        {

            CompressFileEntry compressEntry = new CompressFileEntry(inPath);
            byte[] buffer = _compress.Compress(compressEntry, password);
            File.WriteAllBytes(outPath, buffer);
        }
        /// <summary>
        /// 压缩文件夹
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <param name="password">压缩密码</param>
        /// <returns>压缩文件流</returns>
        public byte[] CompressFromDirectory(string dirPath, string password = null)
        {
            if (!Directory.Exists(dirPath)) throw new CompareException("文件目录不存在");
            var files = Directory.GetFiles(dirPath).Select(n => new CompressFileEntry(n));
            return CompressFromFiles(files, password);
        }
        /// <summary>
        /// 批量压缩文件
        /// </summary>
        /// <param name="compressEntries">待压缩文件集合</param>
        /// <param name="password">压缩密码</param>
        /// <returns>压缩文件流</returns>
        public byte[] CompressFromFiles(IEnumerable<CompressEntry> compressEntries, string password = null)
        {
            ValidateEntry(compressEntries);
            return _compress.Compress(compressEntries, password);
        }
        /// <summary>
        /// 批量把文件压缩到指定路径
        /// </summary>
        /// <param name="compressEntries">待压缩文件集合</param>
        /// <param name="outPath">压缩后的指定路径</param>
        /// <param name="password">压缩密码</param>
        public void CompressFromFiles(IEnumerable<CompressEntry> compressEntries, string outPath, string password = null)
        {
            var buffer = CompressFromFiles(compressEntries, password);
            File.WriteAllBytes(outPath, buffer);
        }

        /// <summary>
        /// 压缩文件并返回Stream
        /// </summary>
        /// <param name="inPath">待压缩文件路径</param>
        /// <param name="outPath">压缩文件流</param>
        /// <param name="password">压缩密码</param>
        public void CompressFromFile(string inPath, Stream outPath, string password = null)
        {
            if (outPath == null || !outPath.CanWrite) throw new CompareException("流不可写");
            CompressFileEntry compressEntry = new CompressFileEntry(inPath);
            byte[] buffer = _compress.Compress(compressEntry, password);
            outPath.Write(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// 批量压缩文件并返回Stream
        /// </summary>
        /// <param name="compressEntries">待压缩文件集合</param>
        /// <param name="outPath">压缩文件流</param>
        /// <param name="password">压缩密码</param>
        public void CompressFromFiles(IEnumerable<CompressEntry> compressEntries, Stream outPath, string password = null)
        {
            if (outPath == null || !outPath.CanWrite) throw new CompareException("流不可写");
            var buffer = CompressFromFiles(compressEntries, password);
            outPath.Write(buffer, 0, buffer.Length);
        }

        public byte[] Compress(CompressEntry compressEntry, string password = null)
        {
            ValidateEntry(compressEntry);
            return _compress.Compress(compressEntry, password);
        }

        public byte[] Compress(byte[] bytes, string entryName, string password = null)
        {
            var entry = new CompressEntry() { Name = entryName, FileContent = bytes };
            ValidateEntry(entry);
            return Compress(entry, password);
        }

        public byte[] Compress(IEnumerable<CompressEntry> compressEntrys, string password = null)
        {
            ValidateEntry(compressEntrys);
            return _compress.Compress(compressEntrys, password);
        }

        public void Compress(CompressEntry compressEntry, string outPath, string password = null)
        {
            ValidateEntry(compressEntry);
            var buffer = _compress.Compress(compressEntry, password);
            File.WriteAllBytes(outPath, buffer);
        }

        public void Compress(byte[] bytes, string entryName, string outPath, string password = null)
        {
            var entry = new CompressEntry() { Name = entryName, FileContent = bytes };
            ValidateEntry(entry);
            var buffer = Compress(entry, password);
            File.WriteAllBytes(outPath, buffer);
        }

        public void Compress(IEnumerable<CompressEntry> compressEntrys, string outPath, string password = null)
        {
            ValidateEntry(compressEntrys);
            var buffer = _compress.Compress(compressEntrys, password);
            File.WriteAllBytes(outPath, buffer);
        }

        public void Compress(CompressEntry compressEntry, Stream outPath, string password = null)
        {
            ValidateEntry(compressEntry);
            var buffer = _compress.Compress(compressEntry, password);
            outPath.Write(buffer, 0, buffer.Length);
        }

        public void Compress(byte[] bytes, string entryName, Stream outPath, string password = null)
        {
            var entry = new CompressEntry() { Name = entryName, FileContent = bytes };
            ValidateEntry(entry);
            var buffer = Compress(entry, password);
            outPath.Write(buffer, 0, buffer.Length);
        }

        public void Compress(IEnumerable<CompressEntry> compressEntrys, Stream outPath, string password = null)
        {
            ValidateEntry(compressEntrys);
            var buffer = _compress.Compress(compressEntrys, password);
            outPath.Write(buffer, 0, buffer.Length);
        }

        public IEnumerable<CompressEntry> Extract(byte[] buffer, string password = null)
        {
            return _compress.Extract(buffer, password);
        }

        public void Extract(byte[] buffer, string dirPath, string password = null)
        {
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            _compress.Extract(buffer, password).Foreach(n => File.WriteAllBytes(Path.Combine(dirPath, n.Name), n.FileContent));
        }
    }
}

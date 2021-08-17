using System;

namespace BingoX.ComponentModel.Compress
{
    /// <summary>
    /// 流压缩项目
    /// </summary>
    public class CompressEntry
    {
        /// <summary>
        /// 压缩项目名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 流
        /// </summary>
        public byte[] FileContent { get; set; }

        /// <summary>
        /// 完全名称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }


        public static CompressEntry StringContent(string name, string content, System.Text.Encoding encoding)
        {
            return new CompressEntry
            {
                FileContent = encoding.GetBytes(content),
                Name = name,
                FullName = name,
            };
        }
        public static CompressEntry StringContent(string name, string content)
        {
            return StringContent(name, content, System.Text.Encoding.UTF8);
        }
    }
}

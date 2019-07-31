using System.IO;

namespace BingoX.ComponentModel.Compress
{

    /// <summary>
    /// 文件压缩项目
    /// </summary>
    public class CompressFileEntry : CompressEntry
    {
        public CompressFileEntry(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) throw new CompressException("压缩项文件路径无效或文件不存在");
            FileContent = File.ReadAllBytes(filePath);
            Name = Path.GetFileName(filePath);
            FilePath = filePath;
        }
        public CompressFileEntry(string filename, string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) throw new CompressException("压缩项文件路径无效或文件不存在");
            FileContent = File.ReadAllBytes(filePath);
            Name = filename;
            FilePath = filePath;
        }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
    }
}

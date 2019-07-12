namespace BingoX.ComponentModel.DocumentManagementProvider
{
    /// <summary>
    /// 表示一个文件信息
    /// </summary>
    public class DMSFileInfo : BaseSystemInfo
    {
        public DMSFileInfo()
        {
            IsFile = true;
        }
        /// <summary>
        /// 文件大小，以字节为单位。
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 文件内容类型
        /// </summary>
        public string ContentType { get; set; }
    }
}

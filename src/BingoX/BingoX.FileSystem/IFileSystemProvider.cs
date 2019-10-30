namespace BingoX.FileSystem
{
    /// <summary>
    /// 表示一个实现了某个文档管理提供程序的文档管理接口
    /// </summary>
    public interface IFileSystemProvider
    {
        /// <summary>
        /// 获取文件字节数组
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        byte[] GetFileBuffer(string path);
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        void DeleteDirectory(string path);
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        string CreateDirectory(string path);
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        void DeleteFile(string filePath);
        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        /// <param name="rootPath">文件夹路径</param>
        /// <returns></returns>
        bool ExistsDirectory(string rootPath);
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="filePath">文件夹路径</param>
        /// <returns></returns>
        bool ExistsFile(string filePath);
        /// <summary>
        /// 获取路径下的所有文件夹
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <returns></returns>
        DMSDirectoryInfo[] GetDirectories(string dirPath);
        /// <summary>
        /// 获取指定路径的文件信息
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        DMSFileInfo GetFileInfo(string path);
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="fileBuffer">文件流</param>
        /// <param name="fileName">文件名</param>
        /// <param name="isOverWrite">是否覆盖</param>
        /// <returns></returns>
        DMSFileInfo AddFile(byte[] fileBuffer, string fileName, bool isOverWrite);
    }
}

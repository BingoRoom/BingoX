namespace BingoX.FileSystem
{
    /// <summary>
    /// 通过文件哈唏值做保存
    /// </summary>
    public interface IHashFileSystemProvider
    {
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="fileBuffer">文件流</param>
        /// <param name="fileExtension">文件名后缀</param>
        /// <returns>生成的引用路径</returns>
        /// <example>
        /// IHashFileSystemProvider provider;
        /// var filename = provider.AddFile(bytes,".jpg");
        /// output: 
        /// filename : "group/001/698d51a19d8a121ce581499d7b701668.jpg"
        /// </example>
        string AddFile(byte[] fileBuffer, string fileExtension);

        /// <summary>
        /// 通过哈唏值获取路径
        /// </summary>
        /// <param name="hashCode"></param>
        /// <returns></returns>
        ///   /// <example>
        /// IHashFileSystemProvider provider;
        /// var filename = provider.GetFullName("698d51a19d8a121ce581499d7b701668");
        /// output: 
        /// filename : "group/001/698d51a19d8a121ce581499d7b701668.jpg"
        /// </example>
        string GetFullName(string hashCode);

        /// <summary>
        /// 通过路径下载文件流
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        byte[] GetFileBufferByFullName(string fullName);
        /// <summary>
        /// 通过哈唏值下载文件流
        /// </summary>
        /// <param name="hashCode"></param>
        /// <returns></returns>
        byte[] GetFileBuffer(string hashCode);
    }
}

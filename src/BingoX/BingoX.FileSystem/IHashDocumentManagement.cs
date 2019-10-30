namespace BingoX.FileSystem
{
    /// <summary>
    /// 表示一个文档管理接口
    /// </summary>
    public interface IHashDocumentManagement : IHashFileSystemProvider
    {

        void Connection(string connectionString);

       
    }
}

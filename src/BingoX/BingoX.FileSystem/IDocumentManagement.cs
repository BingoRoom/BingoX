using System;
using System.Collections.Specialized;
using System.IO;

namespace BingoX.FileSystem
{
    /// <summary>
    /// 表示一个文档管理接口
    /// </summary>
    public interface IDocumentManagement : IFileSystemProvider, IDocumentManagementProvider, IFileSystemProvider<long>, IHistroyProvider<long>
    {

        DMSFileInfo AddFile(byte[] fileBuffer, NameValueCollection metas);
        DMSFileInfo ByMd5(string md5);



        bool ExistByMd5(string md5);
        NameValueCollection GetConfig();


        byte[] GetFileBufferByFullPath(string APIFullName);

        DMSFileInfo[] GetFiles(string dirPath);
        void Initialize();
    }
}

using System;
using System.Collections.Specialized;
using System.IO;

namespace BingoX.ComponentModel.DocumentManagementProvider
{
    /// <summary>
    /// 表示一个文档管理接口
    /// </summary>
    public interface IDocumentManagement
    {
        IDocumentManagementAPIProvider APIProvider { get; }
        DMSFileInfo AddFile(byte[] fileBuffer,  NameValueCollection metas);
        DMSFileInfo ByMd5(string md5);
        void Close();
        void Connection(); 
        void CreateDirectory(string path);
        void DeleteDirectory(string dirPath);
        void DeleteFile(long id);
        void DeleteFile(string filePath);
        void Dispose();
        bool ExistByMd5(string md5);
        NameValueCollection GetConfig();
        DMSDirectoryInfo[] GetDirectories(string dirPath); 
        byte[] GetFileBuffer(long fileid);
        byte[] GetFileBufferByURL(string APIFullName);
        DMSFileInfo GetFileInfo(string fileName);
        DMSFileInfo GetFileInfo(long fileid);
        DMSFileInfo[] GetFiles(string dirPath); 
        void Initialize();
        void UpdateFile(long id, byte[] fileBuffer, string fileName, NameValueCollection metas);
    }
}

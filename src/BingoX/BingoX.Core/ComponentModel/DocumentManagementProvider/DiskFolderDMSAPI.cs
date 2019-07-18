using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace BingoX.ComponentModel.DocumentManagementProvider
{
    public class DiskFolderDMSAPI : IDocumentManagementAPIProvider
    {
        private string _rootList;

        public DMSFileInfo AddFile(byte[] fileBuffer, string fileName, bool isOverWrite)
        {
            string readPath = GetReadPath(fileName);
            var fs = File.Open(readPath, FileMode.Create);
            fs.Write(fileBuffer, 0, fileBuffer.Length);
            fs.Close();
            fs.Dispose();
            return GetFileInfo(fileName);
        }

        public void Close()
        {
        }

        public void Connection()
        {
        }

        public string CreateDirectory(string path)
        {
            string readPath = GetReadPath(path);
            return Directory.CreateDirectory(readPath).FullName;
        }

        public void DeleteDirectory(string path)
        {
            string readPath = GetReadPath(path);
            Directory.Delete(readPath);
        }

        public void DeleteFile(string filePath)
        {
            string readPath = GetReadPath(filePath);
            if (File.Exists(readPath)) File.Delete(readPath);
        }

        public void Dispose()
        {
        }

        public bool ExistsDirectory(string dirPath)
        {
            return Directory.Exists(dirPath);
        }

        public bool ExistsFile(string filePath)
        {
            return Directory.Exists(filePath);
        }

        public DMSDirectoryInfo[] GetDirectories(string dirPath)
        {

            return Directory.GetDirectories(dirPath).Select(
                n => new DMSDirectoryInfo
                {
                    APIFullName = n,

                }).ToArray();
        }

        public byte[] GetFileBuffer(string path)
        {
            var buffer = File.ReadAllBytes(path);

            return buffer;
        }

        public byte[] GetFileBuffer(long attachmentId)
        {
            throw new NotImplementedException();
        }

        public DMSFileInfo GetFileInfo(string path)
        {
            string readPath = GetReadPath(path);
            if (!File.Exists(readPath)) return null;
            var fs = File.Open(readPath, FileMode.Open);
            var size = fs.Length;
            fs.Close();
            fs.Dispose();
            return new DMSFileInfo()
            {
                Name = System.IO.Path.GetFileName(path),
                Size = size,
                APIFullName = path,
            };
        }

        private string GetReadPath(string path)
        {
            return Path.Combine(_rootList, path);
        }



        public void Initialize(NameValueCollection collection)
        {
            _rootList = Path.Combine(System.Environment.CurrentDirectory, "Documents");
            //  _rootList = collection["RootList"];
            //    StorageFile = System.IO.IsolatedStorage.IsolatedStorageFile.GetMachineStoreForAssembly();
            if (!Directory.Exists(_rootList)) Directory.CreateDirectory(_rootList);
        }
    }
}

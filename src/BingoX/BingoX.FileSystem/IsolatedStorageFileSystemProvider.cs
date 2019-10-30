
using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;

namespace BingoX.FileSystem
{
    public class IsolatedStorageFileSystemProvider : IFileSystemProvider, IDisposable
    {
        private readonly IsolatedStorageFile storage;

        public IsolatedStorageFileSystemProvider(IsolatedStorageFile storage)
        {
            if (storage is null)
            {
                throw new ArgumentNullException(nameof(storage));
            }
            this.storage = storage;

        }



        public DMSFileInfo AddFile(byte[] fileBuffer, string fileName, bool isOverWrite)
        {
            string readPath = GetReadPath(fileName);
            var fs = storage.CreateFile(readPath);
            fs.Write(fileBuffer, 0, fileBuffer.Length);
            fs.Close();
            fs.Dispose();
            return GetFileInfo(fileName);
        }
        private string GetReadPath(string path)
        {
            return path;
        }


        public string CreateDirectory(string path)
        {
            string readPath = GetReadPath(path);
            if (storage.DirectoryExists(readPath)) return readPath;
            return Directory.CreateDirectory(readPath).FullName;
        }

        public void DeleteDirectory(string path)
        {
            string readPath = GetReadPath(path);
            if (!storage.DirectoryExists(readPath)) throw new DocumentManagementException("目录不存在");
            storage.DeleteDirectory(readPath);
        }

        public void DeleteFile(string filePath)
        {
            string readPath = GetReadPath(filePath);
            if (storage.FileExists(readPath)) storage.DeleteFile(readPath);
        }



        public bool ExistsDirectory(string dirPath)
        {
            return storage.DirectoryExists(dirPath);
        }

        public bool ExistsFile(string filePath)
        {
            return storage.FileExists(filePath);
        }

        public DMSDirectoryInfo[] GetDirectories(string dirPath)
        {

            return storage.GetDirectoryNames(dirPath).Select(
                n => new DMSDirectoryInfo
                {
                    APIFullName = n,

                }).ToArray();
        }

        public byte[] GetFileBuffer(string path)
        {
            var stream = storage.OpenFile(path, FileMode.Open);

            return ToArray(stream);
        }

        static byte[] ToArray(IsolatedStorageFileStream stream)
        {

            if (!stream.CanRead) throw new IOException("不能转换成流");




            var buffers = new byte[stream.Length];

            stream.Read(buffers, 0, buffers.Length);


            return buffers;
        }

        public DMSFileInfo GetFileInfo(string path)
        {
            string readPath = GetReadPath(path);
            if (!storage.FileExists(readPath)) return null;
            var fs = storage.OpenFile(readPath, FileMode.Open);
            var size = fs.Length;
            fs.Close();
            fs.Dispose();
            return new DMSFileInfo()
            {
                Name = Path.GetFileName(path),
                Size = size,
                APIFullName = path,
            };
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    storage.Dispose();

                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~IsolatedStorageFileSystemProvider()
        // {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}

using System.Collections.Specialized;

namespace BingoX.FileSystem
{
    public interface IFileSystemProvider<PID>
    {
        void DeleteFile(PID id);
        byte[] GetFileBuffer(PID fileid);
        DMSFileInfo GetFileInfo(PID fileid);
        void UpdateFile(PID id, byte[] fileBuffer, string fileName, NameValueCollection metas);
    }
}

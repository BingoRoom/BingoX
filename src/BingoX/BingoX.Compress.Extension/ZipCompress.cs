using BingoX.ComponentModel.Compress;
using BingoX.Helper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BingoX.Compress
{
    public class ZipCompress : SharpCompress, ICompress
    {


        public override byte[] Compress(IEnumerable<CompressEntry> compressEntrys, string password = null)
        {
            if (!string.IsNullOrEmpty(password)) throw new System.Exception("不支持密码加密");
            Stream stream = new MemoryStream();
            var archive = global::SharpCompress.Archives.Zip.ZipArchive.Create();
            WriteFiles(archive, compressEntrys);
            archive.SaveTo(stream);
            var buffer = stream.ToArray();
            archive.Dispose();
            stream.Dispose();
            return buffer;
        }

        public IEnumerable<CompressEntry> Extract(byte[] bytes, string password = null)
        {
            Stream stream = new MemoryStream(bytes);
            if (!global::SharpCompress.Archives.Zip.ZipArchive.IsZipFile(stream)) throw new LogicException("不为 Zip文件");

            var archive = global::SharpCompress.Archives.Zip.ZipArchive.Open(stream, new global::SharpCompress.Readers.ReaderOptions() { Password = password });
            var list = Extract(archive.ExtractAllEntries());
            return list;
        }
    }
}

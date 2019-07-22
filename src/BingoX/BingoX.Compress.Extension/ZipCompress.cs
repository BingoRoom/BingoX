using BingoX.ComponentModel.Compress;
using BingoX.Helper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BingoX.Compress
{
    public class ZipCompress : AbsCompress, ICompress
    {


        public override byte[] Compress(IEnumerable<CompressEntry> compressEntrys, string password = null)
        {
            Stream stream = new MemoryStream();
            var archive = SharpCompress.Archives.Zip.ZipArchive.Create();
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
            if (!SharpCompress.Archives.Zip.ZipArchive.IsZipFile(stream)) throw new LogicException("不为 Zip文件");

            var archive = SharpCompress.Archives.Zip.ZipArchive.Open(stream, new SharpCompress.Readers.ReaderOptions() { Password = password });
            var list = Extract(archive.ExtractAllEntries());
            return list;
        }
    }
}

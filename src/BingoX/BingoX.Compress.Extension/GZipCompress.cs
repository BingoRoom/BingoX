using BingoX.ComponentModel.Compress;
using BingoX.Helper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BingoX.Compress
{
    public class GZipCompress : SharpCompress, ICompress
    {


        public override byte[] Compress(IEnumerable<CompressEntry> compressEntrys, string password = null)
        {
            Stream stream = new MemoryStream();
            var writer = new global::SharpCompress.Writers.GZip.GZipWriter(stream, new global::SharpCompress.Writers.GZip.GZipWriterOptions());
            WriteFiles(writer, compressEntrys);

            var buffer = stream.ToArray();
            writer.Dispose();
            stream.Dispose();
            return buffer;
        }

        public IEnumerable<CompressEntry> Extract(byte[] bytes, string password = null)
        {
            Stream stream = new MemoryStream(bytes);
            if (!global::SharpCompress.Archives.GZip.GZipArchive.IsGZipFile(stream)) throw new LogicException("不为 GZip文件");

            var archive = global::SharpCompress.Archives.GZip.GZipArchive.Open(stream, new global::SharpCompress.Readers.ReaderOptions() { Password = password });
            var list = Extract(archive.ExtractAllEntries());
            return list;
        }
    }
}

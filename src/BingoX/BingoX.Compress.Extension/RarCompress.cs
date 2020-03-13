using BingoX.ComponentModel.Compress;
using BingoX.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BingoX.Compress
{

    public class RarCompress : SharpCompress, ICompress
    {
        public override byte[] Compress(CompressEntry compressEntry, string password = null)
        {
            throw new NotSupportedException();
        }

        public override byte[] Compress(IEnumerable<CompressEntry> compressEntrys, string password = null)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<CompressEntry> Extract(byte[] bytes, string password = null)
        {
            Stream stream = new MemoryStream(bytes);
            if (!global::SharpCompress.Archives.Rar.RarArchive.IsRarFile(stream)) throw new LogicException("不为 GZip文件");

            var archive = global::SharpCompress.Archives.Rar.RarArchive.Open(stream, new global::SharpCompress.Readers.ReaderOptions() { Password = password });
            var list = Extract(archive.ExtractAllEntries());
            return list;
        }
    }
}

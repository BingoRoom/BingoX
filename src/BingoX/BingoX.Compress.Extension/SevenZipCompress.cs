using BingoX.ComponentModel.Compress;
using System;
using System.Collections.Generic;
using System.IO;

namespace BingoX.Compress
{
    public class SevenZipCompress : AbsCompress, ICompress
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
            if (!SharpCompress.Archives.SevenZip.SevenZipArchive.IsSevenZipFile(stream)) throw new LogicException("不为 7-Zip文件");

            var archive = SharpCompress.Archives.SevenZip.SevenZipArchive.Open(stream, new SharpCompress.Readers.ReaderOptions() { Password = password });
            var list = Extract(archive.ExtractAllEntries());
            return list;
        }
    }
}

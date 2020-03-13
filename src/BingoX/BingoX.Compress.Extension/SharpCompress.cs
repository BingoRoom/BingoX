using BingoX.ComponentModel.Compress;
using BingoX.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BingoX.Compress
{
    public abstract class SharpCompress
    {
        public abstract byte[] Compress(IEnumerable<CompressEntry> compressEntrys, string password = null);
        public virtual byte[] Compress(CompressEntry compressEntry, string password = null)
        {

            return Compress(new CompressEntry[] { compressEntry }, password);
        }
        protected virtual IEnumerable<CompressEntry> Extract(global::SharpCompress.Readers.IReader reader)
        {
            return SharpCompressFactory.Extract(reader);
        }
        protected virtual void WriteFiles(global::SharpCompress.Writers.IWriter writer, IEnumerable<CompressEntry> compressEntrys)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (compressEntrys == null)
            {
                throw new ArgumentNullException(nameof(compressEntrys));
            }
            foreach (var item in compressEntrys)
            {
                writer.Write(item.Name, new MemoryStream(item.FileContent), item.CreateTime);
            }
        }
        protected virtual void WriteFiles(global::SharpCompress.Archives.IWritableArchive writer, IEnumerable<CompressEntry> compressEntrys)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (compressEntrys == null)
            {
                throw new ArgumentNullException(nameof(compressEntrys));
            }
            foreach (var item in compressEntrys)
            {
                writer.AddEntry(item.Name, new MemoryStream(item.FileContent), false, item.FileContent.LongLength, item.CreateTime);
            }
        }
    }
}

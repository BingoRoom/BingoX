using BingoX.ComponentModel.Compress;
using BingoX.Helper;
using System.Collections.Generic;
using System.IO;

namespace BingoX.Compress
{
    public static class SharpCompressFactory
    {
        public static ICompress Zip()
        {
            return new ZipCompress();
        }
        public static ICompress Rar()
        {
            return new RarCompress();
        }
        public static ICompress Tar()
        {
            return new TarCompress();
        }
        public static ICompress GZip()
        {
            return new GZipCompress();
        }
        public static ICompress SevenZip()
        {
            return new SevenZipCompress();
        }

        public static IEnumerable<CompressEntry> Extract(byte[] bytes, string password = null)
        {
            try
            {
                Stream stream = new MemoryStream(bytes);
                var reader = global::SharpCompress.Readers.ReaderFactory.Open(stream, new global::SharpCompress.Readers.ReaderOptions { Password = password });
                return Extract(reader);
            }
            catch (System.Exception ex)
            {

                throw new CompressException("解压失败", ex);
            }

        }

        internal static IEnumerable<CompressEntry> Extract(global::SharpCompress.Readers.IReader reader)
        {
            IList<CompressEntry> list = new List<CompressEntry>();
            while (reader.MoveToNextEntry())
            {
                if (reader.Entry != null)
                {
                    var stream = reader.OpenEntryStream();
                    var buffer = stream.ToArray();
                    list.Add(new CompressEntry { Name = reader.Entry.Key, CreateTime = reader.Entry.CreatedTime, FileContent = buffer });
                }
            }

            return list;
        }
    }
}

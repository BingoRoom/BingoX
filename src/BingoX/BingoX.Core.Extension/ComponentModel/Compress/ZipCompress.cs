using System.IO.Compression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BingoX.Helper;

namespace BingoX.ComponentModel.Compress
{
#if !NET40
    public class ZipCompress : ICompress
    {
        public byte[] Compress(CompressEntry compressEntry, string password = null)
        {
            MemoryStream ms = new MemoryStream();
            ZipArchive zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true);
            try
            {
                AddEntry(zipArchive, compressEntry);
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                zipArchive.Dispose();
                ms.Close();
            }
            return ms.ToArray();
        }

        public byte[] Compress(IEnumerable<CompressEntry> compressEntrys, string password = null)
        {
            MemoryStream ms = new MemoryStream();
            ZipArchive zipArchive = new ZipArchive(ms, ZipArchiveMode.Create, true);
            try
            {
                foreach (var item in compressEntrys)
                {
                    AddEntry(zipArchive, item);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                zipArchive.Dispose();
                ms.Close();
            }
            return ms.ToArray();
        }

        public IEnumerable<CompressEntry> Extract(byte[] bytes, string password = null)
        {
            List<CompressEntry> list = new List<CompressEntry>();
            MemoryStream ms = new MemoryStream(bytes);
            ZipArchive zipArchive = new ZipArchive(ms);
            try
            {
                foreach (var entry in zipArchive.Entries)
                {
                    CompressEntry compressEntry = new CompressEntry();
                    compressEntry.Name = entry.Name;
                    using (Stream stream = entry.Open())
                    {
                        compressEntry.FileContent = stream.ToArray();
                    }
                    list.Add(compressEntry);
                }
            }
            finally
            {
                ms.Close();
            }
            return list;
        }

        /// <summary>
        /// 新增压缩项
        /// </summary>
        /// <param name="zipArchive"></param>
        /// <param name="compressEntry"></param>
        private void AddEntry(ZipArchive zipArchive, CompressEntry compressEntry)
        {
            var entry = zipArchive.CreateEntry(compressEntry.Name);
            if (compressEntry.FileContent == null || compressEntry.FileContent.Length == 0) return;
            using (Stream stream = entry.Open())
            {
                stream.Write(compressEntry.FileContent, 0, compressEntry.FileContent.Length);
            }
        }
    }
#endif
}

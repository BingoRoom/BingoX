using BingoX.ComponentModel.Compress;

using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
namespace BingoX.Compress
{
    public class ZipICSharpCompress : ICompress
    {
        public byte[] Compress(IEnumerable<CompressEntry> compressEntrys, string password = null)
        {
            MemoryStream fs = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(fs);




            try
            {

                zipStream.SetLevel(6);
                if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                foreach (var item in compressEntrys)
                {
                    ZipEntry ent = new ZipEntry(item.Name) { Size = item.FileContent.Length, DateTime = DateTime.Now };
                    var buffer = item.FileContent;
                    zipStream.PutNextEntry(ent);

                    zipStream.Write(buffer, 0, buffer.Length);
                }
                zipStream.Finish();
                zipStream.Close();
                return fs.ToArray();

            }
            catch (Exception ex)
            {

                throw new CompressException("压缩失败", ex);
            }
            finally
            {


                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }

        }

        public byte[] Compress(CompressEntry compressEntry, string password = null)
        {
            return Compress(new[] { compressEntry }, password);
        }

        public IEnumerable<CompressEntry> Extract(byte[] bytes, string password = null)
        {
            MemoryStream fs = new MemoryStream(bytes);
            ZipInputStream zipStream = new ZipInputStream(fs);
            if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
            List<CompressEntry> entries = new List<CompressEntry>();

            try
            {
                ZipEntry ent;
                while ((ent = zipStream.GetNextEntry()) != null)
                {
                    if (!ent.IsFile) continue;
                    if (string.IsNullOrEmpty(ent.Name)) continue;
                    CompressEntry entry = new CompressEntry
                    {
                        Name = ent.Name,
                        CreateTime = ent.DateTime,
                        FileContent = new byte[ent.Size]
                    };
                    zipStream.Read(entry.FileContent, 0, entry.FileContent.Length);
                    entries.Add(entry);
                }

                zipStream.Close();

            }
            catch (Exception ex)
            {
                throw new CompressException("解压失败", ex);
            }
            finally
            {


                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }

            return entries.ToArray();
        }
    }
}

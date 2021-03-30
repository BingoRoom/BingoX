using System;
using System.IO;
using System.Text;

namespace BingoX.Helper
{
    /// <summary>
    /// 提供对输入输出流的辅助
    /// </summary>
    public static class IOHelper
    {
        /// <summary>
        /// 把Stream转为byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ToArray(this Stream stream)
        {
            if (stream == null)
            {
                throw new System.ArgumentNullException("name");
            }

            if (stream is MemoryStream)
            {
                var ms = stream as MemoryStream;
                ms.Seek(0, SeekOrigin.Begin);
                return ms.ToArray();
            }
            stream.TrySeek();
            if (stream.CanRead)
            {

                const int maxbuffer = 1024000;

                var buffers = new byte[maxbuffer];
                var ms = new MemoryStream();
                while (true)
                {

                    var flag = stream.Read(buffers, 0, buffers.Length);

                    ms.Write(buffers, 0, flag);
                    if (flag == 0) break;
                }
                ms.Seek(0, SeekOrigin.Begin);
                return ms.ToArray();
            }
            throw new IOException("不能转换成流");
        }

        /// <summary>
        /// 从当前Stream中读取指定长度的byte[]
        /// </summary>
        /// <param name="stream">当前Stream中</param>
        /// <param name="length">长度</param> 
        /// <returns>byte[]</returns>
        public static byte[] ToArray(this Stream stream, int length)
        {
            if (stream is MemoryStream) return ((MemoryStream)stream).ToArray();
            if (stream == null || !stream.CanRead) return new byte[0];
            if (!stream.CanSeek && stream.Position != 0) return new byte[0];
            if (stream.CanSeek && stream.Position != 0) stream.Seek(0, SeekOrigin.Begin);

            byte[] buffter = new byte[length];
            stream.Read(buffter, 0, length);
            //stream.Seek(0, SeekOrigin.Begin);
            return buffter;
        }

        /// <summary>
        /// 把Stream写入byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="bytes"></param>
        public static void WriteBytes(this Stream stream, byte[] bytes)
        {
            if (stream == null || !stream.CanWrite || !bytes.HasAny()) return;
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 复制一个Stream
        /// </summary>
        /// <param name="source">源Stream</param>
        /// <param name="traget">目标Stream</param>
        public static TryResult Copy(this Stream source, Stream traget)
        {
            if (source == null) return new ArgumentNullException("source");
            if (!source.CanRead) return new Exception("source Can't Read");
            if (source.Length == 0) return true;
            if (!source.CanSeek && source.Position != 0) return new Exception("source Can't Read");
            if (traget == null) return new ArgumentNullException("traget");
            if (!traget.CanWrite) return new Exception("traget Can't Write");
            if (!traget.CanSeek && traget.Position != 0) return new Exception("traget Can't Read");
            if (source is MemoryStream)
            {
                var sm = (MemoryStream)source;
                sm.WriteTo(traget);
            }
            else
            {
                if (!source.CanSeek) return new Exception("source Can't Seek");
                var _buffer = traget.ToArray();
                source.WriteBytes(_buffer);
            }
            return true;
        }

        /// <summary>
        /// 使用指定编码向当前Stream写入字符串
        /// </summary>
        /// <param name="stream">当前Stream</param>
        /// <param name="str">要写入的字符串</param>
        /// <param name="encoding">编码</param>
        public static TryResult WriteString(this Stream stream, string str, Encoding encoding = null)
        {
            if (stream == null) return new ArgumentNullException("stream");
            if (!stream.CanWrite) return new Exception("Can't Write");
            if (string.IsNullOrWhiteSpace(str)) return new ArgumentNullException("str");
            var useencoding = encoding ?? Encoding.UTF8;
            byte[] buffter = useencoding.GetBytes(str);
            stream.Write(buffter, 0, buffter.Length);
            return true;
        }

        /// <summary>
        /// 使用指定编码读出当前Stream所对应的字符串
        /// </summary>
        /// <param name="stream">当前Stream</param> 
        /// <param name="encoding">编码</param>
        public static string ReadString(this Stream stream, Encoding encoding = null)
        {
            var buffter = stream.ToArray();
            if (buffter.Length == 0) return string.Empty;
            var useencoding = encoding ?? Encoding.UTF8;
            return useencoding.GetString(buffter, 0, buffter.Length);
        }

        /// <summary>
        /// 读取当前Stream所有byte
        /// </summary>
        /// <param name="stream">当前Stream</param>
        /// <returns>字节数组</returns>
        public static byte[] ReadToEnd(this Stream stream)
        {
            if (stream == null || !stream.CanRead) return new byte[0];
            byte[] bytes = new byte[stream.Length - stream.Position];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
        const int Nchar = 10;
        const int Rchar = 13;
        const int Lenght = 2048;
        /// <summary>
        /// 使用指定编码读出当前Stream所对应的字符串，支持大文件
        /// </summary>
        /// <param name="stream">当前Stream</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string ReadLastLine(this Stream stream, Encoding encoding = null)
        {
            if (stream == null || !stream.CanRead || !stream.CanSeek || stream.Length == 0) return string.Empty;
            var useencoding = encoding ?? Encoding.UTF8;
            stream.Seek(-1, SeekOrigin.End);

            bool islast = false;
            var hasMulLine = false;
            int index = Lenght;
            byte[] buffter = new byte[Lenght];
            StringBuilder stringBuilder = new StringBuilder();
            while (!islast)
            {
                if (stream.Position == 0) break;

                var by = (byte)stream.ReadByte();

                switch (by)
                {
                    case Nchar:
                    case Rchar:
                        islast = true; break;
                    default:
                        index--;
                        buffter[index] = by;
                        if (index == 0)
                        {
                            hasMulLine = true;
                            stringBuilder.Insert(0, useencoding.GetString(buffter, index, Lenght - index));
                            index = Lenght;
                        }
                        stream.Seek(-2, SeekOrigin.Current);
                        break;
                }
            }
            if (!hasMulLine) return useencoding.GetString(buffter, index, Lenght - index);
            stringBuilder.Insert(0, useencoding.GetString(buffter, index, Lenght - index));
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 设置游标跳到当前Stream的起始位置
        /// </summary>
        /// <param name="stream">当前Stream</param>
        /// <returns></returns>
        public static bool TrySeek(this Stream stream)
        {
            if (stream == null || !stream.CanRead) return false;
            if (!stream.CanSeek) return false;
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);
            return true;
        }
    }
}

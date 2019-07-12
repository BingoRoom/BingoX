using System.IO;

namespace BingoX.Helper
{
    public static class IOHelper
    {
        public static byte[] ToArray(this Stream stream)
        {
            if (stream == null)
            {
                throw new System.ArgumentNullException("name");
            }

            if (stream is MemoryStream)
            {
                var ms = stream as MemoryStream;
                return ms.ToArray();
            }
            if (stream.CanRead && stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                return buffer;
            }
            throw new IOException("不能转换成流");
        }
    }
}

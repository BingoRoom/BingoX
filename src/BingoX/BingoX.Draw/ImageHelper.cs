using System.Drawing;
using System.IO;

namespace BingoX.Draw
{
    public static class ImageHelper
    {
        public static Image ToImage(this byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);


            return Image.FromStream(ms);
        }
        public static byte[] ToArray(this Image image)
        {
            MemoryStream ms = new MemoryStream();

            image.Save(ms, image.RawFormat);
            var buffer = ms.ToArray();
            ms.Dispose();
            return buffer;
        }
        /// <summary>
        /// 以中为添加或缩小
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static RectangleF Add(this RectangleF source, float width)
        {
            RectangleF rectangle = new RectangleF(source.Location - new SizeF(width, width), source.Size + new SizeF(width * 2, width * 2));
            return rectangle;
        }
        /// <summary>
        /// 以中为添加或缩小
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Rectangle Add(this Rectangle source, int width)
        {
            Rectangle rectangle = new Rectangle(source.Location - new Size(width, width), source.Size + new Size(width * 2, width * 2));
            return rectangle;
        }
        /// <summary>
        /// 以中为添加或缩小
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static RectangleF Add(this RectangleF source, SizeF size)
        {
            RectangleF rectangle = new RectangleF(source.Location - size, source.Size + new SizeF(size.Width * 2, size.Height * 2));
            return rectangle;
        }
        /// <summary>
        /// 以中为添加或缩小
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Rectangle Add(this Rectangle source, Size size)
        {
            Rectangle rectangle = new Rectangle(source.Location - size, source.Size + new Size(size.Width * 2, size.Height * 2));
            return rectangle;
        }
    }
}

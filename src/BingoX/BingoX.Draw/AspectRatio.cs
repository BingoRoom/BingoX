using System.Drawing;

namespace BingoX.Draw
{
    /// <summary>
    /// 長寬比
    /// </summary>
    public struct AspectRatio
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public AspectRatio(int width, int height)
        {
            Width = width;
            Height = height;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Height { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}", Width, Height);
        }
        /// <summary>
        /// 長寬比
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static AspectRatio FormSize(int width, int height)
        {
            var gcd = DrawUtility.GCD(width, height);
            return new AspectRatio(width / gcd, height / gcd);
        }
        /// <summary>
        /// 長寬比
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static AspectRatio FormSize(Size size)
        {
            return FormSize(size.Width, size.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public static implicit operator AspectRatioF(AspectRatio p)
        {
            return new AspectRatioF(p.Width, p.Height);
        }
    }
}

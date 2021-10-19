using System.Drawing;

namespace BingoX.Draw
{
    /// <summary>
    /// 長寬比
    /// </summary>
    public struct AspectRatioF
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public AspectRatioF(float width, float height)
        {
            Width = width;
            Height = height;
        }
        /// <summary>
        /// 
        /// </summary>
        public float Width { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public float Height { get; private set; }
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
        public static AspectRatioF FormSize(float width, float height)
        {
            var gcd = DrawUtility.GCD(width, height);
            var tempwidth = width / gcd;
            var tempheight = height / gcd;
            if (tempwidth >= 100)
            {
                var length = tempwidth.ToString().Length - 2;
                var xu = (float)System.Math.Pow(10, length);
                tempwidth = tempwidth / xu;
                tempheight = tempheight / xu;
            }
            if (tempheight >= 100)
            {
                var length = tempheight.ToString().Length - 2;
                var xu = (float)System.Math.Pow(10, length);
                tempwidth = tempwidth / xu;
                tempheight = tempheight / xu;
            }
            return new AspectRatioF(tempwidth, tempheight);
        }
        /// <summary>
        /// 長寬比
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static AspectRatioF FormSize(SizeF size)
        {
            return FormSize(size.Width, size.Height);
        }
    }
}

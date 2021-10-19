using System.Drawing;

namespace BingoX.Draw
{
    /// <summary>
    /// 
    /// </summary>
    public class ResolutionEntry
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Size Size { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AspectRatio Ratio { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static readonly ResolutionEntry HD1080P = new ResolutionEntry() { Name = "1080P", Size = new Size(1920, 1080), Ratio = new AspectRatio(16, 9) };

        /// <summary>
        /// 
        /// </summary>
        public static readonly ResolutionEntry HD720P = new ResolutionEntry() { Name = "720P", Size = new Size(1280, 720), Ratio = new AspectRatio(16, 9) };
        /// <summary>
        /// 
        /// </summary>
        public static readonly ResolutionEntry HD4K = new ResolutionEntry() { Name = "4K", Size = new Size(4096, 2160), Ratio = new AspectRatio(17, 9) };
        /// <summary>
        /// 
        /// </summary>
        public static readonly ResolutionEntry HD8K = new ResolutionEntry() { Name = "8K", Size = new Size(7680, 4320), Ratio = new AspectRatio(16, 9) };







    }
}

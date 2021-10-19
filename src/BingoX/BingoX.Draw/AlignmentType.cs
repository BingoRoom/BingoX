using System;
using System.ComponentModel;

namespace BingoX.Draw
{
    /// <summary>
    /// 方向
    /// </summary>
    [Flags]
    public enum AlignmentType
    {
        /// <summary>
        /// 垂直
        /// </summary>
        [Description("垂直")]
        Horizontally = 1,

        /// <summary>
        /// 橫向
        /// </summary>
        [Description("橫向")]
        Vertically = 2,
    }
}

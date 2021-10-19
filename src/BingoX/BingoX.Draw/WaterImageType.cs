using System;
using System.ComponentModel;

namespace BingoX.Draw
{
    /// <summary>
    ///
    /// </summary>
    public enum WaterImageType
    {
        /// <summary>
        ///
        /// </summary>
        [Description("保持像素")]
        Pixel,

        /// <summary>
        ///
        /// </summary>
        [Description("填充")]
        Full,

        /// <summary>
        ///
        /// </summary>
        [Description("平鋪")]
        Tile,

        /// <summary>
        ///
        /// </summary>
        [Description("文字")]
        Text,
    }
}

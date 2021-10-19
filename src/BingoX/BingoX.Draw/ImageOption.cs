using System.ComponentModel;
using System.Drawing;

namespace BingoX.Draw
{
    /// <summary>
    ///
    /// </summary>
    public class ImageOption
    {
        private float _opacity = 1;
        /// <summary>
        ///
        /// </summary>
        /// <exception cref="ImageException"></exception>

        [Description("透明度，值必須在0-1之間。"), DisplayName("透明度")]
        public float Opacity
        {
            get { return _opacity; }
            set
            {
                if (value > 1 || _opacity < 0) throw new ImageException("超出範圍，值必須在0-1之間。");
                _opacity = value;
            }
        }

        /// <summary>
        ///
        /// </summary>

        [Description("新尺寸，新圖的尺寸。"), DisplayName("新尺寸")]
        public Size? TragetSize { get; set; }
    }
}

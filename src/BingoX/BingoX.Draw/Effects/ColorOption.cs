using System.ComponentModel;

namespace BingoX.Draw.Effects
{
    /// <summary>
    ///
    /// </summary>
    [Description("顏色選項"), DisplayName("顏色選項")]
    public class ColorOption : ImageOption
    {
        /// <summary>
        /// 廠
        /// </summary>
        [Description("RGB:紅"), DisplayName("紅"), Category("濾鏡選項")]
        public int Red { get; set; }

        /// <summary>
        /// 鄭
        /// </summary>
        [Description("RGB:綠"), DisplayName("綠"), Category("濾鏡選項")]
        public int Green { get; set; }

        /// <summary>
        /// 芅
        /// </summary>
        [Description("RGB:藍"), DisplayName("藍"), Category("濾鏡選項")]
        public int Blue { get; set; }
    }
    
}

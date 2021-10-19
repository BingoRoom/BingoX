using System.ComponentModel;

namespace BingoX.Draw.Effects
{
    /// <summary>
    ///
    /// </summary>
    [Description("值選擇"), DisplayName("值選擇")]
    public class ValueOption : ImageOption
    {
        /// <summary>
        ///
        /// </summary>
        [Description("值"), DisplayName("值"), Category("濾鏡選項")]
        public float Value { get; set; }
    }

}

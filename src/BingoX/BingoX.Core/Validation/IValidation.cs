using System.ComponentModel;

namespace BingoX.Validation
{
    public interface IValidation<TIn>
    {
        bool Valid(TIn value);
    }
    public enum IDCardGender
    {
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        Male,
        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        Female
    }
}

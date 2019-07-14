using System.ComponentModel;

namespace BingoX.Validation
{
    public interface IValidation<TIn>
    {
        bool Valid(TIn value);
    }
}

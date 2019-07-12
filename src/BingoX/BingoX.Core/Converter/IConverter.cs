namespace BingoX.Converter
{
    public interface IConverter<TIn, TOut>
    {
        TOut To(TIn value);
    }
}

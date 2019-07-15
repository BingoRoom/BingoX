namespace BingoX.ComponentModel.FastReflection
{
    public interface IFastReflectionCache<TKey, TValue>
    {
        TValue Get(TKey key);
    }
}

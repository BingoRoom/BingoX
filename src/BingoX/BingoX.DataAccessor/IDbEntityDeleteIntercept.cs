namespace BingoX.DataAccessor
{
    public interface IDbEntityDeleteIntercept : IDbEntityIntercept
    {

        void OnDelete(DbEntityDeleteInfo info);


    }
}
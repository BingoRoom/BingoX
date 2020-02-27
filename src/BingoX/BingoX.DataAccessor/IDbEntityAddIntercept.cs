namespace BingoX.DataAccessor
{
    public interface IDbEntityAddIntercept : IDbEntityIntercept
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="info"></param>
        void OnAdd(DbEntityCreateInfo info);

    }
}
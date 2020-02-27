namespace BingoX.DataAccessor
{
    public interface IDbEntityModifiyIntercept : IDbEntityIntercept
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="info"></param>
        void OnModifiy(DbEntityChangeInfo info);

    }
}
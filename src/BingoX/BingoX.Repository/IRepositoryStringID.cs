using BingoX.Domain;

namespace BingoX.Repository
{
    /// <summary>
    /// 支持字符串主键的仓储接口
    /// </summary>
    /// <typeparam name="TDomain">领域实体</typeparam>
    public interface IRepositoryStringID<TDomain> : IRepository<TDomain, string> where TDomain : class, IStringEntity<TDomain>
    {
        new TDomain GetId(string id);

        new bool Exist(string id);

        new int Delete(string[] pkArray);
    }

    public class RepositoryStringID<TDomain> : Repository<TDomain, string>, IRepositoryStringID<TDomain> where TDomain : class, IStringEntity<TDomain>
    {
        public RepositoryStringID(RepositoryContextOptions options) : base(options)
        {

        }
        public override TDomain GetId(string id)
        {
            return base.GetId(id);
        }

        public override bool Exist(string id)
        {
            return base.Exist(id);
        }

        public override int Delete(string[] pkArray)
        {
            return base.Delete(pkArray);
        }
    }
}
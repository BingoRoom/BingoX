using BingoX.Domain;
using System;

namespace BingoX.Repository
{
    /// <summary>
    /// 支持GUID主键的仓储接口
    /// </summary>
    /// <typeparam name="TDomain">领域实体</typeparam>
    public interface IRepositoryGuid<TDomain> : IRepository<TDomain, Guid> where TDomain : class, IGuidEntity<TDomain>
    {
        new TDomain GetId(Guid id);

        new bool Exist(Guid id);

        new int Delete(Guid[] pkArray);
    }

    public class RepositoryGuid<TDomain> : Repository<TDomain, Guid>, IRepositoryGuid<TDomain> where TDomain : class, IGuidEntity<TDomain>
    {
        public RepositoryGuid(RepositoryContextOptions options) : base(options)
        {

        }

        public override TDomain GetId(Guid id)
        {
            return base.GetId(id);
        }

        public override bool Exist(Guid id)
        {
            return base.Exist(id);
        }

        public override int Delete(Guid[] pkArray)
        {
            return base.Delete(pkArray);
        }
    }
}
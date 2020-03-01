using BingoX.Domain;

namespace BingoX.Repository
{
    /// <summary>
    /// 表示一个领域聚合仓储
    /// </summary>
    /// <typeparam name="TDomain">领域聚合</typeparam>
    public interface IAggregateRepository<TDomain> : IRepository where TDomain : IAggregate
    {

    }

}
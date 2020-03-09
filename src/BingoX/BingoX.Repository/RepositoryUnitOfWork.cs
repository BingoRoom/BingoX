using BingoX.DataAccessor;
using System;
using System.Collections.Generic;

namespace BingoX.Repository
{
    public sealed class RepositoryUnitOfWork : IRepositoryUnitOfWork
    {
        readonly IList<IUnitOfWork> units = new List<IUnitOfWork>();
        public void Commit()
        {
            try
            {
                foreach (var item in units)
                {
                    item.BeginTransaction();
                }
                foreach (var item in units)
                {
                    item.SaveChanges();
                }
                foreach (var item in units)
                {
                    item.Commit();
                }
            }
            catch (Exception ex)
            {
                Rollback();
                throw new DataAccessorException("提交事务失败", ex);
            }
        }

        public void Rollback()
        {
            try
            {
                foreach (var item in units)
                {
                    item.Rollback();
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessorException("回滚事务失败", ex);
            }
        }

        internal void Add(IUnitOfWork unitOfWork)
        {
            if (units.Contains(unitOfWork)) return;
            units.Add(unitOfWork);
        }

        internal void Reomve(IUnitOfWork unitOfWork)
        {
            units.Remove(unitOfWork);
        }


    }
}
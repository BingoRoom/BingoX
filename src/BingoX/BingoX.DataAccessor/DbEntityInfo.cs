
using System;

namespace BingoX.DataAccessor
{
    public class DbEntityInfo
    {
        public DbEntityInfo(object entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entity = entity;
        }
        public bool Accept { get; set; }
        public object Entity { get; private set; }
    }
}
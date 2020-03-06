
using System.Linq;
using BingoX.Helper;
using System;
using System.Collections.Generic;

namespace BingoX.DataAccessor.SqlSugar
{
    /// <summary>
    /// 数据库拦截器管理器
    /// </summary>
    public class SqlSugarDbEntityInterceptManagement : DbEntityInterceptManagement
    {
        public SqlSugarDbEntityInterceptManagement(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }


        private static IDictionary<string, object> ToDic(SqlSugarPropertyValues dbPropertyValues)
        {
            IDictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            foreach (var item in dbPropertyValues.Properties)
            {
                if (!dic.ContainsKey(item)) dic.Add(item, dbPropertyValues[item]);
            }
            return dic;
        }


        public void Interceptor(SqlSugarEntityEntry entityEntry)

        {
            var attributes = GetAttributes(entityEntry.Entity.GetType());
            if (attributes.IsEmpty()) return;
            var aops = attributes.Select(n =>
            {
                if (n.Intercept != null) return n.Intercept;
                var intercept = serviceProvider.GetService(n.AopType) as IDbEntityIntercept;
                return intercept;
            }).ToArray();
            switch (entityEntry.State)
            {

                case SqlSugarEntityState.Unchanged:
                    break;
                case SqlSugarEntityState.Deleted:
                    {
                        var flagAccept = aops.OfType<IDbEntityDeleteIntercept>().All(n =>
                        {
                            var info = new SqlSugarDbEntityDeleteInfo(entityEntry);
                            n.OnDelete(info);
                            return info.Accept;
                        });
                        if (!flagAccept) entityEntry.State = SqlSugarEntityState.Unchanged;
                        break;
                    }
                case SqlSugarEntityState.Modified:
                    {
                        var currentValues = ToDic(entityEntry.CurrentValues);
                        var originalValues = ToDic(entityEntry.OriginalValues);
                        var changeValues = currentValues.GetChanges(originalValues);
                        var flagAccept = aops.OfType<IDbEntityModifiyIntercept>().All(n =>
                        {
                            var info = new SqlSugarDbEntityChangeInfo(entityEntry, currentValues, originalValues, changeValues);
                            n.OnModifiy(info);
                            return info.Accept;
                        });
                        if (flagAccept)
                        {
                            entityEntry.CurrentValues.SetValues(changeValues);
                        }
                        else
                        {
                            entityEntry.State = SqlSugarEntityState.Unchanged;
                        }
                        break;
                    }
                case SqlSugarEntityState.Added:
                    {
                        var currentValues = ToDic(entityEntry.CurrentValues);
                        var flagAccept = aops.OfType<IDbEntityAddIntercept>().All(n =>
                        {
                            var info = new SqlSugarDbEntityCreateInfo(entityEntry, currentValues);
                            n.OnAdd(info);
                            return info.Accept;
                        });
                        if (flagAccept)
                        {
                            entityEntry.CurrentValues.SetValues(currentValues);
                        }
                        else
                        {
                            entityEntry.State = SqlSugarEntityState.Unchanged;
                        }
                        break;
                    }
                default:
                    break;
            }
        }
    }
#if Standard
#endif
}

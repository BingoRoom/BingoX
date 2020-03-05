#if Standard
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
#else

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
#endif
using System.Linq;
using BingoX.Helper;
using System;
using System.Collections.Generic;

namespace BingoX.DataAccessor.EF
{
    /// <summary>
    /// 数据库拦截器管理器
    /// </summary>
    public class EfDbEntityInterceptManagement : DbEntityInterceptManagement
    {
        public EfDbEntityInterceptManagement(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            
        }
        
       
#if Standard
        private static IDictionary<string, object> ToDic(PropertyValues dbPropertyValues)
        {
            IDictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            foreach (var item in dbPropertyValues.Properties)
            {
                if (!dic.ContainsKey(item.Name)) dic.Add(item.Name, dbPropertyValues[item]);
            }
            return dic;
        }
#else
        private static IDictionary<string, object> ToDic(DbPropertyValues dbPropertyValues)
        {
            IDictionary<string, object> dic = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            foreach (var item in dbPropertyValues.PropertyNames)
            {
                if (!dic.ContainsKey(item)) dic.Add(item, dbPropertyValues[item]);
            }
            return dic;
        }
#endif

#if Standard
        public void Interceptor(EntityEntry entityEntry)
#else
        public void Interceptor(DbEntityEntry entityEntry)
#endif
        {
            var attributes = GetAttributes(entityEntry.Entity.GetType());
            if (attributes.IsEmpty()) return;
            var aops = attributes.Select(n =>
            {
                var intercept = serviceProvider.GetService(n.AopType) as IDbEntityIntercept;
                return intercept;
            });
            switch (entityEntry.State)
            {
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    {
                        var flagAccept = aops.OfType<IDbEntityDeleteIntercept>().All(n =>
                        {
                            var info = new EfDbEntityDeleteInfo(entityEntry);
                            n.OnDelete(info);
                            return info.Accept;
                        });
                        if (!flagAccept) entityEntry.State = EntityState.Unchanged;
                        break;
                    }
                case EntityState.Modified:
                    {
                        var currentValues = ToDic(entityEntry.CurrentValues);
                        var originalValues = ToDic(entityEntry.OriginalValues);
                        var changeValues = currentValues.GetChanges(originalValues);
                        var flagAccept = aops.OfType<IDbEntityModifiyIntercept>().All(n =>
                        {
                            var info = new EfDbEntityChangeInfo(entityEntry, currentValues, originalValues, changeValues);
                            n.OnModifiy(info);
                            return info.Accept;
                        });
                        if (flagAccept)
                        {
                            entityEntry.CurrentValues.SetValues(changeValues);
                        }
                        else
                        {
                            entityEntry.State = EntityState.Unchanged;
                        }
                        break;
                    }
                case EntityState.Added:
                    {
                        var currentValues = ToDic(entityEntry.CurrentValues);
                        var flagAccept = aops.OfType<IDbEntityAddIntercept>().All(n =>
                        {
                            var info = new EfDbEntityCreateInfo(entityEntry, currentValues);
                            n.OnAdd(info);
                            return info.Accept;
                        });
                        if (flagAccept)
                        {
                            entityEntry.CurrentValues.SetValues(currentValues);
                        }
                        else
                        {
                            entityEntry.State = EntityState.Unchanged;
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

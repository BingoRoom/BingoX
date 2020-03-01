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
    public class EfDbEntityInterceptManagement
    {
        private readonly IDictionary<Type, IEnumerable<DbEntityInterceptAttribute>> dictionary = new Dictionary<Type, IEnumerable<DbEntityInterceptAttribute>>();
        private readonly List<DbEntityInterceptAttribute> global = new List<DbEntityInterceptAttribute>();
        private readonly IServiceProvider serviceProvider;

        public EfDbEntityInterceptManagement(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

        }
        T GetService<T>(Type type)
        {
            object obj = null;
#if Standard
            obj = serviceProvider.GetRequiredService(type);
#else
            obj = serviceProvider.GetService(type);
#endif
            if (obj is T) return (T)obj;
            return default(T);
        }
        object GetService(Type type)
        {
            object obj = null;
#if Standard
            obj = serviceProvider.GetRequiredService(type);
#else
            obj = serviceProvider.GetService(type);
#endif
            return obj;
        }

        public IEnumerable<IDbEntityIntercept> GetAops(Type entityType)
        {
            var attributes = GetAttributes(entityType);
            if (attributes.IsEmpty()) return null;
            var aops = attributes.Select(n =>
            {
                var intercept = n.Intercept;
                if (intercept == null) intercept = GetService<IDbEntityIntercept>(n.AopType);

                if (intercept == null)
                {
                    var constructor = n.AopType.GetConstructors().FirstOrDefault();
                    var par = constructor.GetParameters();
                    if (par.Length == 0) intercept = constructor.Invoke(null) as IDbEntityIntercept;
                    else
                    {
                        var parms = par.Select(x => GetService(x.ParameterType)).ToArray();
                        intercept = constructor.Invoke(parms) as IDbEntityIntercept;
                    }
                }
                return intercept;
            }).Where(n => n != null);
            return aops;
        }

        public IEnumerable<DbEntityInterceptAttribute> GetAttributes(Type entityType)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            IEnumerable<DbEntityInterceptAttribute> intercepts;
            if (dictionary.ContainsKey(entityType))
            {
                intercepts = dictionary[entityType];
            }
            else
            {
                intercepts = entityType.GetCustomAttributesIncludeBaseType<DbEntityInterceptAttribute>().ToArray();
                dictionary.Add(entityType, intercepts);
            }
            return global.Union(intercepts).ToArray();
        }

        public void AddGlobalIntercept(Type dbEntityIntercept)
        {
            if (!typeof(IDbEntityIntercept).IsAssignableFrom(dbEntityIntercept)) throw new LogicException("类型不为IDbEntityIntercept");
            global.Add(new DbEntityInterceptAttribute(dbEntityIntercept));
        }
        public void AddGlobalIntercept(DbEntityInterceptAttribute dbEntityIntercept)
        {
            global.Add(dbEntityIntercept);
        }
        public void AddRangeGlobalIntercepts(IEnumerable<DbEntityInterceptAttribute> dbEntityIntercepts)
        {
            global.AddRange(dbEntityIntercepts);
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

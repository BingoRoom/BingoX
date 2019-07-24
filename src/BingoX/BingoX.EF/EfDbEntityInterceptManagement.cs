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
using BingoX.Repository;
using BingoX.Helper;
using System;
using System.Collections.Generic;

namespace BingoX.EF
{
#if Standard

    public class EfDbEntityDeleteInfo : DbEntityDeleteInfo
    {
        private readonly EntityEntry entityEntry;

        public EfDbEntityDeleteInfo(EntityEntry entityEntry) : base(entityEntry.Entity)
        {
            this.entityEntry = entityEntry;
        }
    }
    public class EfDbEntityCreateInfo : DbEntityCreateInfo
    {
        private readonly EntityEntry entityEntry;

        public EfDbEntityCreateInfo(EntityEntry entityEntry, IDictionary<string, object> currentValues)
            : base(entityEntry.Entity, currentValues)
        {
            this.entityEntry = entityEntry;
        }
        public override void SetValue(string name, object value)
        {

            if (CurrentValues.ContainsKey(name)) CurrentValues[name] = value;
        }
    }
    public class EfDbEntityChangeInfo : DbEntityChangeInfo
    {
        private readonly EntityEntry entityEntry;

        public EfDbEntityChangeInfo(EntityEntry entityEntry, IDictionary<string, object> currentValues, IDictionary<string, object> originalValues, IDictionary<string, object> changeValues)
            : base(entityEntry.Entity, currentValues, originalValues, changeValues)
        {
            this.entityEntry = entityEntry;
        }
        public override void SetValue(string name, object value)
        {
            if (ChangeValues.ContainsKey(name)) ChangeValues[name] = value;
            else if (CurrentValues.ContainsKey(name))
            {
                ChangeValues.Add(name, value);
            }
        }
    }
    public class EfDbEntityInterceptManagement
    {
        readonly IDictionary<Type, IDbEntityIntercept[]> dictionary = new Dictionary<Type, IDbEntityIntercept[]>();

        public IDbEntityIntercept[] GetAops(Type entityType)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }


            if (dictionary.ContainsKey(entityType))
            {
                return dictionary[entityType];
            }

            //var allatts = entityType.GetCustomAttributes<AopEntityAttribute>();
            var atts = DbEntityInterceptServiceCollectionExtensions.Options.Intercepts.Union(entityType.GetCustomAttributesIncludeBaseType<DbEntityInterceptAttribute>()).OfType<DbEntityInterceptAttribute>();

            if (atts.IsEmpty()) return BingoX.Utility.EmptyUtility<IDbEntityIntercept>.EmptyArray;
            var aops = atts.Select(n =>
            {
                var constructor = n.AopType.GetConstructors().First();
                var paramters = constructor.GetParameters().Select(x => DbEntityInterceptServiceCollectionExtensions.ApplicationServices.GetRequiredService(x.ParameterType)).ToArray();
                return constructor.FastInvoke<IDbEntityIntercept>(paramters);
            }
            ).ToArray();

            dictionary.Add(entityType, aops);
            return aops;
        }

        private static IDictionary<string, object> ToDic(PropertyValues dbPropertyValues)
        {
            IDictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var item in dbPropertyValues.Properties)
            {
                dic.Add(item.Name, dbPropertyValues[item]);
            }
            return dic;
        }


        //private static IDictionary<string, object> ToDic(DbPropertyValues dbPropertyValues)
        //{
        //    IDictionary<string, object> dic = new Dictionary<string, object>();
        //    foreach (var item in dbPropertyValues.PropertyNames)
        //    {
        //        dic.Add(item, dbPropertyValues[item]);
        //    }
        //    return dic;
        //}



        public void Interceptor(EntityEntry entityEntry)

        //public static void EntityInterceptor(DbEntityEntry entityEntry)

        {
            var aops = GetAops(entityEntry.Entity.GetType());
            if (aops.IsEmpty()) return;

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
#endif
}

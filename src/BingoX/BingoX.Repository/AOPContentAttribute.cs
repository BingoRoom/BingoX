
using System;
using System.Collections.Generic;

namespace BingoX.Repository
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class DbEntityInterceptAttribute : Attribute, IDbEntityIntercept
    {
        public DbEntityInterceptAttribute(Type aopType, int order = 0)
        {
            if (aopType == null)
            {
                throw new ArgumentNullException(nameof(aopType));
            }
            if (!typeof(IDbEntityIntercept).IsAssignableFrom(aopType)) throw new RepositoryException("类型错误，没有实现接口BingoX.Repository.IDbEntityIntercept");
            AopType = aopType;
            Order = order;
        }

        public Type AopType { get; private set; }
        public int Order { get; private set; }
    }

    [Serializable]
    public class RepositoryException : System.Data.Common.DbException
    {
        public RepositoryException() { }
        public RepositoryException(string message) : base(message) { }
        public RepositoryException(string message, Exception inner) : base(message, inner) { }
        protected RepositoryException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    public interface IDbEntityIntercept
    {





    }

    public interface IDbEntityAddIntercept : IDbEntityIntercept
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="info"></param>
        void OnAdd(DbEntityCreateInfo info);

    }

    public interface IDbEntityDeleteIntercept : IDbEntityIntercept
    {

        void OnDelete(DbEntityDeleteInfo info);


    }

    public interface IDbEntityModifiyIntercept : IDbEntityIntercept
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="info"></param>
        void OnModifiy(DbEntityChangeInfo info);

    }
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
    public class DbEntityDeleteInfo : DbEntityInfo
    {
        public DbEntityDeleteInfo(object entity) : base(entity)
        {

        }

    }
    public class DbEntityCreateInfo : DbEntityInfo
    {
        public DbEntityCreateInfo(object entity, IDictionary<string, object> currentValues) : base(entity)
        {


            if (currentValues == null)
            {
                throw new ArgumentNullException(nameof(currentValues));
            }

            CurrentValues = currentValues;
        }
        public IDictionary<string, object> CurrentValues { get; private set; }

        public virtual void SetValue(string name, object value)
        {
            if (CurrentValues.ContainsKey(name)) CurrentValues[name] = value;
        }
    }
    public class DbEntityChangeInfo : DbEntityInfo
    {
        public DbEntityChangeInfo(object entity, IDictionary<string, object> currentValues, IDictionary<string, object> originalValues, IDictionary<string, object> changeValues) : base(entity)
        {



            if (currentValues == null)
            {
                throw new ArgumentNullException(nameof(currentValues));
            }

            if (originalValues == null)
            {
                throw new ArgumentNullException(nameof(originalValues));
            }

            if (changeValues == null)
            {
                throw new ArgumentNullException(nameof(changeValues));
            }

            CurrentValues = currentValues;
            OriginalValues = originalValues;
            ChangeValues = changeValues;

        }
        public IDictionary<string, object> CurrentValues { get; private set; }
        public IDictionary<string, object> OriginalValues { get; private set; }
        public IDictionary<string, object> ChangeValues { get; private set; }
        public virtual void SetValue(string name, object value)
        {
            if (ChangeValues.ContainsKey(name)) ChangeValues[name] = value;
            else if (CurrentValues.ContainsKey(name))
            {
                ChangeValues.Add(name, value);
            }
        }
    }
}
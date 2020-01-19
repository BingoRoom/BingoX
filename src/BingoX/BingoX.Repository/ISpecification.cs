using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BingoX.Repository
{
    /// <summary>
    /// 数据操作规格
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISpecification<T> where T : class
    {

        /// <summary>
        /// 当前页码
        /// </summary>
        int PageIndex { get; set; }
        /// <summary>
        /// 页长
        /// </summary>
        int PageSize { get; set; }
        /// <summary>
        /// 数据操作规格表达式
        /// </summary>
        /// <returns></returns>
        Expression<Func<T, bool>> ToExpression();
        /// <summary>
        /// 数据排序规格表达式
        /// </summary>
        /// <returns></returns>
        OrderModelField<T>[] ToStorExpression();
        /// <summary>
        /// 判断数据操作规格是否有效
        /// </summary>
        /// <param name="entity">数据库实体</param>
        /// <returns>是否有效</returns>
        bool IsSatisfiedBy(T entity);


        /// <summary>
        /// 升序排序
        /// </summary>
        /// <param name="orderExpression">排序规格表达式</param>
        ISpecification<T> Orderby(Expression<Func<T, object>> orderExpression, bool desc = false);
        /// <summary>
        /// 降序排序
        /// </summary>
        /// <param name="orderExpression">排序规格表达式</param>
        ISpecification<T> OrderbyAsc(Expression<Func<T, object>> orderExpression);
        /// <summary>
        /// 降序排序
        /// </summary>
        /// <param name="orderExpression">排序规格表达式</param>
        ISpecification<T> OrderbyDesc(Expression<Func<T, object>> orderExpression);
        /// <summary>
        /// 构建逻辑与规格
        /// </summary>
        /// <param name="specification">规格操作数</param>
        /// <returns></returns>
        ISpecification<T> And(ISpecification<T> specification);
        /// <summary>
        /// 构建逻辑非规格
        /// </summary>
        /// <param name="specification">规格操作数</param>
        /// <returns></returns>
        ISpecification<T> Not(ISpecification<T> specification);
        /// <summary>
        /// 构建逻辑或规格
        /// </summary>
        /// <param name="specification">规格操作数</param>
        /// <returns></returns>
        ISpecification<T> Or(ISpecification<T> specification);
        /// <summary>
        /// 构建逻辑与规格
        /// </summary>
        /// <param name="expression">表达式操作数</param>
        /// <returns></returns>
        ISpecification<T> And(Expression<Func<T, bool>> expression);
        /// <summary>
        /// 构建逻辑或规格
        /// </summary>
        /// <param name="expression">表达式操作数</param>
        /// <returns></returns>
        ISpecification<T> Or(Expression<Func<T, bool>> expression);
        /// <summary>
        /// 构建逻辑非规格
        /// </summary>
        /// <param name="expression">表达式操作数</param>
        /// <returns></returns>
        ISpecification<T> Not(Expression<Func<T, bool>> expression);
    }

    /// <summary>
    /// 动态数据查询操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDynamicSpecification<T> : ISpecification<T> where T : class
    {
        ISpecification<T> And(string propertyName, object value);

        ISpecification<T> And(IDictionary<string, object> dictionary);
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="orderExpression">属性名称</param>
        ISpecification<T> Orderby(string propertyName, bool desc = false);
        /// <summary>
        /// 升序排序
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        ISpecification<T> OrderbyAsc(string propertyName);
        /// <summary>
        /// 升序排序
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        ISpecification<T> OrderbyDesc(string propertyName);
    }
    public struct OrderModelField<T>
    {
        public bool IsDesc { get; set; }
        public Expression<Func<T, object>> OrderPredicates { get; set; }
    }

    [Serializable]
    public class SpecificationException : Exception
    {
        public SpecificationException() { }
        public SpecificationException(string message) : base(message) { }
        public SpecificationException(string message, Exception inner) : base(message, inner) { }
        protected SpecificationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    internal class BuildDynamicExpression
    {
        static BuildDynamicExpression()
        {
            equalsDic = new Dictionary<Type, MethodInfo>
                        {
                            {typeof(string) , typeof(string).GetMethods().LastOrDefault(n => n.Name =="Equals"&& n.GetParameters().Length==1) },
                            {typeof(int) , typeof(int).GetMethods().LastOrDefault(n => n.Name =="Equals") },
                            {typeof(long) , typeof(long).GetMethods().LastOrDefault(n => n.Name =="Equals") },
                            {typeof(short) , typeof(short).GetMethods().LastOrDefault(n => n.Name =="Equals") },
                            {typeof(uint) , typeof(uint).GetMethods().LastOrDefault(n => n.Name =="Equals") },
                            {typeof(ulong) , typeof(ulong).GetMethods().LastOrDefault(n => n.Name =="Equals") },
                            {typeof(ushort) , typeof(ushort).GetMethods().LastOrDefault(n => n.Name =="Equals") },
                            {typeof(byte) , typeof(byte).GetMethods().LastOrDefault(n => n.Name =="Equals") },
                            {typeof(char) , typeof(char).GetMethods().LastOrDefault(n => n.Name =="Equals") },
                            {typeof(double) , typeof(double).GetMethods().LastOrDefault(n => n.Name =="Equals") },
                            {typeof(decimal) , typeof(decimal).GetMethods().LastOrDefault(n => n.Name =="Equals") },
                            {typeof(DateTime) , typeof(DateTime).GetMethods().LastOrDefault(n => n.Name =="Equals") },
                       };
            var method = typeof(Queryable).GetMethods().FirstOrDefault(n => n.Name == "Contains");
            containsDic = new Dictionary<Type, MethodInfo>
                       {
                             {typeof(string) ,typeof(string).GetMethods().FirstOrDefault(n => n.Name == "Contains") },
                            {typeof(int) , method.MakeGenericMethod(typeof(int))  },
                            {typeof(long) , method.MakeGenericMethod(typeof(long))  },
                            {typeof(short) , method.MakeGenericMethod(typeof(short))  },
                            {typeof(uint) , method.MakeGenericMethod(typeof(uint))  },
                            {typeof(ulong) , method.MakeGenericMethod(typeof(ulong))  },
                            {typeof(ushort) , method.MakeGenericMethod(typeof(ushort))  },
                            {typeof(byte) , method.MakeGenericMethod(typeof(byte))  },
                            {typeof(char) , method.MakeGenericMethod(typeof(char))  },
                            {typeof(double) , method.MakeGenericMethod(typeof(double))  },
                            {typeof(decimal) , method.MakeGenericMethod(typeof(decimal))  },
                            {typeof(DateTime) , method.MakeGenericMethod(typeof(DateTime))  },


                       };

        }
        static readonly IDictionary<Type, MethodInfo> equalsDic;
        static readonly IDictionary<Type, MethodInfo> containsDic;
        static readonly IDictionary<Type, Dictionary<string, PropertyInfo>> propertyDic = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
        public static MethodInfo GetContains(Type propertyType)
        {
            if (containsDic.ContainsKey(propertyType)) return containsDic[propertyType];
            throw new SpecificationException("不支持类型" + propertyType.Name);
        }
        public static MethodInfo GetEquals(Type propertyType)
        {
            if (equalsDic.ContainsKey(propertyType)) return equalsDic[propertyType];
            throw new SpecificationException("不支持类型" + propertyType.Name);

        }
        public static MethodInfo GetMethod(Type propertyType)
        {
            if (propertyType == typeof(string))
            {
                switch (DynamicSpecificationOption.StringCondition)
                {
                    case StringCondition.Contains:
                        return GetContains(propertyType);
                    case StringCondition.Equals:
                        return GetEquals(propertyType);
                    default:
                        throw new SpecificationException("未支持类型");
                }
            }

            return GetEquals(propertyType);

        }
        public static ParameterExpression CreateParameterExpression(Type studentType)
        {

            ParameterExpression param = Expression.Parameter(studentType, "x");
            return param;
        }
        public static MemberExpression GetPropertyExpression(ParameterExpression param, string propertyName)
        {
            Dictionary<string, PropertyInfo> typePropertyDic = null;
            if (propertyDic.ContainsKey(param.Type))
            {
                typePropertyDic = propertyDic[param.Type];
            }
            else
            {
                typePropertyDic = new Dictionary<string, PropertyInfo>(StringComparer.CurrentCultureIgnoreCase );
                propertyDic.Add(param.Type, typePropertyDic);
            }
            PropertyInfo propertyInfo = null;
            if (typePropertyDic.ContainsKey(propertyName))
            {
                propertyInfo = typePropertyDic[propertyName];
            }
            else
            {
                if (DynamicSpecificationOption.CamelCaseProperty)
                {
                    propertyInfo = param.Type.GetProperties().FirstOrDefault(n => string.Equals(n.Name, propertyName, StringComparison.CurrentCultureIgnoreCase));
                }
                else
                {
                    propertyInfo = param.Type.GetProperty(propertyName);

                }
                if (propertyInfo != null) typePropertyDic.Add(propertyName, propertyInfo);
            }


            if (propertyInfo == null) throw new SpecificationException(propertyName + "字段不存在");
            MemberExpression memberPropExpr = Expression.Property(param, propertyInfo);


            return memberPropExpr;
        }
        public static PropertyInfo GetProperty(ParameterExpression param, string propertyName)
        {
            MemberExpression memberPropExpr = GetPropertyExpression(param, propertyName);
            PropertyInfo property = memberPropExpr.Member as PropertyInfo;

            return property;
        }
        //public enum Condition
        //{
        //    Equals,
        //    Contains
        //}
    }
    internal class BuildDynamicExpression<T> : BuildDynamicExpression
    {


        public static Expression<Func<T, object>> BuildOrderPredicate(string propertyName)
        {

            ParameterExpression param = CreateParameterExpression(typeof(T));
            var propertyExpression = GetPropertyExpression(param, propertyName);

            UnaryExpression expression = Expression.Convert(propertyExpression, typeof(object));
            Expression<Func<T, object>> orderByExpression = Expression.Lambda<Func<T, object>>(expression, param);
            return orderByExpression;
        }
        public static Expression BuildCondition(string propertyName, object value)
        {
            ParameterExpression param = CreateParameterExpression(typeof(T));
            var left = GetPropertyExpression(param, propertyName);
            var method = GetMethod(((PropertyInfo)left.Member).PropertyType);
            //   var type = value.GetType();
            var right = Expression.Constant(value);
            MethodCallExpression callExpression = Expression.Call(left, method, right);
            return callExpression;
        }

        static Expression GetBody(LambdaExpression expression)
        {
            Expression body;
            if (expression.Body is UnaryExpression)
                body = ((UnaryExpression)expression.Body).Operand;
            else
                body = expression.Body;

            return body;
        }
        public static Expression<Func<T, bool>> BuildConditionPredicate(string propertyName, object value)
        {
            if (value is System.Collections.IList) throw new SpecificationException("查询参数不支付数组");

            ParameterExpression param = CreateParameterExpression(typeof(T));
            var containsExpr = BuildCondition(propertyName, value);
            var predicate = Expression.Lambda<Func<T, bool>>(containsExpr, param);
            return predicate;
        }

    }

    public class DynamicSpecificationOption
    {
        static DynamicSpecificationOption()
        {
            CamelCaseProperty = true;
        }
        public static StringCondition StringCondition { get; set; }
        public static bool CamelCaseProperty { get; set; }
    }
    public enum StringCondition
    {
        Contains,
        Equals,
    }
    public class DynamicSpecification<T> : Specification<T>, IDynamicSpecification<T> where T : class
    {

        #region Dynamic condition
        public virtual DynamicSpecification<T> And(IDictionary<string, object> dictionary)
        {
            foreach (var item in dictionary)
            {
                And(item.Key, item.Value);
            }
            return this;
        }

        public virtual DynamicSpecification<T> And(string propertyName, object value)
        {
            if (value is null) return this;


            Expression<Func<T, bool>> expression = BuildDynamicExpression<T>.BuildConditionPredicate(propertyName, value);
            And(expression);
            return this;
        }

        #endregion

        #region order

        public virtual DynamicSpecification<T> OrderbyAsc(string name)
        {
            var orderExpression = BuildDynamicExpression<T>.BuildOrderPredicate(name);
            OrderPredicates.Add(new OrderModelField<T>() { IsDesc = false, OrderPredicates = orderExpression });
            return this;
        }
        public virtual DynamicSpecification<T> OrderbyDesc(string name)
        {
            var orderExpression = BuildDynamicExpression<T>.BuildOrderPredicate(name);
            OrderPredicates.Add(new OrderModelField<T>() { IsDesc = true, OrderPredicates = orderExpression });
            return this;

        }
        public virtual DynamicSpecification<T> Orderby(string name, bool desc = false)
        {
            var orderExpression = BuildDynamicExpression<T>.BuildOrderPredicate(name);
            OrderPredicates.Add(new OrderModelField<T>() { IsDesc = desc, OrderPredicates = orderExpression });
            return this;

        }
        #endregion

        #region MyRegion
        ISpecification<T> IDynamicSpecification<T>.Orderby(string propertyName, bool desc)
        {
            return Orderby(propertyName, desc);
        }

        ISpecification<T> IDynamicSpecification<T>.OrderbyAsc(string propertyName)
        {
            return OrderbyAsc(propertyName);
        }

        ISpecification<T> IDynamicSpecification<T>.OrderbyDesc(string propertyName)
        {
            return OrderbyDesc(propertyName);
        }

        ISpecification<T> IDynamicSpecification<T>.And(string propertyName, object value)
        {
            return And(propertyName, value);
        }
        ISpecification<T> IDynamicSpecification<T>.And(IDictionary<string, object> dictionary)
        {
            return And(dictionary);
        }
        #endregion
    }
    public class Specification<T> : ISpecification<T> where T : class
    {
        public Specification()
        {
            PageIndex = 0;
            PageSize = 20;
        }
        static Expression<Func<T, bool>> True() { return f => true; }
        protected internal Expression<Func<T, bool>> SearchPredicate = True();
        protected internal IList<OrderModelField<T>> OrderPredicates = new List<OrderModelField<T>>();

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public virtual bool IsSatisfiedBy(T entity)
        {
            return SearchPredicate.Compile().Invoke(entity);

        }
        public virtual OrderModelField<T>[] ToStorExpression()
        {
            return OrderPredicates.ToArray();
        }
        public virtual Expression<Func<T, bool>> ToExpression()
        {
            return SearchPredicate;
        }

        public virtual Specification<T> And(Specification<T> rightExpression)
        {
            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.AndAlso(SearchPredicate.Body, rightExpression.SearchPredicate.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            SearchPredicate = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

            return this;
        }
        public virtual Specification<T> Not(Specification<T> specification)
        {


            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.NotEqual(SearchPredicate.Body, specification.SearchPredicate.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            SearchPredicate = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

            return this;
        }

        public virtual Specification<T> Or(Specification<T> specification)
        {




            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.OrElse(SearchPredicate.Body, specification.SearchPredicate.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            SearchPredicate = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

            return this;
        }

        public virtual Specification<T> And(Expression<Func<T, bool>> expression)
        {
            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.AndAlso(SearchPredicate.Body, expression.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            SearchPredicate = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

            return this;
        }

        public virtual Specification<T> Or(Expression<Func<T, bool>> expression)
        {
            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.OrElse(SearchPredicate.Body, expression.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            SearchPredicate = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
            return this;
        }

        public virtual Specification<T> Not(Expression<Func<T, bool>> expression)
        {
            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.NotEqual(SearchPredicate.Body, expression.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            SearchPredicate = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
            return this;
        }


        #region Order

        public virtual Specification<T> OrderbyAsc(Expression<Func<T, object>> orderExpression)
        {
            OrderPredicates.Add(new OrderModelField<T>() { IsDesc = false, OrderPredicates = orderExpression });
            return this;
        }
        public virtual Specification<T> OrderbyDesc(Expression<Func<T, object>> orderExpression)
        {

            OrderPredicates.Add(new OrderModelField<T>() { IsDesc = true, OrderPredicates = orderExpression });
            return this;

        }
        public virtual Specification<T> Orderby(Expression<Func<T, object>> orderExpression, bool desc = false)
        {

            OrderPredicates.Add(new OrderModelField<T>() { IsDesc = desc, OrderPredicates = orderExpression });
            return this;

        }




        #endregion

        #region interface

        ISpecification<T> ISpecification<T>.Not(ISpecification<T> specification)
        {
            return Not(specification as Specification<T>);
        }

        ISpecification<T> ISpecification<T>.Or(ISpecification<T> specification)
        {
            return Not(specification as Specification<T>);
        }

        ISpecification<T> ISpecification<T>.And(Expression<Func<T, bool>> expression)
        {
            return Not(expression);
        }

        ISpecification<T> ISpecification<T>.Or(Expression<Func<T, bool>> expression)
        {
            return Not(expression);
        }

        ISpecification<T> ISpecification<T>.Not(Expression<Func<T, bool>> expression)
        {
            return Not(expression);
        }

        ISpecification<T> ISpecification<T>.And(ISpecification<T> specification)
        {
            return Not(specification as Specification<T>);
        }

        ISpecification<T> ISpecification<T>.Orderby(Expression<Func<T, object>> orderExpression, bool desc)
        {
            return Orderby(orderExpression, desc);
        }

        ISpecification<T> ISpecification<T>.OrderbyDesc(Expression<Func<T, object>> orderExpression)
        {
            return OrderbyDesc(orderExpression);
        }

        ISpecification<T> ISpecification<T>.OrderbyAsc(Expression<Func<T, object>> orderExpression)
        {
            return OrderbyAsc(orderExpression);
        }



        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.Helper
{
    public static class TypeHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Type RemoveNullabl(this Type obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            var type = obj;

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(type) : type;

        }
        readonly static List<Type> numbertypes = new List<Type>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsNumberType(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (type.IsValueType == false) throw new NotSupportedException("不支持非值类型");
            if (numbertypes.Contains(type)) return true;
            var obj = System.Activator.CreateInstance(type);
            var typecode = Convert.GetTypeCode(obj);
            switch (typecode)
            {

                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    numbertypes.Add(type);
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        /// 类型是否AllowMultiple的缓存
        /// </summary>
        private static readonly ConcurrentCache<Type, bool> typeAllowMultipleCache = new ConcurrentCache<Type, bool>();
        /// <summary>
        /// 类型的默认值缓存
        /// </summary>
        private static readonly ConcurrentCache<Type, object> typeDefaultValueCache = new ConcurrentCache<Type, object>();
        /// <summary>
        /// 关联的AttributeUsageAttribute是否AllowMultiple
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAllowMultiple(this Type type)
        {
            return typeAllowMultipleCache.GetOrAdd(type, (t => t.IsInheritFrom<Attribute>() && t.GetTypeInfo().GetCustomAttribute<AttributeUsageAttribute>().AllowMultiple));
        }
        /// <summary>
        ///  获取所有共有属性集合
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            return type.GetInterfaces().Concat(new[] { type }).SelectMany(itf => itf.GetProperties()).Distinct();
        }

        /// <summary>
        ///  获取所有公共方法集合
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetAllMethodInfos(this Type type)
        {
            return type.GetInterfaces().Concat(new[] { type }).SelectMany(itf => itf.GetMethods()).Distinct();
        }
        /// <summary>
        /// 返回type的详细类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetTypeInfo(this Type type)
        {
            return type;
        }
        /// <summary>
        /// 是否可以从TBase类型派生
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsInheritFrom<TBase>(this Type type)
        {
            return typeof(TBase).IsAssignableFrom(type);
        }

        /// <summary>
        /// 返回类型不含namespace的名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private static string GetName(this Type type)
        {
            if (type.GetTypeInfo().IsGenericType == false)
            {
                return type.Name;
            }

            var builder = new StringBuilder();
            foreach (var argType in type.GetGenericArguments())
            {
                if (builder.Length > 0)
                {
                    builder.Append(",");
                }
                builder.Append(argType.GetName());
            }
            builder.Insert(0, string.Format("{0}<", type.Name));
            builder.Append(">");
            return builder.ToString();
        }
        /// <summary>
        /// 返回方法的完整名称
        /// </summary>
        /// <param name="method">方法</param>
        /// <returns></returns>
        private static string GetFullName(this MethodInfo method)
        {
            var builder = new StringBuilder();
            foreach (var p in method.GetParameters())
            {
                if (builder.Length > 0)
                {
                    builder.Append(",");
                }
                builder.Append(p.ParameterType.GetName());
            }

            var insert = string.Format("{0} {1}(", method.ReturnType.GetName(), method.Name);
            builder.Insert(0, insert);
            builder.Append(")");
            return builder.ToString();
        }
    }
}

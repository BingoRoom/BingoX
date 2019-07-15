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
        /// 获取可空类型的基类型
        /// </summary>
        /// <param name="obj">可空类型</param>
        /// <returns>基类型</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Type RemoveNullabl(this Type obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            var type = obj;
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(type) : type;
        }
        /// <summary>
        /// 值类型缓存器
        /// </summary>
        readonly static List<Type> numbertypes = new List<Type>();
        /// <summary>
        /// 判断类型是否为数据类型
        /// </summary>
        /// <param name="type">类型</param>
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
        /// 返回类型不含namespace的名称,支持泛型
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
        /// 返回方法的定义字符串
        /// </summary>
        /// <param name="method">方法</param>
        /// <returns>格式：string GetStr(int,string)</returns>
        public static string GetFullName(this MethodInfo method)
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

        /// <summary>
        /// 从程序集反射出所有实现了指定类型的具体类型
        /// </summary>
        /// <typeparam name="TBaseType">指定类型</typeparam>
        /// <param name="assembly">程序集</param>
        /// <returns>类型集合</returns>
        public static IEnumerable<Type> GetConcreteDescendentTypes<TBaseType>(this Assembly assembly)
        {
            return assembly.GetConcreteDescendentTypes(typeof(TBaseType));
        }

        /// <summary>
        /// 从程序集反射出所有实现了指定类型的具体类型
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="baseType">指定类型</param>
        /// <returns>类型集合</returns>
        public static IEnumerable<Type> GetConcreteDescendentTypes(this Assembly assembly, Type baseType)
        {
            return assembly.GetTypes().Where(type => baseType.IsAssignableFrom(type) && IsConcreteType(type));
        }

        /// <summary>
        /// 是否是具体类型，凡是能直接实例化的类型都是具体类型。
        /// </summary>
        public static bool IsConcreteType(this Type type)
        {
            return !type.IsGenericTypeDefinition && !type.IsAbstract && !type.IsInterface;
        }

        /// <summary>
        /// 从程序集反射出所有实现指定类型的具体类型并实例化。
        /// </summary>
        /// <typeparam name="TBaseType">指定类型</typeparam>
        /// <param name="assembly">程序集</param>
        /// <returns>指定类型的对象集合</returns>
        public static IEnumerable<TBaseType> CreateConcreteDescendentInstances<TBaseType>(this Assembly assembly)
        {
            return assembly.GetConcreteDescendentTypes<TBaseType>().Where(type => !type.ContainsGenericParameters).Select(type => type.CreateInstance<TBaseType>());
        }
        /// <summary>
        /// 用于缓存类型的默认对象
        /// </summary>
        static readonly Dictionary<Type, object> DefaultValuesObjects = new Dictionary<Type, object>();
        /// <summary>
        /// 获取类型的默认对象
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static object GetDefaultValue(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            lock (LockObj)
            {
                var castType = RemoveNullable(type);
                if (!castType.IsValueType) return null;
                if (DefaultValuesObjects.ContainsKey(castType)) return DefaultValuesObjects[castType];
                var obj = Activator.CreateInstance(castType);
                DefaultValuesObjects.Add(castType, obj);
                return obj;
            }
        }
        /// <summary>
        /// 对象锁
        /// </summary>
        private static readonly object LockObj = new object();


        /// <summary>
        /// 类型过滤规则枚举
        /// </summary>
        [Flags]
        public enum ChildTypes
        {
            /// <summary>
            /// 抽象类
            /// </summary>
            Abstract = 1,
            /// <summary>
            /// 接口
            /// </summary>
            Interface = 2,
            /// <summary>
            /// 类
            /// </summary>
            Class = 4,
            /// <summary>
            /// 结构
            /// </summary>
            Struct = 8,
            /// <summary>
            /// 所有
            /// </summary>
            All = Abstract | Interface | Class | Struct,
        }

        /// <summary>
        /// 取当前类型的所有派生类
        /// </summary>
        /// <param name="type">当前类型</param>
        /// <returns></returns>
        public static Type[] GetChildTypes(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            List<Type> typeList = new List<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var types = GetChildTypes(assembly, type, ChildTypes.All);
                if (types.HasAny()) typeList.AddRange(types);
            }
            return typeList.ToArray();
        }

        /// <summary>
        /// 取当前类型的所有派生类
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="type">当前类型</param>
        /// <param name="child">类型过滤规则枚举</param>
        /// <returns></returns>
        public static Type[] GetChildTypes(this Assembly assembly, Type type, ChildTypes child)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (type == null) throw new ArgumentNullException("type");
            if (type == Types.Object) throw new TypeAccessException("can't Object's Type");
            List<Type> typeList = new List<Type>();
            var types = assembly.GetTypes();
            foreach (Type item in types)
            {
                if (!type.IsAssignableFrom(item)) continue;
                bool canAdd = ChildTypes.All.InFlag(child) ||
                              (ChildTypes.Abstract.InFlag(child) && item.IsAbstract) ||
                              (ChildTypes.Interface.InFlag(child) && item.IsInterface) ||
                              (ChildTypes.Class.InFlag(child) && item.IsClass) ||
                              (ChildTypes.Struct.InFlag(child) && item.IsValueType);

                if (canAdd) typeList.Add(item);
            }
            return typeList.ToArray();
        }

        /// <summary>
        /// 取Dll库版本
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Version GetVersion(this Assembly assembly)
        {
            return assembly.GetName().Version;
        }
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="types"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TryResult<object> InovkeType(this Type type, Type[] types, params object[] parameters)
        {
            if (type == null) return new ArgumentNullException("type");
            object setvalue = null;
            try
            {
                var tryparse = type.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static);
                if (tryparse != null && parameters.Length == 2)
                {
                    var obj = tryparse.FastInvoke(null, parameters);
                    if (obj is bool) setvalue = parameters[1];
                }
                var parse = type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static);
                if (parse != null) setvalue = parse.FastInvoke(parameters);
                if (setvalue != null) return setvalue;
                if (types == null) return new ArgumentNullException("type");
                var countst = type.GetConstructor(types);
                if (countst == null) return new Exception("不能通过构造,Parse,TryParse等函数转换");
                setvalue = countst.FastInvoke(parameters);
                return setvalue;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// 判断类型是否可空类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            return type != null && (type.IsGenericType && type.GetGenericTypeDefinition() == Types.Nullable);
        }
        /// <summary>
        /// 移除Nullable类型
        /// </summary>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Type RemoveNullable(this Type conversionType)
        {
            return (conversionType != null && IsNullable(conversionType)) ? Nullable.GetUnderlyingType(conversionType) : conversionType;
        }
    }
}

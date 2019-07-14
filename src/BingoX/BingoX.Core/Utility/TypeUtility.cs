using BingoX.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.Utility
{
    public static class TypeUtility
    {
        /// <summary>
        /// 返回<paramref name="assembly"/>中的所有实现或继承了<typeparamref name="TBaseType"/>的具体类型。
        /// </summary>
        public static IEnumerable<Type> GetConcreteDescendentTypes<TBaseType>(this Assembly assembly)
        {
            return assembly.GetConcreteDescendentTypes(typeof(TBaseType));
        }

        /// <summary>
        /// 返回<paramref name="assembly"/>中的所有实现或继承了<paramref name="baseType"/>的具体类型。
        /// </summary>
        public static IEnumerable<Type> GetConcreteDescendentTypes(this Assembly assembly, Type baseType)
        {
            return assembly.GetTypes().Where(type => baseType.IsAssignableFrom(type) && IsConcreteType(type));
        }
        /// <summary>
        /// 返回 
        /// </summary>
        public static Type GetConcreteTypes(string stringtype)
        {
            return AppDomain.CurrentDomain.GetAssemblies().Select(assembly => assembly.GetType(stringtype)).FirstOrDefault(type => type != null);
        }

        public static Type FindType(string classtype)
        {
            if (classtype == null) return null;
            var classarry = classtype.Split(',');
            var ass = Assembly.GetEntryAssembly();
            Type findclasstype = null;
            switch (classarry.Length)
            {
                case 1:
                    findclasstype = ass.GetType(classarry[0]);
                    break;
                case 2:
                    var assname = classarry[1];
                    if (string.Equals(System.IO.Path.GetFileNameWithoutExtension(ass.ManifestModule.ScopeName), assname,
                        StringComparison.CurrentCultureIgnoreCase))
                    {
                        findclasstype = ass.GetType(classarry[0]);
                        break;
                    }
                    ass = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(n => string.Equals(System.IO.Path.GetFileNameWithoutExtension(n.ManifestModule.ScopeName), assname, StringComparison.CurrentCultureIgnoreCase));
                    if (ass != null) findclasstype = ass.GetType(classarry[0]);
                    break;
            }
            return findclasstype;
        }

        /// <summary>
        /// 是否是具体类型，凡是能直接实例化的类型都是具体类型。
        /// </summary>
        public static bool IsConcreteType(this Type type)
        {
            return !type.IsGenericTypeDefinition && !type.IsAbstract && !type.IsInterface;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TBaseType"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<TBaseType> CreateConcreteDescendentInstances<TBaseType>(this Assembly assembly)
        {
            return assembly.GetConcreteDescendentTypes<TBaseType>().Where(type => !type.ContainsGenericParameters).Select(type => type.CreateInstance<TBaseType>());
        }

        /// <summary>
        /// 是否数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumeric(this Type type)
        {
            if (type == null) return false;

            switch (type.FullName)
            {
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.Decimal":
                case "System.Double":
                case "System.Single":
                case "System.Byte":
                case "System.SByte":
                case "System.Char":
                    return true;

            }

            return false;
        }

        static readonly Dictionary<Type, object> DefaultValuesObjects = new Dictionary<Type, object>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
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

        private static readonly object LockObj = new object();


        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="type"></param>
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
        /// 取当前类型的所有子类
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="type"></param>
        /// <param name="child"></param>
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
        public static TryResult<object> InovkeType(this Type type, Type[] types, object value)
        {
            if (type == null) return new ArgumentNullException("type");
            if (value == DBNull.Value) return new ArgumentNullException("type");
            object setvalue = null;
            try
            {
                var tryparse = type.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static);
                if (tryparse != null)
                {
                    var parameters = new[] { value, null };
                    var obj = tryparse.Invoke(null, BindingFlags.InvokeMethod, null, parameters, null);
                    if (obj is bool)
                    {
                        setvalue = parameters[1];
                    }
                }
                var parse = type.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static);
                if (parse != null)
                {
                    var parameters = new[] { value };
                    setvalue = parse.Invoke(null, BindingFlags.InvokeMethod, null, parameters, null);
                }
                if (setvalue != null) return setvalue;
                if (types == null) return new ArgumentNullException("type");
                var countst = type.GetConstructor(types);
                if (countst == null) return new Exception("不能通过构造,Parse,TryParse等函数转换");
                setvalue = countst.Invoke(new[] { value });
                return setvalue;
            }
            catch (Exception ex)
            {

                return ex;
            }

        }

        /// <summary>
        /// 
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

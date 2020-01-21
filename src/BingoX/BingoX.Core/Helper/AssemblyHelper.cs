using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BingoX.Helper
{

    /// <summary>
    /// 提供一个针对程序集的辅助
    /// </summary>
    public static class AssemblyHelper
    {
        public readonly static Func<Type, bool> IsClassPredicate = (source) => source.IsClass && !source.IsAbstract;
        public readonly static Func<Type, bool> IsInterfacePredicate = (source) => source.IsInterface;
        public readonly static Func<Type, Type, bool> IsFromPredicate = (source, compare) => compare.IsAssignableFrom(source) ;
        public readonly static Func<Type, Type, bool> IsFromAndNotSelfPredicate = (source, compare) => compare.IsAssignableFrom(source) && source != compare;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ReadTextFile(this Assembly assembly, string name)
        {
            return ReadTextFile(assembly, name, Encoding.UTF8);
        }
        /// <summary>
        /// 读取程序集中的嵌入资源
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="name"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadTextFile(this Assembly assembly, string name, Encoding encoding)
        {
            var fs = assembly.GetManifestResourceStream(name);
            if (fs == null) throw new Exception("文件不存在");
            using (fs)
            {
                using (var reader = new System.IO.StreamReader(fs, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        /// <summary>
        /// 从当前程序集反射指定类型的所有派生类型
        /// </summary>
        /// <param name="assembly">要反射的程序集</param>
        /// <param name="interfaceType">要反射的指定类型（支持泛型）</param>
        /// <param name="typeArguments">泛型的替代类型数组</param>
        /// <returns>类型数组</returns>
        public static Type[] GetImplementedClass(this Assembly assembly, Type interfaceType, params Type[] typeArguments)
        {
            var types = assembly.GetTypes().Where(o => o.IsClass && !o.IsAbstract);
            if (interfaceType.IsGenericType)
            {
                var maketype = interfaceType.MakeGenericType(typeArguments);
                return types.Where(o => maketype.IsAssignableFrom(o)).ToArray();
            }
            else
            {
                return types.Where(o => interfaceType.IsAssignableFrom(o)).ToArray();
            }
        }

        /// <summary>
        /// 从当前程序集反射指定类型的所有派生类型
        /// </summary>
        /// <param name="assembly">要反射的程序集</param>
        /// <param name="scouceType">要反射的指定类型（支持泛型）</param>
        /// <param name="typeArguments">泛型的替代类型数组</param>
        /// <returns>类型数组</returns>
        public static Type[] GetGeneric(this Assembly assembly, Type scouceType, Type[] typeGenericArguments, FindType type)
        {
            if (!scouceType.IsGenericType) throw new LogicException("不为泛型接口");
            if (typeGenericArguments.IsEmpty()) throw new LogicException("泛型参数为空");
            if (type == FindType.Interface && scouceType.IsClass) throw new LogicException("查找派生接口时，基础类型不能为类");
            IEnumerable<Type> types = null;
            switch (type)
            {
                case FindType.Interface:
                    types = assembly.GetTypes().Where(IsInterfacePredicate);
                    break;
                case FindType.Class:
                    types = assembly.GetTypes().Where(IsClassPredicate);
                    break;
                default:
                    break;
            }

            var arguments = scouceType.GetGenericArguments();
            var maketype = scouceType.MakeGenericType(typeGenericArguments);
            return types.Where(o => IsFromPredicate(maketype, o)).ToArray();
        }
        public enum FindType
        {
            Interface,
            Class
        }
  
        /// <summary>
        /// 从当前程序集反射指定类型的所有派生类型
        /// </summary>
        /// <typeparam name="TInterfaceType"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Type[] GetImplementedClass<TInterfaceType>(this Assembly assembly)
        {
            return GetImplementedClass(assembly, typeof(TInterfaceType));

        }
        /// <summary>
        /// 从当前程序集反射指定类型的所有派生类型
        /// </summary>
        /// <typeparam name="TInterfaceType"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Type[] GetImplementedClass(this Assembly assembly, Type[] types)
        {
            var implementedClass = assembly.GetTypes().Where(o => IsClassPredicate(o) && types.Any(x => IsFromPredicate(o,x))).ToArray();
            return implementedClass;
        }
    }
}

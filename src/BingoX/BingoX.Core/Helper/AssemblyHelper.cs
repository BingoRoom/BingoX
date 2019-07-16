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
        /// <param name="assembly"></param>
        /// <param name="interfaceType"></param>
        /// <param name="genericType"></param>
        /// <returns>类型数组</returns>
        public static Type[] GetImplementedClass(this Assembly assembly, Type interfaceType, params Type[] genericType)
        {
            var types = assembly.GetTypes().Where(o => o.IsClass && !o.IsAbstract);
            if (interfaceType.IsGenericType)
            {
                var maketype = interfaceType.MakeGenericType(genericType);
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
        /// <typeparam name="TInterfaceType"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Type[] GetImplementedClass<TInterfaceType>(this Assembly assembly)
        {
            return GetImplementedClass(assembly, typeof(TInterfaceType));

        }

    }
}

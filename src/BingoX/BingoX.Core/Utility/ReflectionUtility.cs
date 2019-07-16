using BingoX.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.Utility
{
    public class ReflectionUtility
    {
        /// <summary>
        /// 类型缓存
        /// </summary>
        static readonly IDictionary<string, Type> TypeCache = new Dictionary<string, Type>(DelegateComparer.ComparerString());
        /// <summary>
        /// 通过类型名称从程序集反射类型
        /// </summary>
        /// <param name="typename">类型名称</param>
        /// <returns></returns>
        public static Type GetType(string typename)
        {
            if (string.IsNullOrEmpty(typename)) return null;
            if (TypeCache.ContainsKey(typename)) return TypeCache[typename];
            Type type = Type.GetType(typename);
            if (type != null)
            {
                TypeCache.Add(typename, type);
                return type;
            }
            var allAssembly = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in allAssembly)
            {
                type = assembly.GetType(typename);
                if (type == null) continue;
                TypeCache.Add(typename, type);
                break;
            }
            return type;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T">要创建的对象类型</typeparam>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="className">类型名称</param>
        /// <typeparam name="T">指定类型</typeparam>
        /// <returns>返回对象实例</returns>
        public static T CreateInstance<T>(string assemblyName, string className) where T : class
        {
            object obj = CreateInstance(assemblyName, className);
            if (obj is T) return (T)obj;
            return default(T);
        }

        /// <summary>
        /// 通过程序集、类名创建对象
        /// </summary>
        /// <param name="assemblyName">程序集</param>
        /// <param name="className">类名</param>
        /// <returns>返回对象</returns>
        public static object CreateInstance(string assemblyName, string className)
        {
            var assembly = CompareUtility.FirstOrDefault(assemblyName, AppDomain.CurrentDomain.GetAssemblies(), n => n.ManifestModule.Name);
            if (assembly == null) throw new ArgumentNullException("assemblyName");
            var objType = assembly.GetType(className);
            if (objType == null) throw new ArgumentNullException("className");
            var obj = objType.CreateInstance();
            return obj;
        }

        /// <summary>
        /// 通过指定程序集、类名、方法名、参数列表执行方法
        /// </summary>                     　
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="className">类型名称</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="parameters">方法列表</param>
        /// <returns>返回对象实例</returns>
        public static TryResult<T> InvokeMethod<T>(string assemblyName, string className, string methodName, params object[] parameters)
        {
            var obj = InvokeMethod(assemblyName, className, methodName, parameters);
            if (obj.HasError) return obj.Error;
            return ObjectUtility.Cast<T>(obj);
        }

        /// <summary>
        /// 通过对象实例、方法名、参数列表执行方法
        /// </summary>
        /// <param name="obj">对象实例</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回对象</returns>
        public static TryResult<object> InvokeMethod(object obj, string methodName, params object[] parameters)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (methodName == null) throw new ArgumentNullException("methodName");
            var method = obj.GetType().GetMethod(methodName, ConstValue.DefaulBindingFlags);
            if (method == null)
            {
                return new Exception("NotFindMethod");
            }
            try
            {
                return method.FastInvoke(obj, parameters);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// 通过指定程序集、类名、方法名、参数列表执行方法
        /// </summary>                    
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="className">类型名称</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="parameters">方法列表</param>
        /// <returns>返回对象</returns>
        public static TryResult<object> InvokeMethod(string assemblyName, string className, string methodName, params object[] parameters)
        {
            try
            {
                object obj = CreateInstance(assemblyName, className);
                return InvokeMethod(obj.GetType(), methodName, parameters);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="path">程序集文件路径</param>
        /// <returns>加载后的程序集</returns>
        public static TryResult<Assembly> AddAssembly(string path)
        {
            string assemblyName = Path.GetFileName(path);
            var assembly = CompareUtility.FirstOrDefault(assemblyName, AppDomain.CurrentDomain.GetAssemblies(), n => n.ManifestModule.Name);
            if (assembly != null) return assembly;
            if (!File.Exists(path)) return new IOException("文件不存在");
            try
            {
                var fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                byte[] buffter = new byte[fs.Length];
                fs.Read(buffter, 0, buffter.Length);
                assembly = Assembly.Load(buffter);
            }
            catch (Exception ex)
            {
                return ex;
            }
            return assembly;
        }
    }
}

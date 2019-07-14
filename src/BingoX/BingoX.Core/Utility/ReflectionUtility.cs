using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.Utility
{
    public static class ReflectionUtility
    {
        static readonly IDictionary<string, Type> TypeCache = new Dictionary<string, Type>(DelegateComparer.ComparerString());
        /// <summary>
        /// 从程序集中取类型
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        public static Type GetType(string typename)
        {

            if (string.IsNullOrEmpty(typename))
                return null;
            if (TypeCache.ContainsKey(typename))
                return TypeCache[typename];
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
                if (type == null)
                    continue;
                TypeCache.Add(typename, type);
                break;
            }
            return type;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(string assemblyName, string className)
            where T : class
        {
            object obj = CreateInstance(assemblyName, className);
            if (obj is T)
                return (T)obj;
            return default(T);
        }

        /// <summary>
        /// 执行方法
        /// </summary>                     　
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static TryResult<T> InvokeMethod<T>(string assemblyName, string className, string methodName, params object[] parameters)
        {
            var obj = InvokeMethod(assemblyName, className, methodName, parameters);
            if (obj.HasError) return obj.Error;
            return ObjectUtility.Cast<T>(obj);
        }



        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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

                return method.Invoke(obj, parameters);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        /// <summary>
        /// 执行方法
        /// </summary>                    
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
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
        /// 创建对象
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
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
        ///  创建对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CreateInstance(this Type type)
        {
            if (type == null)
                return null;
            var iConstructor = type.GetConstructor(new Type[] { });
            if (iConstructor == null)
                return null;
            var obj = iConstructor.Invoke(new object[] { });
            return obj;
        }

        /// <summary>
        ///  创建对象
        /// </summary>
        /// <param name="type"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateInstance<T>(this Type type)
        {
            return typeof(T).IsAssignableFrom(type) ? (T)CreateInstance(type) : default(T);
        }

        /// <summary>
        /// 添加程序集
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static TryResult<Assembly> AddAssembly(string path)
        {
            string assemblyName = Path.GetFileName(path);

            var assembly = CompareUtility.FirstOrDefault(assemblyName, AppDomain.CurrentDomain.GetAssemblies(), n => n.ManifestModule.Name);
            if (assembly != null) return assembly;
            if (!File.Exists(path))
                return new IOException("文件不存在");
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

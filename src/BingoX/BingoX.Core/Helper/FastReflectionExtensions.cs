using BingoX.ComponentModel.FastReflection;
using BingoX.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.Helper
{
    /// <summary>
    /// 提供针对快速反射的辅助
    /// </summary>
    public static class FastReflectionExtensions
    {
        /// <summary>
        /// 提供对象实例、属性值，设置PropertyInfo指定的属性值
        /// </summary>
        /// <param name="propertyInfo">属性</param>
        /// <param name="instance">对象实例</param>
        /// <param name="value">属性值</param>
        public static void FastSetValue(this PropertyInfo propertyInfo, object instance, object value)
        {
            FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo).SetValue(instance, value);
        }

        /// <summary>
        /// 提供对象实例，获取PropertyInfo指定的属性值
        /// </summary>
        /// <param name="propertyInfo">属性</param>
        /// <param name="instance">对象实例</param>
        /// <returns>返回属性值</returns>
        public static object FastGetValue(this PropertyInfo propertyInfo, object instance)
        {
            return FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo).GetValue(instance);
        }


        /// <summary>
        /// 提供对象实例，获取PropertyInfo指定的属性值，并转型T的实例
        /// </summary>
        /// <param name="propertyInfo">属性</param>
        /// <param name="instance">对象实例</param>
        /// <returns>返回T的实例</returns>
        public static T FastGetValue<T>(this PropertyInfo propertyInfo, object instance)
        {
            var obj = propertyInfo.FastGetValue(instance);
            return ObjectUtility.Cast<T>(obj);
        }
        /// <summary>
        /// 提供对象实例，获取PropertyInfo指定的属性值
        /// </summary>
        /// <param name="fieldInfo">字段</param>
        /// <param name="instance">对象实例</param>
        /// <returns></returns>
        public static object FastGetValue(this FieldInfo fieldInfo, object instance)
        {
            return FastReflectionCaches.FieldAccessorCache.Get(fieldInfo).GetValue(instance);
        }


        /// <summary>
        /// 提供对象实例，获取FieldInfo指定的属性值，并转型T的实例
        /// </summary>
        /// <param name="fieldInfo">字段</param>
        /// <param name="instance">对象实例</param>
        /// <returns></returns>
        public static T FastGetValue<T>(this FieldInfo fieldInfo, object instance)
        {
            var obj = fieldInfo.FastGetValue(instance);
            return ObjectUtility.Cast<T>(obj);
        }

        /// <summary>
        /// 提供对象实例、参数列表，调度MethodInfo指定的方法
        /// </summary>
        /// <param name="methodInfo">方法</param>
        /// <param name="instance">对象实例</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回调度结果</returns>
        public static object FastInvoke(this MethodInfo methodInfo, object instance, params object[] parameters)
        {
            return FastReflectionCaches.MethodInvokerCache.Get(methodInfo).Invoke(instance, parameters);
        }

        /// <summary>
        /// 提供参数列表，调度ConstructorInfo并返回对象
        /// </summary>
        /// <param name="constructorInfo">构造函数</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回对象</returns>
        public static object FastInvoke(this ConstructorInfo constructorInfo, params object[] parameters)
        {
            return FastReflectionCaches.ConstructorInvokerCache.Get(constructorInfo).Invoke(parameters);
        }
        /// <summary>
        /// 提供参数列表，调度ConstructorInfo并返回对象，把执行结果转型为T
        /// </summary>
        /// <param name="constructorInfo">构造函数</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>执行结果</returns>
        public static T FastInvoke<T>(this ConstructorInfo constructorInfo, params object[] parameters)
        {
            return typeof(T).IsAssignableFrom(constructorInfo.DeclaringType)
                       ? (T)FastInvoke(constructorInfo, parameters)
                       : default(T);
        }

        /// <summary>
        ///  创建当前类型的对象实例，并转型为T。
        /// </summary>
        /// <param name="type">当前类型</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>对象实例</returns>
        public static T CreateInstance<T>(this Type type)
        {
            return typeof(T).IsAssignableFrom(type) ? (T)CreateInstance(type) : default(T);
        }

        /// <summary>
        ///  创建当前类型的对象实例
        /// </summary>
        /// <param name="type">当前类型</param>
        /// <returns>对象实例</returns>
        public static object CreateInstance(this Type type)
        {
            if (type == null) return null;
            var iConstructor = type.GetConstructor(new Type[] { });
            if (iConstructor == null) return null;
            var obj = iConstructor.FastInvoke();
            return obj;
        }
        /// <summary>
        ///  创建当前类型的对象实例，并转型为T。
        /// </summary> 
        /// <typeparam name="T"></typeparam>
        /// <returns>对象实例</returns>
        public static T CreateInstance<T>()
        {
            return CreateInstance<T>(typeof(T));
        }

        /// <summary>
        /// 通过指定的构造函数创建当前类型的实例，并转型为T。
        /// </summary>
        /// <param name="type">当前类型</param>
        /// <param name="parmTypes">参数类型列表</param>
        /// <param name="parms">参数列表</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>对象实例</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T CreateInstance<T>(this Type type, Type[] parmTypes, object[] parms)
        {
            if (type == null || !typeof(T).IsAssignableFrom(type)) return default(T);
            var iConstructor = type.GetConstructor(parmTypes);
            var obj = iConstructor.FastInvoke(parms);
            return (T)obj;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.Utility
{
    /// <summary>
    /// 提供从当前程序集以及依赖程序集反射特定接口类型的功能
    /// </summary>
    public class FindInterfaceMappToType
    {
        private readonly Type _interfacType;
        private readonly Assembly _intefaceAssembly;
        private readonly Assembly _maptoAssembly;
        /// <summary>
        /// 构造反射器
        /// </summary>
        /// <param name="interfacType">接口类型</param>
        /// <param name="intefaceAssembly">当前程序集</param>
        /// <param name="maptoAssembly">依赖程序集</param>
        public FindInterfaceMappToType(Type interfacType, Assembly intefaceAssembly, Assembly maptoAssembly)
        {
            _interfacType = interfacType;
            _intefaceAssembly = intefaceAssembly;
            _maptoAssembly = maptoAssembly;
        }
        /// <summary>
        /// 执行反射
        /// </summary>
        /// <returns></returns>
        public IDictionary<Type, Type[]> Find()
        {
            IDictionary<Type, Type[]> dictionary = new Dictionary<Type, Type[]>();
            var types =
              _intefaceAssembly.GetTypes()
                  .Where(n => _interfacType.IsAssignableFrom(n) && n.IsInterface && n != _interfacType).OrderBy(n => n.Name)
                  .ToArray();
            var mapptos =
                _maptoAssembly.GetTypes()
                    .Where(n => _interfacType.IsAssignableFrom(n) && n.IsClass && !n.IsAbstract && n != _interfacType)
                    .ToArray();
            foreach (var type in types)
            {
                if (type.IsGenericType) continue;
                var maptoType = mapptos.Where(n => type.IsAssignableFrom(n) && n.IsClass && !n.IsAbstract).ToArray();
                dictionary.Add(type, maptoType.Length == 0 ? null : maptoType);
            }
            return dictionary;
        }
        /// <summary>
        /// 从当前程序集以及依赖程序集反射特定接口类型
        /// </summary>
        /// <param name="interfacType">接口类型</param>
        /// <param name="intefaceAssembly">当前程序集</param>
        /// <param name="maptoAssembly">依赖程序集</param>
        /// <returns></returns>
        public static IDictionary<Type, Type[]> Find(Type interfacType, Assembly intefaceAssembly, Assembly maptoAssembly)
        {
            return new FindInterfaceMappToType(interfacType, intefaceAssembly, maptoAssembly).Find();
        }
    }
}

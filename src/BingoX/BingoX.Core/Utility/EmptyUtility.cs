using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
 
 

namespace BingoX.Utility
{
    /// <summary>
    /// 提供一个空集合的工具
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EmptyUtility<T>
    {
        /// <summary>
        /// 返回一个指定类型的空数组
        /// </summary>
        public static readonly T[] EmptyArray = new T[0];
        /// <summary>
        /// 返回一个指定类型的只读空集合
        /// </summary>
        public static readonly IList<T> EmptyList = new ReadOnlyCollection<T>(EmptyArray);
    }
}

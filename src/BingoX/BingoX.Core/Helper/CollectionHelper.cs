﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace BingoX.Helper
{
    /// <summary>
    /// 表示一个针对集合的辅助
    /// </summary>
    public static class CollectionHelper
    {

        static readonly Dictionary<Type, object> DictionaryEmpty = new Dictionary<Type, object>();
        static readonly Dictionary<Type, object> ArrayEmpty = new Dictionary<Type, object>();
        /// <summary>
        /// 返回一个指定类型的空数组
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <returns>空数组</returns>
        public static T[] EmptyOfArray<T>()
        {
            if (ArrayEmpty.ContainsKey(typeof(T))) return ArrayEmpty[typeof(T)] as T[];

            if (ArrayEmpty.ContainsKey(typeof(T))) return ArrayEmpty[typeof(T)] as T[];
            var arr = new T[] { };
            ArrayEmpty.Add(typeof(T), arr);
            return arr;

        }
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> comparer) where T : class
        {
            return source.Distinct(new DynamicEqualityComparer<T>(comparer));
        }

        private sealed class DynamicEqualityComparer<T> : IEqualityComparer<T>
            where T : class
        {
            private readonly Func<T, T, bool> _func;

            public DynamicEqualityComparer(Func<T, T, bool> func)
            {
                _func = func;
            }

            public bool Equals(T x, T y) => _func(x, y);

            public int GetHashCode(T obj) => 0;
        }
        /// <summary>
        /// 返回一个指定类型的空IList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>空IList</returns>
        public static IList<T> EmptyOfList<T>()
        {
            if (DictionaryEmpty.ContainsKey(typeof(T))) return DictionaryEmpty[typeof(T)] as IList<T>;
            var lis = new ReadOnlyCollectionBuilder<T>();
            DictionaryEmpty.Add(typeof(T), lis);
            return lis;
        }
        /// <summary>
        /// 返回当前枚举的List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static List<T> AsList<T>(this IEnumerable<T> collection)
        {
            if (collection == null) return new List<T>();
            return new List<T>(collection);
        }

        /// <summary>
        /// 通过枚举返回支持数据绑定的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static BindingList<T> AsBindingList<T>(this IEnumerable<T> collection)
        {
            if (collection == null) return new BindingList<T>();
            return new BindingList<T>(collection.OfType<T>().ToList());
        }

        /// <summary>
        /// 通过枚举返回符合转型条件的集合
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static List<TTarget> AsList<TSource, TTarget>(this IEnumerable<TSource> collection)
        {
            if (collection == null) return new List<TTarget>();
            return new List<TTarget>(collection.OfType<TTarget>());
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> source, params Expression<Func<T, object>>[] expressions)
        {
            PropertyInfo[] members = PrepareMemberInfos(expressions).OfType<PropertyInfo>().ToArray();

            Func<T, object[]> func = CreateShredder<T>(members);
            DataTable table = new DataTable() { TableName = typeof(T).Name };
            foreach (var item in members)
            {
                table.Columns.Add(item.Name, item.PropertyType);
            }
            table.BeginLoadData();
            try
            {
                foreach (T item in source)
                {
                    DataRow dataRow = table.NewRow();
                    dataRow.ItemArray = func(item);
                    table.Rows.Add(dataRow);
                }
                return table;
            }
            finally
            {
                table.EndLoadData();
            }
        }

        private static Func<T, object[]> CreateShredder<T>(PropertyInfo[] members)
        {
            return new Func<T, object[]>(n => members.Select(x => FastReflectionExtensions.FastGetValue(x, n)).ToArray());
        }

        private static IEnumerable<MemberInfo> PrepareMemberInfos<T>(ICollection<Expression<Func<T, object>>> expressions)
        {
            if (expressions == null || expressions.Count == 0)
            {
                return typeof(T).GetMembers(BindingFlags.Instance | BindingFlags.Public).Where(delegate (MemberInfo m)
                {
                    if (m.MemberType != MemberTypes.Field)
                    {
                        PropertyInfo propertyInfo = m as PropertyInfo;
                        if ((object)propertyInfo != null && propertyInfo.CanRead)
                        {
                            return propertyInfo.GetIndexParameters().Length == 0;
                        }
                        return false;
                    }
                    return true;
                });
            }
            try
            {
                return expressions.Select(GetAccessedMember);
            }
            catch (ArgumentException innerException)
            {
                throw new ArgumentException("One of the supplied expressions is not allowed.", "expressions", innerException);
            }
            MemberInfo GetAccessedMember(LambdaExpression lambda)
            {
                Expression expression = lambda.Body;
                if (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)
                {
                    expression = ((UnaryExpression)expression).Operand;
                }
                MemberExpression memberExpression = expression as MemberExpression;
                if (memberExpression == null || memberExpression.Expression.NodeType != ExpressionType.Parameter)
                {
                    throw new ArgumentException($"Illegal expression: {lambda}", "lambda");
                }
                return memberExpression.Member;
            }
        }




        /// <summary>
        /// 从数组移除项目
        /// </summary>
        /// <param name="source">数组</param>
        /// <param name="items">要移除的项目</param>
        /// <typeparam name="T">项目类型</typeparam>
        /// <returns>返回移除之后的新数组</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T[] ReomveItme<T>(this T[] source, params T[] items)
        {
            if (!source.HasAny() || !items.HasAny()) return source;
            IList slist = source;
            var indexlist = items.Select(item => slist.IndexOf(item)).Where(n => n != -1).ToArray();
            if (!indexlist.HasAny()) return source;
            var arr = new T[source.Length - indexlist.Length];
            var index = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (indexlist.Contains(i)) continue;
                arr[index] = source[i];
                index++;
            }
            return arr;
        }

        /// <summary>
        /// 对集合实现删除操作
        /// </summary>
        /// <param name="source">要删除的元素列表</param>
        /// <param name="filter">过滤器</param>
        /// <param name="method">删除时执行的委托</param>
        public static void RemoveElements(this IList source, Func<object, bool> filter, Action<object> method = null)
        {
            if (filter == null) return;
            var indexs = (from object d1 in source where filter(d1) select source.IndexOf(d1)).ToList();
            indexs.Sort();
            for (var i = indexs.Count - 1; i >= 0; i--)
            {
                var index = indexs[i];
                if (index == -1) continue;
                if (method != null)
                {
                    method(source[index]);
                }
                source.RemoveAt(index);
            }
        }
        /// <summary>
        /// 为数组追加元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T[] AppendItem<T>(this T[] source, T item)
        {
            if (source == null && item == null) return new T[0];
            if (source == null) return new[] { item };
            if (item == null) return source;
            var arr = source;
            Array.Resize(ref arr, source.Length + 1);
            arr[source.Length] = item;
            return arr;
        }

        /// <summary>
        /// 为数组追加元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T[] AppendItems<T>(this T[] source, params T[] items)
        {
            if (source == null && items == null) return new T[0];
            if (source == null) return items;
            if (items == null) return source;
            int index = source.Length;
            var arr = source;
            Array.Resize(ref arr, source.Length + items.Length);
            var itemindex = 0;
            for (int s = index; s < arr.Length; s++)
            {
                arr[s] = items[itemindex];
                itemindex++;
            }
            return arr;
        }




        /// <summary>
        ///  是否有记录数
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool HasAny(this IEnumerable enumerable)
        {
            return enumerable != null && enumerable.GetEnumerator().MoveNext();
        }

        /// <summary>
        /// 取集合总长度
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static int GetCount(this IEnumerable enumerable)
        {
            if (enumerable == null) return 0;

            if (enumerable is ICollection == false)
            {
                var enumerator = enumerable.GetEnumerator();
                var index = 0;
                while (enumerator.MoveNext())
                {
                    index++;
                }
                return index;

            }
            return ((ICollection)enumerable).Count;
        }

        /// <summary>
        ///  是否含有指定记录的数,大于零并小于等于实际记录数
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="length"> </param>
        /// <returns></returns>
        public static bool HasCount(this IEnumerable enumerable, int length)
        {
            if (!enumerable.HasAny()) return false;
            return length.IsBetween(1, GetCount(enumerable));
        }

        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="charstring"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ToAString<T, TV>(this IEnumerable<T> source, Func<T, TV> keySelector, string charstring)
        {
            return source.ToAString(keySelector, charstring, string.Empty);
        }

        /// <summary>
        /// 输出字符串
        /// </summary>
        /// <param name="source">集合</param>
        /// <param name="keySelector">输出文本模板格式化参数值Func</param>
        /// <param name="charstring">连接符</param>
        /// <param name="format">输出文本模板</param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <returns>用链接符连接的字符串</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ToAString<T, TV>(this IEnumerable<T> source, Func<T, TV> keySelector, string charstring, string format)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");
            if (string.IsNullOrWhiteSpace(charstring)) charstring = ",";
            if (string.IsNullOrWhiteSpace(format)) format = "{0}";
            StringBuilder stringbuilder = new StringBuilder();
            foreach (T variable in source)
            {
                stringbuilder.AppendFormat("{0}{1}", string.Format(format, keySelector(variable)), charstring);
            }
            if (stringbuilder.Length > 0) stringbuilder.Remove(stringbuilder.Length - charstring.Length, charstring.Length);
            return stringbuilder.ToString();
        }

        /// <summary>
        ///  对集合执行循环操作
        /// </summary>
        /// <param name="source">集合</param>
        /// <param name="action">操作</param>
        /// <typeparam name="T"></typeparam>     
        /// <returns></returns>
        public static void Foreach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");
            foreach (var variable in source)
            {
                action(variable);
            }
        }
        /// <summary>
        ///  对集合执行循环操作
        /// </summary>
        /// <param name="source">集合</param>
        /// <param name="action">操作</param>
        /// <typeparam name="T"></typeparam>     
        /// <returns></returns>
        public static void Foreach<T>(this IEnumerable<T> source, Action<int, T> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");
            int index = 0;
            foreach (var variable in source)
            {
                action(index, variable);
                index++;
            }
        }

        /// <summary>
        ///  过滤
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, TV>(this IEnumerable<T> source, Func<T, TV> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, TV>(keySelector));
        }

        /// <summary>
        ///  过滤
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="comparer"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, TV>(this IEnumerable<T> source, Func<T, TV> keySelector, IEqualityComparer<TV> comparer)
        {
            return source.Distinct(new CommonEqualityComparer<T, TV>(keySelector, comparer));
        }

        /// <summary>
        /// 从集合获取指定范围的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetRange<T>(this IList<T> collection, int index, int count)
        {

            if (index < 0 || count < 0 || collection.Count < index) yield return default(T);
            int tragetlenght = collection.Count - index;
            if (tragetlenght > count) tragetlenght = count;
            for (int i = 0; i < tragetlenght; i++)
            {
                var tmp = index;
                index++;
                yield return collection[tmp];
            }
        }

        /// <summary>
        /// 移动集合内指定项目的位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">集合</param>
        /// <param name="t">项目实例</param>
        /// <param name="newIndex">新索引</param>
        /// <returns></returns>
        public static IList<T> Move<T>(this IList<T> collection, T t, int newIndex)
        {
            var oldIndex = collection.IndexOf(t);
            if (oldIndex == newIndex)
                return collection;
            if (oldIndex.IsBetween(-1, collection.Count) && newIndex.IsBetween(-1, collection.Count))
            {
                collection.Remove(t);
                if (oldIndex > newIndex)
                    collection.Insert(newIndex, t);
                else
                    collection.Insert(newIndex - 1, t);
            }
            return collection;
        }

        /// <summary>
        /// 把集合中的指定项目从旧索引位置移动到新索引位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">集合</param>
        /// <param name="oldIndex">旧索引</param>
        /// <param name="newIndex">新索引</param>
        /// <returns></returns>
        public static IList<T> Move<T>(this IList<T> collection, int oldIndex, int newIndex)
        {
            if (oldIndex == newIndex)
                return collection;
            if (oldIndex.IsBetween(-1, collection.Count) && newIndex.IsBetween(-1, collection.Count))
            {
                var t = collection[oldIndex];
                collection.Remove(t);
                if (oldIndex > newIndex)
                    collection.Insert(newIndex, t);
                else
                    collection.Insert(newIndex - 1, t);

            }
            return collection;
        }
        /// <summary>
        /// 从集合移除指定范围的项目
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">集合</param>
        /// <param name="index">移除项目的起始索引</param>
        /// <param name="count">移除数量</param>
        /// <returns></returns>
        public static void ReomveRange<T>(this IList<T> collection, int index, int count)
        {

            if (index < 0) return;
            int reomvelenght = collection.Count - index - count;
            if (reomvelenght > 0)
            {
                for (int i = index; i < reomvelenght; i++)
                    collection.RemoveAt(i);
            }
        }

        /// <summary>
        /// 提供检查器检查集合所有内容是否符合要求
        /// </summary>
        /// <param name="source">集合</param>
        /// <param name="funcCheck">检查器</param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static bool ForCheck<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> funcCheck)
        {
            if (source.HasAny() && funcCheck != null)
            {
                return source.Select(funcCheck).All(flag => flag);
            }
            return true;
        }
    }

    class CommonEqualityComparer<T, TV> : IEqualityComparer<T>
    {
        private readonly Func<T, TV> _keySelector;
        private readonly IEqualityComparer<TV> _comparer;

        public CommonEqualityComparer(Func<T, TV> keySelector, IEqualityComparer<TV> comparer)
        {
            this._keySelector = keySelector;
            this._comparer = comparer;
        }

        public CommonEqualityComparer(Func<T, TV> keySelector)
            : this(keySelector, EqualityComparer<TV>.Default)
        { }

        public bool Equals(T x, T y)
        {
            return _comparer.Equals(_keySelector(x), _keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return _comparer.GetHashCode(_keySelector(obj));
        }
    }
}

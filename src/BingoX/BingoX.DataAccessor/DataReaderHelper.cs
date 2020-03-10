using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace BingoX.DataAccessor
{
    public static class DataReaderHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="classType"></param>
        /// <param name="columnIndexMapping"></param>
        /// <returns></returns>
        public static T Cast<T>(this IDataReader dataReader) where T : class, new()
        {
            if (!dataReader.IsEffective()) return null;

            if (!dataReader.Read()) return null;
            var obj = new T();
            var mappingColumns = GetMapping(typeof(T), dataReader);
            Cast(dataReader, obj, mappingColumns);
            return obj;
        }
        private static void Cast<T>(this IDataRecord dataReader, T obj, IDictionary<int, PropertyInfo> mappingColumns)
        {
            if (dataReader == null)
                return;
            foreach (var mapping in mappingColumns)
            {


                var oldojb = dataReader.GetValue(mapping.Key);
                if (oldojb == null || Convert.IsDBNull(oldojb)) continue;
                var pvalue = BingoX.Utility.ObjectUtility.Cast(oldojb, mapping.Value.PropertyType, oldojb);

                BingoX.Helper.FastReflectionExtensions.FastSetValue(mapping.Value, obj, pvalue);
            }

        }
        /// <summary>
        /// 是否有效
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static bool IsEffective(this IDataReader dataReader)
        {
            return dataReader != null && !dataReader.IsClosed;
        }
        public static IList<T> GetList<T>(this IDataReader dataReader) where T : class, new()
        {
            List<T> list = new List<T>(); if (!dataReader.IsEffective())
                return list;
            var mappingColumns = GetMapping(typeof(T), dataReader);


            while (dataReader.Read())
            {
                T row = new T();
                Cast(dataReader, row, mappingColumns);
                list.Add(row);
            }

            return list;

        }
        private static IDictionary<int, PropertyInfo> GetMapping(Type type, IDataRecord dataRecord)
        {
            if (type == null || type.IsInterface || type.IsAbstract)
                return null;


            var properties = type.GetProperties();
            IDictionary<int, PropertyInfo> list = new Dictionary<int, PropertyInfo>();
            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                var name = dataRecord.GetName(i);
                var propertyInfo = properties.FirstOrDefault(n => n.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
                if (propertyInfo == null && !propertyInfo.CanWrite)
                    continue;


                list.Add(i, propertyInfo);
            }



            return list;

        }
    }
}

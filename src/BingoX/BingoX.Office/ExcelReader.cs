using BingoX.Helper;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BingoX.Office
{
    public class ExcelReader 
    {
        /// <summary>
        /// 查询EXCEL文件，第一张表的数据
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static DataTable GetFirstSheet(Stream filepath, bool firstRowIsHeader = true)
        {
            var reader = ExcelReaderFactory.CreateReader(filepath);
            var ds = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = firstRowIsHeader
                }
            });
            return ds.Tables[0];
        }

        public static IList<T> GetFirstSheet<T>(Stream filepath) where T : class, new()
        {
            var reader = ExcelReaderFactory.CreateReader(filepath);
            var properties = typeof(T).GetProperties();
            IDictionary<int, PropertyInfo> dic = new Dictionary<int, PropertyInfo>();
            IList<T> list = new List<T>();
            if (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetString(i);
                    var property = properties.FirstOrDefault(n => string.Equals(n.Name, name, StringComparison.CurrentCultureIgnoreCase));
                    if (property != null) dic.Add(i, property);
                }
                while (reader.Read())
                {
                    T item = new T();
                    foreach (var keyValuePair in dic)
                    {
                        var prop = keyValuePair.Value;
                        prop.FastSetValue(item, BingoX.Utility.ObjectUtility.Cast(reader.GetValue(keyValuePair.Key), prop.PropertyType));
                    }
                    list.Add(item);
                }

            }
            reader.Close();
            return list;
        }
    }
}

using BingoX.Helper;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace BingoX.Office
{
    public class ExcelWriter
    {
        public static IDictionary<string, string> GetTableColumn(Type tableType)
        {


            var exports = tableType.GetProperties().Select(n => new { Property = n, Export = n.GetAttribute<ExportDisplayNameAttribute>() }).Where(n => n.Export != null).OrderBy(n => n.Export.Order);
            IDictionary<string, string> dic = exports.ToDictionary(n => n.Property.Name, n => n.Export.DisplayName);

            if (dic.IsEmpty()) dic = ListBindingHelper.GetListItemProperties(tableType).OfType<PropertyDescriptor>().Where(n => (!string.IsNullOrEmpty(n.DisplayName) && n.DisplayName != n.Name) || string.IsNullOrEmpty(n.DisplayName)).ToDictionary(n => n.Name, n => string.IsNullOrEmpty(n.DisplayName) ? n.Name : n.DisplayName);
            return !dic.IsEmpty() ? dic : null;
        }
        public static void ToFile(Stream stream, object datasource, ExcelWriteConfig config = null)
        {
            if (datasource == null) throw new LogicException("数据源为空");

            IList list = BingoX.Helper.ListBindingHelper.GetList(datasource) as IList;
            if (list == null) throw new LogicException("无效数据源");


            var type = BingoX.Helper.ListBindingHelper.GetListItemType(datasource);
            var properties = BingoX.Helper.ListBindingHelper.GetListItemProperties((object)list);
            IDictionary<string, string> keyValuePairs = null;
            if (config == null || config.MapperColumns.IsEmpty())
            {

                keyValuePairs = GetTableColumn(type);

            }
            else
            {
                keyValuePairs = config.MapperColumns.Where(n => properties.Find(n.Key, true) != null).ToDictionary(n => n.Key, n => n.Value);
            }

            if (keyValuePairs.IsEmpty())
            {
                keyValuePairs = properties.OfType<PropertyDescriptor>().Select(x => x.Name).ToDictionary(n => n, n => n);
            }





            var worksheetname = config?.SheetName ?? "sheet1";

            IWorkbook book = null;
            switch (config?.ExcelType)
            {
                case ExcelType.Xls:
                    {
                        book = new HSSFWorkbook();
                        break;
                    }

                case ExcelType.Xlsx:
                    {
                        book = new XSSFWorkbook();
                        break;
                    }
                default:
                    break;
            }
            var sheet = book.CreateSheet(worksheetname);
            var rowindex = 0;
           
            {
                var header = sheet.CreateRow(rowindex);

                var dis = keyValuePairs.Values.ToArray();
                for (int i = 0; i < dis.Length; i++)
                {
                    var cell = header.CreateCell(i, CellType.String);
                    cell.SetCellValue(dis[i]);

                }
                rowindex++;
            }
            foreach (var item in list)
            {
                var row = sheet.CreateRow(rowindex);
                var colindex = 0;
                foreach (var colname in keyValuePairs.Keys)
                {
                    var property = properties.Find(colname, true);
                    if (property == null) continue;

                    var cell = row.CreateCell(colindex, CellType.String);
                    var v = property.GetValue(item);
                    cell.SetCellValue(v == null ? string.Empty : v.ToString());
                    colindex++;
                }
                rowindex++;
            }

            book.Write(stream);
            book.Close();
        }
    }
}

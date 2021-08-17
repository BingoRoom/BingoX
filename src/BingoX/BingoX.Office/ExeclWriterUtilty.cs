using BingoX.Helper;
using ExcelDataReader;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.Office
{
    public interface IWriterElement
    {

    }
    public interface IWriterUtilty
    {
        DocumentInfo Info { get; set; }
        void Write(Stream stream);
    }
    public class DocumentInfo
    {

        public string Creator { get; set; }
        public string Subject { get; set; }

        public string Keywords { get; set; }
        public string Identifier { get; set; }
        public string Description { get; set; }

        public string Category { get; set; }
        public string Title { get; set; }
    }
    public class ExeclWriterUtilty : IWriterUtilty
    {
        public ExcelType ExcelType { get; set; }
        public DocumentInfo Info { get; set; }

        readonly IList<IExeclWriterElement> elements = new List<IExeclWriterElement>();
        public DataTableElement AddDataTable(object dataSource)
        {

            var el = new DataTableElement(dataSource);
            elements.Add(el);
            return el;
        }
        public MergeCellElement AddMergeCell(object value)
        {
            var el = new MergeCellElement(value);
            elements.Add(el);
            return el;
        }


        public PriceElement AddPrice(byte[] price)
        {
            var el = new PriceElement(price);
            elements.Add(el);
            return el;
        }
        public void Write(Stream stream)
        {
            IWorkbook book = null;
            switch (this.ExcelType)
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
            var sheet1 = book.CreateSheet("sheet1");
            int rowcount = 0;
            foreach (var item in elements)
            {

                switch (item)
                {
                    case PriceElement priceElement:

                        break;
                    case MergeCellElement mergeCellElement:
                        {
                            var y = mergeCellElement.Location.Y;
                            if (y > 0)
                            {

                            }
                        }
                        break;
                    case DataTableElement dataTableElement:
                        {

                        }
                        break;
                }
            }
        }
    }
    public interface IExeclWriterElement
    {
        System.Drawing.Point Location { get; set; }
    }
    public class PriceElement : IExeclWriterElement
    {
        public PriceElement(byte[] price)
        {
            Price = price;
        }
        public byte[] Price { get; private set; }

        public System.Drawing.Point Location { get; set; }

    }
    public class MergeCellElement : IExeclWriterElement
    {
        public MergeCellElement(object value)
        {
            Value = value;
        }
        public object Value { get; private set; }

        public System.Drawing.Point Location { get; set; }

        public System.Drawing.Size Size { get; set; }

        public ICellStyle Style { get; set; }
    }
    public class DataTableElement : IExeclWriterElement
    {
        public DataTableElement(object dataSource, IDictionary<string, string> mapperColumns = null)
        {
            DataSource = ListBindingHelper.GetList(dataSource) as IList;
            ItemType = ListBindingHelper.GetListItemType(dataSource);
            Properties = ListBindingHelper.GetListItemProperties(DataSource);
            if (mapperColumns.IsEmpty())
            {
                MapperColumns = ExcelWriter.GetTableColumn(ItemType);
            }
            else
            {
                //过滤不存在的列
                MapperColumns = mapperColumns.Where(n => Properties.Find(n.Key, true) != null).ToDictionary(n => n.Key, n => n.Value);
            }

            if (MapperColumns.IsEmpty()) MapperColumns = Properties.OfType<PropertyDescriptor>().ToDictionary(n => n.Name, n => string.IsNullOrEmpty(n.DisplayName) ? n.Name : n.DisplayName);

        }
        public string ThemeStyle { get; set; }

        public System.Drawing.Point Location { get; set; }

        public int MinRows { get; set; }

        public object DataSource { get; private set; }
        public Type ItemType { get; private set; }
        public PropertyDescriptorCollection Properties { get; private set; }

        public IDictionary<string, string> MapperColumns { get; private set; }
    }
}

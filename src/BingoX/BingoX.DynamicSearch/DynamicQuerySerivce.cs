using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;

namespace BingoX.DynamicSearch
{
    public class DynamicSearchSerivce
    {



        public DynamicSearchSerivce(DynamicDataAccessorCollection dataAccessors, DynamicTableInfoCollection tableInfoCollection)
        {
            this.DataAccessorCollection = dataAccessors;
            Tables = tableInfoCollection;
        }

        public DynamicTableInfoCollection Tables { get; private set; }
        public DynamicDataAccessorCollection DataAccessorCollection { get; private set; }
        public object GetId(string code, object id)
        {
            var table = Tables[code];
            if (table == null) throw new Exception("未配置对应的查询");
            var dataAccessor = DataAccessorCollection[table.Node];
            switch (dataAccessor)
            {
                case SqlSugarDynamicDataAccessor sqlSugar:
                    return sqlSugar.GetId(table,id);
                default:
                    break;
            }
            throw new Exception();
        }
        public DataSet Query(string code, NameValueCollection filters = null, int? pageIndex = null, int? pageSize = null)
        {
            var table = Tables[code];
            if (table == null) throw new Exception("未配置对应的查询");
          
            long total = 0;
            var ts = Query(table, filters, pageIndex, pageSize, ref total);
            DataSet ds = ts.DataSet;
        

            DataTable tableStatistics = new DataTable("statistics");
            tableStatistics.Columns.Add("name");
            tableStatistics.Columns.Add("value");
            tableStatistics.Rows.Add(new object[] { "total", total });
            ds.Tables.Add(tableStatistics);
            return ds;
        }

        DataTable Query(DynamicTableInfo table, NameValueCollection filters, int? pageIndex, int? pageSize, ref long total)
        {

            var dataAccessor = DataAccessorCollection[table.Node];
            switch (dataAccessor)
            {
                case SqlSugarDynamicDataAccessor sqlSugar:               
                    return sqlSugar.GetData(table, filters, pageIndex, pageSize, ref total);
                default:
                    break;
            }
            throw new Exception();
          




        }

    }

}

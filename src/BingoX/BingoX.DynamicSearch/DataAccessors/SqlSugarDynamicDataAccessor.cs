using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;

namespace BingoX.DynamicSearch
{
    public class DSNDynamicDataAccessor : IDynamicDataAccessor, IDbDynamicDataAccessor
    {
        public DSNDynamicDataAccessor(string nodeName, string dsnName)
        {
            Node = nodeName;
            DsnName = dsnName;
        }

        public string Node { get; private set; }


        public string DsnName { get; private set; }

        public DataTable GetData(string sql, IDictionary<string, object> dataParameters)
        {
            throw new NotImplementedException();
        }

        public string[] GetFieldNames(string tablename)
        {
            throw new NotImplementedException();
        }

        public string GetSafeName(string fieldName)
        {
            throw new NotImplementedException();
        }

        public long GetTotal(string sql, IDictionary<string, object> dataParameters)
        {
            throw new NotImplementedException();
        }

        public bool HasTable(string tablename)
        {
            throw new NotImplementedException();
        }
    }
    public class SqlSugarDynamicDataAccessor : IDynamicDataAccessor, IDbDynamicDataAccessor
    {
        private readonly SqlSugarClient client;
        readonly SqlBuilderProvider sqlBuilder;
        readonly SqlSugar.ConnectionConfig config;
        public SqlSugarDynamicDataAccessor(string node, string connection, string dbtype)
        {
            Node = node;
            ConnectionStrting = connection;
            DbType = dbtype;
            config = new ConnectionConfig()
            {
                ConnectionString = connection,
                DbType = BingoX.Utility.StringUtility.Cast<SqlSugar.DbType>(dbtype)
            };

            this.client = new SqlSugarClient(config);
            switch (client.CurrentConnectionConfig.DbType)
            {
                case SqlSugar.DbType.MySql:
                    sqlBuilder = new MySqlBuilder();
                    break;
                case SqlSugar.DbType.SqlServer:
                    sqlBuilder = new SqlServerBuilder();
                    break;
                case SqlSugar.DbType.Sqlite:
                    sqlBuilder = new SqliteBuilder();
                    break;
                case SqlSugar.DbType.Oracle:
                    sqlBuilder = new OracleBuilder();
                    break;
                case SqlSugar.DbType.PostgreSQL:
                    sqlBuilder = new PostgreSQLBuilder();
                    break;
                default:
                    break;
            }

        }

        public DataTable GetData(DynamicTableInfo table, NameValueCollection filters, int? pageIndex, int? pageSize, ref long total)
        {

            var showfield = table.Fields == null ? "*" : string.Join(",", table.Fields.Where(n => n.FieldJoin == null).Select(n => GetSafeName(n.FieldName)));
            string sql = $"select {showfield} from {GetSafeName(table.TableName)}";

            if (filters == null || filters.Count == 0)
            {
                total = GetTotal($"select count(0) from {GetSafeName(table.TableName)} ", null);
                return GetData(sql, null);
            }

            var conditionals = table.FilterFields.Select(n => new ConditionalModel
            {
                ConditionalType = ToConditionalType(n.FilterType),
                FieldName = n.FieldName,
                FieldValue = filters[n.FieldName]

            }).Where(n => n.FieldValue != null).Cast<IConditionalModel>().ToList();

            var query = client.Queryable(table.TableName, table.TableName);
            var builder = query.Where(conditionals);
            total = builder.Count();
            if (pageIndex == null) pageIndex = 0;
            if (pageSize == null) pageSize = 10;
            var dt = builder.ToDataTablePage(pageIndex.Value, pageSize.Value);
            return dt;
        }

        private ConditionalType ToConditionalType(FilterType filterType)
        {
            switch (filterType)
            {
                case FilterType.Equals:
                    return ConditionalType.Equal;
                case FilterType.Like:
                    return ConditionalType.Like;
                case FilterType.Greater:
                    return ConditionalType.GreaterThan;
                case FilterType.GreaterEquals:
                    return ConditionalType.GreaterThanOrEqual;
                case FilterType.Lesser:
                    return ConditionalType.LessThan;
                case FilterType.LesserEquals:
                    return ConditionalType.LessThanOrEqual;
                case FilterType.NotIn:
                    return ConditionalType.NotIn;
                case FilterType.In:
                    return ConditionalType.In;
                case FilterType.StartLike:
                    return ConditionalType.LikeLeft;
                case FilterType.EndLike:
                    return ConditionalType.LikeRight;
                default:
                    return ConditionalType.Equal;
            }

        }

        public string Node { get; private set; }
        public string ConnectionStrting { get; private set; }
        public string DbType { get; private set; }

        public DataTable GetData(string sql, IDictionary<string, object> dataParameters)
        {
            if (dataParameters == null) return client.Ado.GetDataTable(sql);
            return client.Ado.GetDataTable(sql, dataParameters.Select(n => new SugarParameter(n.Key, n.Value)).ToArray());

        }

        public string[] GetFieldNames(string tablename)
        {
            return client.DbMaintenance.GetColumnInfosByTableName(tablename, false)?.Select(n => n.DbColumnName).ToArray();

        }

        public string GetSafeName(string fieldName)
        {
            return sqlBuilder.GetTranslationColumnName(fieldName);

        }

        public long GetTotal(string sql, IDictionary<string, object> dataParameters)
        {
            if (dataParameters == null) return client.Ado.GetLong(sql);
            return client.Ado.GetLong(sql, dataParameters.Select(n => new SugarParameter(n.Key, n.Value)).ToArray());
        }

        public bool HasTable(string tablename)
        {
            return client.DbMaintenance.GetTableInfoList(false).Any(n => string.Equals(n.Name, tablename));

        }

        public object GetId(DynamicTableInfo table, object id)
        {
            var showfield = table.Fields == null ? "*" : string.Join(",", table.Fields.Where(n => n.FieldJoin == null).Select(n => GetSafeName(n.FieldName)));
            string sql = $"select {showfield} from {GetSafeName(table.TableName)} where {GetSafeName(table.PrimaryKey)} = {id}";
            return client.Ado.SqlQueryDynamic(sql);
        }
    }
}

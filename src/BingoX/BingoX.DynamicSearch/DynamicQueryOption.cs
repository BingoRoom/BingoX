using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BingoX.DynamicSearch
{
    public class DynamicQueryOption
    {



        public DynamicTableInfoCollection Tables { get; private set; }
        public DynamicDataAccessorCollection DataAccessors { get; private set; }


        public void LoadConfig(string filePath)
        {
            DynamicSchemaReader reader = new DynamicSchemaReader(filePath);
            reader.LoadConfig();
            Tables = reader.Tables;
            DataAccessors = reader.DataAccessors;
        }
    }
    public class DynamicSchemaReader
    {
        JObject jObj;
        public DynamicSchemaReader(string path)
        {
            if (!System.IO.File.Exists(path)) throw new Exception("配置文件不存在");
            DataAccessors = new DynamicDataAccessorCollection();
            Tables = new DynamicTableInfoCollection();
            jObj = JObject.Parse(System.IO.File.ReadAllText(path));

        }
        public DynamicSchemaReader() : this(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json"))
        {

        }
        public DynamicTableInfoCollection Tables { get; private set; }
        public DynamicDataAccessorCollection DataAccessors { get; private set; }
        public void LoadConfig()
        {
            GetDataNode();
            GetTables();
        }
        void GetTables()
        {
            var childres = jObj.SelectToken("$.schema.table").Children().Cast<Newtonsoft.Json.Linq.JProperty>().ToList();
            foreach (var item in childres)
            {
                var val = item.Value;
                DynamicTableInfo info = new DynamicTableInfo
                {

                    TableName = val["tablename"]?.ToString(),
                    Node = val["node"]?.ToString(),
                    Code = item.Name,

                    SupportGetId = BingoX.Utility.StringUtility.Cast<bool>(val["supportGetId"]?.ToString()),
                    HasPage = BingoX.Utility.StringUtility.Cast<bool>(val["hasPage"]?.ToString())
                };
                if (info.SupportGetId)
                {
                    info.PrimaryKey = val["primaryKey"]?.ToString();
                    if (string.IsNullOrEmpty(info.PrimaryKey)) throw new Exception("支持主键查询果，必须指定主键字段");
                }
                var classType = val["class"]?.ToString();
                if (!string.IsNullOrEmpty(classType))
                {
                    info.Class = Type.GetType(classType);
                }
                if (val["filterField"] != null)
                {
                    var filterField = val["filterField"].Cast<Newtonsoft.Json.Linq.JObject>().ToArray();
                    info.FilterFields = doFilterFields(filterField);
                }
                if (val["queryFields"] != null)
                {
                    var queryFields = val["queryFields"].Cast<Newtonsoft.Json.Linq.JObject>().ToArray();
                    info.Fields = doQueryFields(queryFields);
                }
                Tables.AddTable(info);

            }
        }
        IDictionary<string, string> ToDictionary(Newtonsoft.Json.Linq.JProperty[] filters)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in filters)
            {
                dic.Add(item.Name, item.Value?.ToString());
            }
            return dic;
        }
        DynamicQueryField[] doQueryFields(Newtonsoft.Json.Linq.JObject[] filters)
        {
            List<DynamicQueryField> list = new List<DynamicQueryField>();
            foreach (var item in filters)
            {
                var field = new DynamicQueryField()
                {
                    FieldName = item["fieldName"]?.ToString(),
                    DisplayName = item["dsisplayName"]?.ToString(),

                };
                var convert = item["convert"]?.ToString();
                if (!string.IsNullOrEmpty(convert))
                {
                    var type = Type.GetType(convert);
                    field.Convert = BingoX.Helper.FastReflectionExtensions.CreateInstance<IConvert>(type);
                }
                var fieldJoinJson = item["fieldJoin"];
                if (fieldJoinJson != null)
                {
                    var node = fieldJoinJson["node"]?.ToString();
                    switch (DataAccessors[node])
                    {
                        case IWebRemoteDynamicDataAccessor web:
                            {
                                var join = new WebApiFieldJoinInfo
                                {
                                    Url = fieldJoinJson["url"]?.ToString(),
                                    Args = ToDictionary(fieldJoinJson["args"].Children().Cast<Newtonsoft.Json.Linq.JProperty>().ToArray()),

                                    Node = node,
                                    JoinType = BingoX.Utility.StringUtility.Cast<FieldJoinType>(fieldJoinJson["filterType"]?.ToString())
                                };
                                if (fieldJoinJson["fields"] != null)
                                    join.Fields = doQueryFields(fieldJoinJson["fields"].Children().Cast<Newtonsoft.Json.Linq.JObject>().ToArray());
                                field.FieldJoin = join;
                                break;
                            }
                        case IDbDynamicDataAccessor db:
                            {
                                var join = new DbFieldJoinInfo
                                {
                                    ForeignKey = fieldJoinJson["foreignKey"]?.ToString(),
                                    PrimaryKey = fieldJoinJson["primaryKey"]?.ToString(),
                                    TableName = fieldJoinJson["tablename"]?.ToString(),
                                    CustomSQL = fieldJoinJson["customSQL"]?.ToString(),
                                    Node = node,
                                    Fields = doQueryFields(fieldJoinJson["fields"].Children().Cast<Newtonsoft.Json.Linq.JObject>().ToArray()),
                                    JoinType = BingoX.Utility.StringUtility.Cast<FieldJoinType>(fieldJoinJson["filterType"]?.ToString())
                                };
                                field.FieldJoin = join;
                                break;
                            }
                        default:
                            break;
                    }
                }
                list.Add(field);
            }
            return list.ToArray();
        }
        DynamicFilterField[] doFilterFields(Newtonsoft.Json.Linq.JObject[] filters)
        {
            List<DynamicFilterField> list = new List<DynamicFilterField>();
            foreach (var item in filters)
            {
                var field = new DynamicFilterField()
                {
                    FieldName = item["fieldName"]?.ToString(),
                    Name = item["name"]?.ToString(),
                    FilterType = BingoX.Utility.StringUtility.Cast<FilterType>(item["filterType"]?.ToString())
                };
                list.Add(field);
            }
            return list.ToArray();
        }
        void GetDataNode()
        {
            IDynamicDataAccessor[] dataAccessor = jObj.SelectToken("$.dataNode").Children().Cast<Newtonsoft.Json.Linq.JObject>().Select(n =>
             {
                 IDynamicDataAccessor dynamicaccess = null;
                 var nodeType = jObj.SelectToken(n.Path + ".nodeType")?.ToString();
                 var nodeName = jObj.SelectToken(n.Path + ".nodeName")?.ToString();
                 switch (nodeType)
                 {
                     case "db":
                         {
                             var dbtype = jObj.SelectToken(n.Path + ".dbtype")?.ToString();
                             var connectionStrting = jObj.SelectToken(n.Path + ".connectionStrting")?.ToString();
                             dynamicaccess = new SqlSugarDynamicDataAccessor(nodeName, connectionStrting, dbtype);
                             break;
                         }
                     case "dsn":
                         {

                             var dsnName = jObj.SelectToken(n.Path + ".dsnName")?.ToString();
                             dynamicaccess = new DSNDynamicDataAccessor(nodeName, dsnName);
                             break;
                         }
                     case "webremote":
                         {
                             var remotetype = jObj.SelectToken(n.Path + ".remotetype")?.ToString();
                             var hosturl = jObj.SelectToken(n.Path + ".hosturl")?.ToString();
                             dynamicaccess = WebRemoteDynamicDataAccessor.Create(nodeName, remotetype, hosturl);
                             break;
                         }
                     default:
                         throw new NotSupportedException(nodeType);
                 }
                 return dynamicaccess;

             }).ToArray();
            foreach (var item in dataAccessor)
            {

                DataAccessors.Add(item);
            }
        }
    }

}

using Newtonsoft.Json.Linq;
using System;

namespace BingoX.DynamicSearch
{
    public class DynamicTableInfo
    {
        public string Code { get; set; }

        public string TableName { get; set; }

        public Type ClassType { get; set; }

        public DynamicQueryField[] Fields { get; set; }

        public DynamicFilterField[] FilterFields { get; set; }
        public string Node { get; set; }
        public string PrimaryKey { get; set; }
        public bool SupportGetId { get; set; }
        public bool HasPage { get; set; }
        public Type Class { get; set; }
    }

}

using System.Collections.Generic;

namespace BingoX.DynamicSearch
{
    public class FieldJoinInfo
    {

        public FieldJoinType JoinType { get; set; }

        public string Node { get; set; }
        public DynamicQueryField[] Fields { get; set; }
    }

    public class DbFieldJoinInfo : FieldJoinInfo
    {
        public string ForeignKey { get; set; }

        public string PrimaryKey { get; set; }
        public string TableName { get; set; }
        public string CustomSQL { get; set; }
    }
    public class WebApiFieldJoinInfo : FieldJoinInfo
    {
        public string Url { get; set; }

        public  IDictionary<string,string> Args { get; set; }
        public string TableName { get; set; }
    }
}

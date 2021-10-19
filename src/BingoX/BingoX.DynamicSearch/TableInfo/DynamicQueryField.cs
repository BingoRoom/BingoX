namespace BingoX.DynamicSearch
{
    public class DynamicQueryField
    {
        public string FieldName { get; set; }
        public string DisplayName { get; set; }

        public FieldJoinInfo FieldJoin { get; set; }

        public IConvert Convert { get; set; }
    }

}

namespace BingoX.ComponentModel.Data
{
    public struct PageSizeDescriptor
    {
        public int Index { get; set; }
        public int Size { get; set; }
        public static PageSizeDescriptor Parse(QueryDescriptor query)
        {
            return new PageSizeDescriptor { Index = query.PageIndex, Size = query.PageSize, };
        }
    }
}
namespace BingoX.ComponentModel.Data
{
    public struct SortDescriptor
    {
        public SortingDirection Direction { get; set; }
        public string Field { get; set; }
        public enum SortingDirection
        {
            Ascending,
            Descending
        }
    }
}
namespace BingoX.ComponentModel.Data
{
    public class SortDescriptor
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
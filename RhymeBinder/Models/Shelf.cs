namespace RhymeBinder.Models
{
    public class Shelf
    {
        public int ShelfId { get; set; }
        public int UserId { get; set; }
        public int BinderId { get; set; }
        public int ShelfLevel { get; set; }
        public int SortOrder {  get; set; }
        public Binder Binder { get; set; }
        public SimpleUser User { get; set; }
        
    }
}

namespace RhymeBinder.Models
{
    public class DisplayBinder : Binder
    {
        public int PageCount { get; set; }
        public int GroupCount { get; set; }

        public string CreatedByName { get; set; }

        public string ModifyByName { get; set; }

    }
}

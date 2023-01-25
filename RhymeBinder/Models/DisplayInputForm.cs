using System.Collections.Generic;


namespace RhymeBinder.Models
{
    public class DisplayInputForm
    {
        public int UserId { get; set; }
        public List<Binder> Binders { get; set;}
        public ImportEntry ImportEntry { get; set; }
        public List<DisplayFileImportResult> Results { get; set; }
    }
}

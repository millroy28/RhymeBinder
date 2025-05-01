using System.Collections.Generic;
using RhymeBinder.Models.DBModels;
using RhymeBinder.Models.ImportModels;


namespace RhymeBinder.Models.ViewModels
{
    public class DisplayInputForm
    {
        public int UserId { get; set; }
        public List<Binder> Binders { get; set; }
        public ImportEntry ImportEntry { get; set; }
        public List<DisplayFileImportResult> Results { get; set; }
    }
}

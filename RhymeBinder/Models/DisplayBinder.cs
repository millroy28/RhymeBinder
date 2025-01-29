using System.Collections.Generic;

namespace RhymeBinder.Models
{
    public class DisplayBinder : Binder
    {
        public int PageCount { get; set; }
        public int GroupCount { get; set; }
        public string CreatedByName { get; set; }
        public string ModifyByName { get; set; }
        public string LastAccessedByName { get; set; }
        public string WorkedInName { get; set; }

    }
}

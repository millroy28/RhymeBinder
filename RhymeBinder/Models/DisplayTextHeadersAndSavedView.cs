using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RhymeBinder.Models
{
    public class DisplayTextHeadersAndSavedView
    {
        public List<DisplayTextHeader> TextHeaders { get; set; }
        public SavedView View { get; set; }
        public List<TextGroup> Groups { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RhymeBinder.Models
{
    public class DisplayTextHeader : TextHeader
    {
        public string CreatedByName { get; set; }

        public string ModifyByName { get; set; }

        public string ReadByName { get; set; }

        public string RevisionStatus { get; set; }
        public List<TextGroup> Groups { get; set; }
        public bool Selected { get; set; }
        public int? GroupSequence { get; set; }
    }
}

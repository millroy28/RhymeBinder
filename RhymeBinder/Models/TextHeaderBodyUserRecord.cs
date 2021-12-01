using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RhymeBinder.Models
{
    public class TextHeaderBodyUserRecord
    {
        public Text Text { get; set; }
        
        public TextHeader TextHeader { get; set; }

        public SimpleUser User { get; set; }

        public List<TextRevisionStatus> RevisionStatuses { get; set; }

        public List<TextHeader> PreviousTextHeaders { get; set; }

        public List<Text> PreviousTexts { get; set; }

        //public List<TextRecord> TextHistory { get; set; }

    }
}

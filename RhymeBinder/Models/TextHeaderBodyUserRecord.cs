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

        //public List<TextRecord> TextHistory { get; set; }

    }
}

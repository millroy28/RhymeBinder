using System;
using System.Collections.Generic;

namespace RhymeBinder.Models.DBModels
{
    public partial class TextNote
    {
        public TextNote()
        {
            TextHeaders = new HashSet<TextHeader>();
        }

        public int TextNoteId { get; set; }
        public string Note { get; set; }

        public virtual ICollection<TextHeader> TextHeaders { get; set; }
    }
}

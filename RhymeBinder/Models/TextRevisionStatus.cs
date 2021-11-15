using System;
using System.Collections.Generic;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class TextRevisionStatus
    {
        public TextRevisionStatus()
        {
            TextHeaders = new HashSet<TextHeader>();
        }

        public int TextRevisionStatusId { get; set; }
        public string TextRevisionStatus1 { get; set; }

        public virtual ICollection<TextHeader> TextHeaders { get; set; }
    }
}

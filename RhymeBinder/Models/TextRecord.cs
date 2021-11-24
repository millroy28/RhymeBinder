using System;
using System.Collections.Generic;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class TextRecord
    {
        public int TextRecordId { get; set; }
        public int TextHeaderId { get; set; }
        public int TextId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Recorded { get; set; }

        public virtual Text Text { get; set; }
        public virtual TextHeader TextHeader { get; set; }
        public virtual SimpleUser User { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class EditWindowProperty
    {
        public int EditWindowPropertyId { get; set; }
        public int? UserId { get; set; }
        public int TextHeaderId { get; set; }
        public string ActiveElement { get; set; }
        public int? CursorPosition { get; set; }
        public int? ShowLineCount { get; set; }
        public int? ShowParagraphCount { get; set; }

        public virtual TextHeader TextHeader { get; set; }
        public virtual SimpleUser User { get; set; }
    }
}

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
        public int? TextAreaFocus { get; set; }
        public int? CursorPosition { get; set; }

        public virtual TextHeader TextHeader { get; set; }
        public virtual SimpleUser User { get; set; }
    }
}

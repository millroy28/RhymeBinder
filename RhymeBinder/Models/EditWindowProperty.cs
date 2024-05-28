using System;
using System.Collections.Generic;

namespace RhymeBinder.Models
{
    public partial class EditWindowProperty
    {
        public int EditWindowPropertyId { get; set; }
        public int? UserId { get; set; }
        public int TextHeaderId { get; set; }
        public string ActiveElement { get; set; }
        public int? BodyCursorPosition { get; set; }
        public int? BodyScrollPosition { get; set; }
        public int? NoteCursorPosition { get; set; }
        public int? NoteScrollPosition { get; set; }
        public int? TitleCursorPosition { get; set; }
        public int? ShowLineCount { get; set; }
        public int? ShowParagraphCount { get; set; }

        public virtual TextHeader TextHeader { get; set; }
        public virtual SimpleUser User { get; set; }
    }
}

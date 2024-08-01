using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RhymeBinder.Models
{
    public class TextEdit
    {
        public int UserId { get;set; }

        // Text fields
        public int TextId { get; set; }
        public string TextBody { get; set; }

        // Text Note Fields
        public int? TextNoteId { get; set; }
        public string Note { get; set; }

        // Text Header Fields
        public int TextHeaderId { get; set; }
        public string Title { get; set; }
        public DateTime? Created { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastRead { get; set; }
        public int? LastReadBy { get; set; }
        public int? TextRevisionStatusId { get; set; }
        public int? VisionNumber { get; set; }
        public int? VersionOf { get; set; }
        public DateTime? VisionCreated { get; set; }
        public int? VisionCreatedBy { get; set; }
        public bool? Deleted { get; set; }
        public bool? Locked { get; set; }
        public bool? Top { get; set; }
        public int? BinderId { get; set; }


        //  Display fields
        public string DisplayTitle { get; set; }
        public string CreatedByUserName { get; set; }
        public string LastModifiedByUserName { get; set; }
        public string CurrentRevisionStatus { get; set; }
        public List<DisplayTextGroup> Groups { get; set; }
        //public List<TextGroup> MemberOfGroups { get; set; }
        //public List<TextGroup> AvailableGroups { get; set; }

        //  EditWindowProperty fields
        public int EditWindowPropertyId { get; set; }
        public string ActiveElement { get; set; }
        public int? BodyCursorPosition { get; set; }
        public int? BodyScrollPosition { get; set; }
        public int? NoteCursorPosition { get; set; }
        public int? NoteScrollPosition { get; set; }
        public int? TitleCursorPosition { get; set; }
        public int? ShowLineCount { get; set; }
        public int? ShowParagraphCount { get; set; }

        //  Revision Status Dropdown fields
        public List<TextRevisionStatus> AllRevisionStatuses { get; set; }
        //  Previous Texts for left sidebar slideout
        public List<SimpleTextHeaderAndText> PreviousTexts { get; set; }

    }
}

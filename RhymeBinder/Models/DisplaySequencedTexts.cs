using System.Collections.Generic;

namespace RhymeBinder.Models
{
    public class DisplaySequencedTexts
    {
        //public string StartHeaderKey { get; } = "◮";
        //public string EndHeaderKey { get; } = "◭";
        //public string StartBodyKey { get; } = "◨";
        //public string EndBodyKey { get; } = "◧";
        // intended to serve view of all texts sequenced in a group
        public List<DisplaySimpleText> SimpleTexts { get; set; }
        public List<DisplaySimpleText> EditedSimpleTexts { get; set; }
        public string GroupName { get; set; }
        public int GroupId { get; set; }
        public int BinderId { get; set; }
        public string BinderName { get; set; }
        public string BinderColor {  get; set; }
        public string BinderNameColor {  get; set; }
        public bool BinderReadOnly { get; set; }
        public int UserId { get; set; }
    }
    public class DisplaySimpleText
    {
        public int TextHeaderId { get; set; }
        public string Title { get; set; }
        public string TextBody { get; set; }
        public string Note { get; set; }
        public int SequenceNumber { get; set; }
        public List<string> MemberOfGroups { get; set; }
        public bool IsChanged { get; set; } = false;
        // Do I want edit/create information here, or will that clutter things up. Here is where you would add it in the future
    }
}

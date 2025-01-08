using System.Collections.Generic;

namespace RhymeBinder.Models
{
    public class DisplaySequencedTexts
    {
        // intended to serve view of all texts sequenced in a group
        public List<DisplaySimpleText> SimpleTexts { get; set; }
        public string GroupName { get; set; }
        public int BinderId { get; set; }

    }
    public class DisplaySimpleText
    {
        public int TextHeaderId { get; set; }
        public string Title { get; set; }
        public string TextBody { get; set; }
        public int SequenceNumber { get; set; }
        public List<string> MemberOfGroups { get; set; }
        // Do I want edit/create information here, or will that clutter things up. Here is where you would add it in the future
    }
}

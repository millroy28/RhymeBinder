namespace RhymeBinder.Models.ViewModels
{
    public class DisplayTextMemberGroup
    {
        public int TextHeaderID { get; set; }
        public string GroupTitle { get; set; }
        public int? PreviousSequencedTextHeaderId { get; set; }
        public string PreviousSequencedTextHeaderTitle { get; set; } = string.Empty;
        public int? NextInSequenceTextHeaderId { get; set; }
        public string NextInSequenceTextHeaderTitle { get; set; } = string.Empty;
    }
}

namespace RhymeBinder.Models.DBModels
{

    public partial class AdjacentHeadersInSequence
    {
        public int TextHeaderID { get; set; }
        public string GroupTitle { get; set; }
        public int GroupId {  get; set; }
        public bool IsSequenced { get; set; }
        public int? PreviousInSequenceTextHeaderId { get; set; } = null;
        public string PreviousInSequenceTextHeaderTitle { get; set; } = string.Empty;
        public int? NextInSequenceTextHeaderId { get; set; } = null;
        public string NextInSequenceTextHeaderTitle { get; set; } = string.Empty;

    }
}

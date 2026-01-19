namespace RhymeBinder.Models.DBModels
{
    public class LnkMetadataTextHeader
    {
        public int LnkMetadataTextHeaderId { get; set; }
        public int BinderTextMetadataValueId { get; set; }
        public int TextHeaderId { get; set; }

        // Navigation properties
        //public virtual BinderTextMetadataValue BinderTextMetadataValue { get; set; } = null!;
        //public virtual TextHeader TextHeader { get; set; } = null!;

    }
}

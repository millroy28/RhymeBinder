namespace RhymeBinder.Models.DBModels
{
    public class TextHeaderStamp
    {
        public int TextHeaderId { get; set; }
        public int BinderTextMetadataValueId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public bool SelectedValue { get; set; }
        public virtual TextHeader TextHeader { get; set; } = null!;
        public virtual BinderTextMetadataValue BinderTextMetadataValue { get; set; } = null!;
    }
}

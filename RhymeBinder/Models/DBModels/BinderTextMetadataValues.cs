using System;

namespace RhymeBinder.Models.DBModels
{
    public class BinderTextMetadataValue
    {
        public int BinderTextMetadataValueId { get; set; }
        public int BinderTextMetadataHeaderId { get; set; }
        public string? Name { get; set; }
        public int? SortOrder { get; set; }

        // Navigation property
        //public virtual BinderTextMetadataHeader BinderTextMetadataHeader { get; set; } = null!;
    }
}

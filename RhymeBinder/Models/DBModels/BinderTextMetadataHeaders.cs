using System;
using System.Collections.Generic;

namespace RhymeBinder.Models.DBModels
{
    public class BinderTextMetadataHeader
    {
        public int BinderTextMetadataHeaderId { get; set; }
        public int BinderId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int CreatedById { get; set; }
        public DateTime Created { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModified { get; set; }

        // Navigation properties
        public virtual Binder Binder { get; set; } = null!;
        public virtual SimpleUser CreatedBy { get; set; } = null!;
        public virtual SimpleUser? LastModifiedBy { get; set; }
        public virtual ICollection<BinderTextMetadataValue> BinderTextMetadataValues { get; set; } = new List<BinderTextMetadataValue>();
        public virtual ICollection<LnkMetadataTextHeader> LnkMetadataTextHeaders { get; set; } = new List<LnkMetadataTextHeader>();

    }
}

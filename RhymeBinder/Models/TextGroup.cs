using System;
using System.Collections.Generic;

namespace RhymeBinder.Models
{
    public partial class TextGroup
    {
        public TextGroup()
        {
            GroupHistories = new HashSet<GroupHistory>();
            LnkTextHeadersTextGroups = new HashSet<LnkTextHeadersTextGroup>();
        }

        public int TextGroupId { get; set; }
        public string GroupTitle { get; set; }
        public int? OwnerId { get; set; }
        public string Notes { get; set; }
        public bool? Locked { get; set; }
        public bool? Hidden { get; set; }
        public int? BinderId { get; set; }
        public int? SavedViewId { get; set; }

        public virtual Binder Binder { get; set; }
        public virtual SimpleUser Owner { get; set; }
        public virtual SavedView SavedView { get; set; }
        public virtual ICollection<GroupHistory> GroupHistories { get; set; }
        public virtual ICollection<LnkTextHeadersTextGroup> LnkTextHeadersTextGroups { get; set; }
    }
}

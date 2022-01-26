using System;
using System.Collections.Generic;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class TextGroup
    {
        public TextGroup()
        {
            LnkTextHeadersTextGroups = new HashSet<LnkTextHeadersTextGroup>();
        }

        public int TextGroupId { get; set; }
        public string GroupTitle { get; set; }
        public int? OwnerId { get; set; }

        public virtual SimpleUser Owner { get; set; }
        public virtual ICollection<LnkTextHeadersTextGroup> LnkTextHeadersTextGroups { get; set; }
    }
}

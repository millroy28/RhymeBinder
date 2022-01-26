using System;
using System.Collections.Generic;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class LnkTextHeadersTextGroup
    {
        public int LnkHeaderGroupId { get; set; }
        public int? TextGroupId { get; set; }
        public int? TextHeaderId { get; set; }

        public virtual TextHeader TextGroup { get; set; }
        public virtual TextGroup TextHeader { get; set; }
    }
}

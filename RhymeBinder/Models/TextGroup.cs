﻿using System;
using System.Collections.Generic;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class TextGroup
    {
        public TextGroup()
        {
            TextHeaders = new HashSet<TextHeader>();
        }

        public int TextGroupId { get; set; }
        public string GroupTitle { get; set; }
        public int? OwnerId { get; set; }

        public virtual SimpleUser Owner { get; set; }
        public virtual ICollection<TextHeader> TextHeaders { get; set; }
    }
}

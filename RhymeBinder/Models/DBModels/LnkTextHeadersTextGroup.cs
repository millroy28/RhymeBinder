﻿using System;
using System.Collections.Generic;

namespace RhymeBinder.Models.DBModels
{
    public partial class LnkTextHeadersTextGroup
    {
        public int LnkHeaderGroupId { get; set; }
        public int TextHeaderId { get; set; }
        public int TextGroupId { get; set; }
        public int? Sequence { get; set; }
        public virtual TextGroup TextGroup { get; set; }
        public virtual TextHeader TextHeader { get; set; }
    }
}

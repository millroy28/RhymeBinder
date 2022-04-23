﻿using System;
using System.Collections.Generic;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class GroupHistory
    {
        public int GroupHistoryLogId { get; set; }
        public int? UserId { get; set; }
        public int? TextHeaderId { get; set; }
        public int? TextGroupId { get; set; }
        public int? GroupActionId { get; set; }
        public DateTime? DateLogged { get; set; }

        public virtual GroupAction GroupAction { get; set; }
        public virtual TextGroup TextGroup { get; set; }
        public virtual TextHeader TextHeader { get; set; }
        public virtual SimpleUser User { get; set; }
    }
}

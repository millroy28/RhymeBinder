using System;
using System.Collections.Generic;

namespace RhymeBinder.Models.DBModels
{
    public partial class GroupAction
    {
        public GroupAction()
        {
            GroupHistories = new HashSet<GroupHistory>();
        }

        public int GroupActionId { get; set; }
        public string GroupAction1 { get; set; }

        public virtual ICollection<GroupHistory> GroupHistories { get; set; }
    }
}

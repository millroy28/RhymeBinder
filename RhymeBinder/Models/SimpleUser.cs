using System;
using System.Collections.Generic;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class SimpleUser
    {
        public SimpleUser()
        {
            SavedViews = new HashSet<SavedView>();
            TextGroups = new HashSet<TextGroup>();
            TextHeaderCreatedByNavigations = new HashSet<TextHeader>();
            TextHeaderLastModifiedByNavigations = new HashSet<TextHeader>();
            TextHeaderLastReadByNavigations = new HashSet<TextHeader>();
            TextRecords = new HashSet<TextRecord>();
        }

        public int UserId { get; set; }
        public string AspNetUserId { get; set; }
        public string UserName { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public virtual ICollection<SavedView> SavedViews { get; set; }
        public virtual ICollection<TextGroup> TextGroups { get; set; }
        public virtual ICollection<TextHeader> TextHeaderCreatedByNavigations { get; set; }
        public virtual ICollection<TextHeader> TextHeaderLastModifiedByNavigations { get; set; }
        public virtual ICollection<TextHeader> TextHeaderLastReadByNavigations { get; set; }
        public virtual ICollection<TextRecord> TextRecords { get; set; }
    }
}

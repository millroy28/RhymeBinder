using System;
using System.Collections.Generic;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class Binder
    {
        public Binder()
        {
            SavedViews = new HashSet<SavedView>();
            TextGroups = new HashSet<TextGroup>();
            TextHeaders = new HashSet<TextHeader>();
        }

        public int BinderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? Created { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int? LastModifiedBy { get; set; }
        public bool? Hidden { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Selected { get; set; }

        public virtual SimpleUser CreatedByNavigation { get; set; }
        public virtual SimpleUser LastModifiedByNavigation { get; set; }
        public virtual SimpleUser User { get; set; }
        public virtual ICollection<SavedView> SavedViews { get; set; }
        public virtual ICollection<TextGroup> TextGroups { get; set; }
        public virtual ICollection<TextHeader> TextHeaders { get; set; }
    }
}

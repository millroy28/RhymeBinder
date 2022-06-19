using System;
using System.Collections.Generic;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class Binder
    {
        public Binder()
        {
            LnkTextHeadersBinders = new HashSet<LnkTextHeadersBinder>();
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

        public virtual SimpleUser CreatedByNavigation { get; set; }
        public virtual SimpleUser LastModifiedByNavigation { get; set; }
        public virtual SimpleUser User { get; set; }
        public virtual ICollection<LnkTextHeadersBinder> LnkTextHeadersBinders { get; set; }
    }
}

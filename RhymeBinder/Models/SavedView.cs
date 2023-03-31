using System;
using System.Collections.Generic;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class SavedView
    {
        public int SavedViewId { get; set; }
        public int? UserId { get; set; }
        public int RecordsPerPage { get; set; }
        public string SetValue { get; set; }
        public string SortValue { get; set; }
        public bool? Descending { get; set; }
        public string ViewName { get; set; }
        public bool? Default { get; set; }
        public bool? Saved { get; set; }
        public bool? LastView { get; set; }
        public bool? LastModified { get; set; }
        public bool? LastModifiedBy { get; set; }
        public bool? Created { get; set; }
        public bool? CreatedBy { get; set; }
        public bool? VisionNumber { get; set; }
        public bool? RevisionStatus { get; set; }
        public bool? Groups { get; set; }
        public int? BinderId { get; set; }

        public virtual Binder Binder { get; set; }
        public virtual SimpleUser User { get; set; }
    }
}

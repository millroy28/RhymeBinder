using System;
using System.Collections.Generic;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class TextHeader
    {
        public TextHeader()
        {
            EditWindowProperties = new HashSet<EditWindowProperty>();
            GroupHistories = new HashSet<GroupHistory>();
            InverseVersionOfNavigation = new HashSet<TextHeader>();
            LnkTextHeadersTextGroups = new HashSet<LnkTextHeadersTextGroup>();
            LnkTextSubmissions = new HashSet<LnkTextSubmission>();
            Submissions = new HashSet<Submission>();
            TextRecords = new HashSet<TextRecord>();
        }
        public int TextHeaderId { get; set; }
        public int TextId { get; set; }
        public string Title { get; set; }
        public DateTime? Created { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastRead { get; set; }
        public int? LastReadBy { get; set; }
        public int? TextRevisionStatusId { get; set; }
        public int? VisionNumber { get; set; }
        public int? VersionOf { get; set; }
        public bool? Deleted { get; set; }
        public bool? Locked { get; set; }
        public bool? Top { get; set; }
        public int? BinderId { get; set; }

        public virtual Binder Binder { get; set; }
        public virtual SimpleUser CreatedByNavigation { get; set; }
        public virtual SimpleUser LastModifiedByNavigation { get; set; }
        public virtual SimpleUser LastReadByNavigation { get; set; }
        public virtual Text Text { get; set; }
        public virtual TextRevisionStatus TextRevisionStatus { get; set; }
        public virtual TextHeader VersionOfNavigation { get; set; }
        public virtual ICollection<EditWindowProperty> EditWindowProperties { get; set; }
        public virtual ICollection<GroupHistory> GroupHistories { get; set; }
        public virtual ICollection<TextHeader> InverseVersionOfNavigation { get; set; }
        public virtual ICollection<LnkTextHeadersTextGroup> LnkTextHeadersTextGroups { get; set; }
        public virtual ICollection<LnkTextSubmission> LnkTextSubmissions { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
        public virtual ICollection<TextRecord> TextRecords { get; set; }
    }
}

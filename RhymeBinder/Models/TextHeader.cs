using System;
using System.Collections.Generic;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class TextHeader
    {
        public TextHeader()
        {
            InverseVersionOfNavigation = new HashSet<TextHeader>();
            LnkTextSubmissions = new HashSet<LnkTextSubmission>();
            Submissions = new HashSet<Submission>();
            TextRecords = new HashSet<TextRecord>();
        }

        public int TextHeaderId { get; set; }
        public int TextGroupId { get; set; }
        public int TextId { get; set; }
        public string Title { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime? LastRead { get; set; }
        public int? TextRevisionStatusId { get; set; }
        public int? VisionNumber { get; set; }
        public int? VersionOf { get; set; }
        public bool? Deleted { get; set; }
        public bool? Locked { get; set; }
        public bool? Top { get; set; }

        public virtual Text Text { get; set; }
        public virtual TextGroup TextGroup { get; set; }
        public virtual TextRevisionStatus TextRevisionStatus { get; set; }
        public virtual TextHeader VersionOfNavigation { get; set; }
        public virtual ICollection<TextHeader> InverseVersionOfNavigation { get; set; }
        public virtual ICollection<LnkTextSubmission> LnkTextSubmissions { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
        public virtual ICollection<TextRecord> TextRecords { get; set; }
    }
}

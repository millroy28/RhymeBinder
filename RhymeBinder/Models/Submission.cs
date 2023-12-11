using System;
using System.Collections.Generic;

namespace RhymeBinder.Models
{
    public partial class Submission
    {
        public Submission()
        {
            LnkTextSubmissions = new HashSet<LnkTextSubmission>();
        }

        public int SubmissionId { get; set; }
        public int PublicationId { get; set; }
        public int SubmissionStatusId { get; set; }
        public int TextHeaderId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime? Submitted { get; set; }
        public DateTime? Reply { get; set; }

        public virtual Publication Publication { get; set; }
        public virtual SubmissionStatus SubmissionStatus { get; set; }
        public virtual TextHeader TextHeader { get; set; }
        public virtual ICollection<LnkTextSubmission> LnkTextSubmissions { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace RhymeBinder.Models
{
    public partial class SubmissionStatus
    {
        public SubmissionStatus()
        {
            Submissions = new HashSet<Submission>();
        }

        public int SubmissionStatusId { get; set; }
        public string SubmissionStatus1 { get; set; }

        public virtual ICollection<Submission> Submissions { get; set; }
    }
}

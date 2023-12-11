using System;
using System.Collections.Generic;

namespace RhymeBinder.Models
{
    public partial class LnkTextSubmission
    {
        public int LnkTextSumbissionId { get; set; }
        public int TextHeaderId { get; set; }
        public int SubmissionId { get; set; }
        public DateTime? Created { get; set; }

        public virtual Submission Submission { get; set; }
        public virtual TextHeader TextHeader { get; set; }
    }
}

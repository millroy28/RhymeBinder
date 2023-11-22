
using System.Collections.Generic;


#nullable disable

namespace RhymeBinder.Models
{
    public partial class Publication
    {
        public Publication()
        {
            Submissions = new HashSet<Submission>();
        }
        public int PublicationId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? PublicationTypeId { get; set; }
        public int? PublicationRatingId { get; set; }

        public virtual PublicationRating PublicationRating { get; set; }
        public virtual PublicationType PublicationType { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
    }
}

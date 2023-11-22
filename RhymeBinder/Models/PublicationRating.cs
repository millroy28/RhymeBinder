using System.Collections.Generic;


#nullable disable

namespace RhymeBinder.Models
{
    public partial class PublicationRating
    {
        public PublicationRating()
        {
            Publications = new HashSet<Publication>();
        }
        public int PublicationRatingId { get; set; }
        public string PublicationRating1 { get; set; }

        public virtual ICollection<Publication> Publications { get; set; }
    }
}

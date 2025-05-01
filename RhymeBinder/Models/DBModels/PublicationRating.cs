using System;
using System.Collections.Generic;

namespace RhymeBinder.Models.DBModels
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

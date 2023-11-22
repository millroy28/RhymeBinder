﻿using System.Collections.Generic;


#nullable disable

namespace RhymeBinder.Models
{
    public partial class PublicationType
    {
        public PublicationType()
        {
            Publications = new HashSet<Publication>();
        }
        public int PublicationTypeId { get; set; }
        public string PublicationType1 { get; set; }

        public virtual ICollection<Publication> Publications { get; set; }
    }
}

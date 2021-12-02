using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RhymeBinder.Models
{
    public class SimpleTextHeaderAndText
    {
        public string Title { get; set; }
        public string TextBody { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? LastModified { get; set; }

        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

    }
}

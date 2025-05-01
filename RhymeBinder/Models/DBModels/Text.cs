using System;
using System.Collections.Generic;

namespace RhymeBinder.Models.DBModels
{
    public partial class Text
    {
        public Text()
        {
            TextHeaders = new HashSet<TextHeader>();
            TextRecords = new HashSet<TextRecord>();
        }

        public int TextId { get; set; }
        public string TextBody { get; set; }
        public DateTime? Created { get; set; }

        public virtual ICollection<TextHeader> TextHeaders { get; set; }
        public virtual ICollection<TextRecord> TextRecords { get; set; }
    }
}

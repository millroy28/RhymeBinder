using System;
using System.Collections.Generic;
using RhymeBinder.Models.DBModels;

namespace RhymeBinder.Models.DTOModels
{
    public partial class TextHeaderTitleDefaultType
    {
        public TextHeaderTitleDefaultType()
        {
            Binders = new HashSet<Binder>();
        }

        public int TextHeaderTitleDefaultTypeId { get; set; }
        public string TextHeaderTitleDefaultType1 { get; set; }

        public virtual ICollection<Binder> Binders { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace RhymeBinder.Models
{
    public partial class LnkTextHeadersBinder
    {
        public int LnkTextHeadersBindersId { get; set; }
        public int? TextHeaderId { get; set; }
        public int? BinderId { get; set; }

        public virtual Binder Binder { get; set; }
        public virtual TextHeader TextHeader { get; set; }
    }
}

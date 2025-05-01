using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RhymeBinder.Models.DBModels;

namespace RhymeBinder.Models.ViewModels
{
    public class DisplayTextGroup : TextGroup
    {
        public int HeaderCount { get; set; }
        public string BinderName { get; set; }
        public bool? Selected { get; set; }
        public List<int> LinkedTextHeaderIds { get; set; }

    }
}

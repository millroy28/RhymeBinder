using System.Collections.Generic;
using RhymeBinder.Models.DBModels;
using RhymeBinder.Models.DTOModels;

namespace RhymeBinder.Models.ViewModels
{
    public class DisplayBinderTextGroup : Binder
    {
        public List<TextGroupCount> GroupCount { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RhymeBinder.Models.DTOModels
{
    public class TextGroupSummary
    {
        public int TextGroupId { get; set; }
        public int SavedViewId { get; set; }
        public string GroupTitle { get; set; }
        
    }


}

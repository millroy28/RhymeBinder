using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RhymeBinder.Models.DBModels;

namespace RhymeBinder.Models.DTOModels
{
    public class TextGroupCount : TextGroup
    {
        public int Count { get; set; }
    }
}

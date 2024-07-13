﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RhymeBinder.Models
{
    public class DisplayTextHeadersAndSavedView
    {
        public List<DisplayTextHeader> TextHeaders { get; set; }
        public SavedView View { get; set; }
        public List<DisplayTextGroup> Groups { get; set; }
        public DisplayBinder Binder { get; set; }
        public List<Binder> UserBinders { get; set; }
        public string MenuTitle { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int LowIndex { get; set; }
        public int HighIndex { get; set; }
        public int TotalHeaders { get; set; }
    }
}

using System.Collections.Generic;
using RhymeBinder.Models.DBModels;

namespace RhymeBinder.Models.ViewModels
{
    public class DisplaySimpleUser : SimpleUser
    {
        public List<TimeZone> TimeZones { get; set; }
    }
}
